using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Reflection;

namespace Rawr
{
	public class ElitistArmoryService
	{
		private const string URL_REQ = "http://www.elitistarmory.com/rawr/req/{0}/{1}/{2}/{3}";
		private const string URL_QUEUE = "http://www.elitistarmory.com/rawr/queue/{0}/{1}/{2}";
		private const string URL_CHAR = "http://www.elitistarmory.com/rawr/char/{0}/{1}/{2}";
		private WebClient _webClient;

		public ElitistArmoryService()
		{
			_webClient = new WebClient();
			_webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
			_queueTimer.Tick += new EventHandler(CheckQueueAsync);
		}

		public event EventHandler<EventArgs<string>> GetCharacterProgressChanged;
		private string _progress = "Requesting Character...";
		public string Progress
		{
			get { return _progress; }
			set
			{
				if (_progress != value)
				{
					_progress = value;
					if (GetCharacterProgressChanged != null)
					{
						GetCharacterProgressChanged(this, new EventArgs<string>(value));
					}
				}
			}
		}

		private bool _canceled = false;
		public void CancelAsync()
		{
			_webClient.CancelAsync();
			_canceled = true;
		}

		public event EventHandler<EventArgs<Character>> GetCharacterCompleted;
		public void GetCharacterAsync(CharacterRegion region, string realm, string name, bool forceRefresh)
		{
			_lastRegion = region;
			_lastRealm = realm;
			_lastName = name;
			_canceled = false;
			_webClient.DownloadStringAsync(new Uri(string.Format(URL_REQ,
				region.ToString().ToLower(), realm.ToLower(), name.ToLower(), forceRefresh ? "1" : "0")));
			this.Progress = "Downloading Character Data...";
		}

		private DispatcherTimer _queueTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
		private CharacterRegion _lastRegion;
		private string _lastRealm;
		private string _lastName;
		private void CheckQueueAsync(object sender, EventArgs e)
		{
			_queueTimer.Stop();
			if (!_canceled)
			{
				_webClient.DownloadStringAsync(new Uri(string.Format(URL_QUEUE,
					_lastRegion.ToString().ToLower(), _lastRealm.ToLower(), _lastName.ToLower())));
				this.Progress = "Downloading Character Data...";
			}
		}

		private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (!e.Cancelled)
			{
				XDocument xdoc;
				using (StringReader sr = new StringReader(e.Result))
				{
					xdoc = XDocument.Load(sr);
				}
				if (xdoc.Root.Name == "queue")
				{
					Progress = "Queued (Postion: " + xdoc.Root.Attribute("position").Value + ")";
					_queueTimer.Start();
				}
				else
				{
					Progress = "Parsing Character Data...";
					BackgroundWorker bwParse = new BackgroundWorker();
					bwParse.DoWork += new DoWorkEventHandler(ParseCharacterXDoc);
					bwParse.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParse_RunWorkerCompleted);
					bwParse.RunWorkerAsync(xdoc);
				}
			}
		}

		void bwParse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			KeyValuePair<Character, Dictionary<CharacterSlot, ItemInstance>> kvp = (KeyValuePair<Character, Dictionary<CharacterSlot, ItemInstance>>)e.Result;
			Character character = kvp.Key;
			Dictionary<CharacterSlot, ItemInstance> items = kvp.Value;

			foreach (KeyValuePair<CharacterSlot, ItemInstance> item in items)
			{
				character[item.Key] = item.Value;

				if (item.Value.Id > 0 && !character.AvailableItems.Contains(item.Value.Id.ToString()))
					character.AvailableItems.Add(item.Value.Id.ToString());
				if (item.Value.Enchant != null && item.Value.EnchantId > 0)
				{
					string enchantString = (-1 * (item.Value.Enchant.Id + (10000 * (int)item.Value.Enchant.Slot))).ToString();
					if (!character.AvailableItems.Contains(enchantString))
						character.AvailableItems.Add(enchantString);
				}
			}

			Progress = "Complete!";
			if (this.GetCharacterCompleted != null)
				this.GetCharacterCompleted(this, new EventArgs<Character>(character));
		}

		private void ParseCharacterXDoc(object sender, DoWorkEventArgs e)
		{
			XDocument xdoc = e.Argument as XDocument;
			Character character = new Character();
			
			XElement xchar = xdoc.Root.Element("character");
			character.Region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), xchar.Attribute("region").Value, true);
			character.Realm = xchar.Attribute("realm").Value;
			character.Name = xchar.Attribute("name").Value;
			character.ClassIndex = int.Parse(xchar.Attribute("class_id").Value);
			character.RaceIndex = int.Parse(xchar.Attribute("race_id").Value);
			if (character.Race == CharacterRace.Draenei) character.ActiveBuffs.Add(Buff.GetBuffByName("Heroic Presence"));

			Dictionary<CharacterSlot, ItemInstance> items = new Dictionary<CharacterSlot, ItemInstance>();
			foreach (XElement xequipment in xchar.Element("equipInfo").Elements("equipment"))
			{
				ItemInstance item = new ItemInstance(string.Format("{0}.{1}.{2}.{3}.{4}",
					(xequipment.Attribute("item_id") ?? new XAttribute("item_id", "0")).Value,
					(xequipment.Attribute("gem1_id") ?? new XAttribute("gem1_id", "0")).Value,
					(xequipment.Attribute("gem2_id") ?? new XAttribute("gem2_id", "0")).Value,
					(xequipment.Attribute("gem3_id") ?? new XAttribute("gem3_id", "0")).Value,
					(xequipment.Attribute("enchant_id") ?? new XAttribute("enchant_id", "0")).Value));
				items[Character.GetCharacterSlotFromId(int.Parse(xequipment.Attribute("slot").Value) + 1)] = item;
			}

			foreach (XElement xtalent in xchar.Element("talentInfo").Elements("talent"))
			{
				if (xtalent.Attribute("active").Value == "1")
				{
					string talentCode = xtalent.Attribute("value").Value;
					switch (character.Class)
					{
						case CharacterClass.Warrior:
							character.WarriorTalents = new WarriorTalents(talentCode);
							if (character.WarriorTalents.Devastate > 0) character.CurrentModel = "ProtWarr";
							else character.CurrentModel = "DPSWarr";
							break;
						case CharacterClass.Paladin:
							character.PaladinTalents = new PaladinTalents(talentCode);
							if (character.PaladinTalents.HolyShield > 0) character.CurrentModel = "ProtPaladin";
							else if (character.PaladinTalents.CrusaderStrike > 0) character.CurrentModel = "Retribution";
							else character.CurrentModel = "Healadin";
							break;
						case CharacterClass.Hunter:
							character.HunterTalents = new HunterTalents(talentCode);
							character.CurrentModel = "Hunter";
							break;
						case CharacterClass.Rogue:
							character.RogueTalents = new RogueTalents(talentCode);
							character.CurrentModel = "Rogue";
							break;
						case CharacterClass.Priest:
							character.PriestTalents = new PriestTalents(talentCode);
							if (character.PriestTalents.Shadowform > 0) character.CurrentModel = "ShadowPriest";
							else character.CurrentModel = "HolyPriest";
							break;
						case CharacterClass.Shaman:
							character.ShamanTalents = new ShamanTalents(talentCode);
							if (character.ShamanTalents.ElementalMastery > 0) character.CurrentModel = "Elemental";
							else if (character.ShamanTalents.Stormstrike > 0) character.CurrentModel = "Enhance";
							else character.CurrentModel = "RestoSham";
							break;
						case CharacterClass.Mage:
							character.MageTalents = new MageTalents(talentCode);
							character.CurrentModel = "Mage";
							break;
						case CharacterClass.Warlock:
							character.WarlockTalents = new WarlockTalents(talentCode);
							character.CurrentModel = "Warlock";
							break;
						case CharacterClass.Druid:
							character.DruidTalents = new DruidTalents(talentCode);
							if (character.DruidTalents.ProtectorOfThePack > 0) character.CurrentModel = "Bear";
							else if (character.DruidTalents.LeaderOfThePack > 0) character.CurrentModel = "Cat";
							else if (character.DruidTalents.MoonkinForm > 0) character.CurrentModel = "Moonkin";
							else character.CurrentModel = "Tree";
							break;
						case CharacterClass.DeathKnight:
							character.DeathKnightTalents = new DeathKnightTalents(talentCode);
							if (character.DeathKnightTalents.Anticipation > 0) character.CurrentModel = "TankDK";
							else character.CurrentModel = "DPSDK";
							break;
						default:
							break;
					}

					TalentsBase talents = character.CurrentTalents;
					Dictionary<string, PropertyInfo> glyphProperties = new Dictionary<string, PropertyInfo>();
					foreach (PropertyInfo pi in talents.GetType().GetProperties())
					{
						GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
						if (glyphDatas.Length > 0)
						{
							GlyphDataAttribute glyphData = glyphDatas[0];
							glyphProperties[glyphData.Name] = pi;
						}
					}

					foreach (XElement glyph in xtalent.Element("glyphInfo").Elements("glyph"))
					{
						PropertyInfo pi;
						if (glyphProperties.TryGetValue(glyph.Attribute("name").Value, out pi))
						{
							pi.SetValue(talents, true, null);
						}
					}
				}
			}

			//TODO: Implement pet talent info parsing

			foreach (XElement xprofession in xchar.Element("professionInfo").Elements("profession"))
			{   
				Profession profession = (Profession)(int.Parse(xprofession.Attribute("id").Value));
				if (character.PrimaryProfession == Profession.None)
					character.PrimaryProfession = profession;
				else
					character.SecondaryProfession = profession;

				switch (profession)
				{
					case Profession.Mining:
						character.ActiveBuffs.Add(Buff.GetBuffByName("Toughness"));
						break;
					case Profession.Skinning:
						character.ActiveBuffs.Add(Buff.GetBuffByName("Master of Anatomy"));
						break;
					case Profession.Blacksmithing:
						character.WristBlacksmithingSocketEnabled = true;
						character.HandsBlacksmithingSocketEnabled = true;
						break;
				}
			}

			Calculations.GetModel(character.CurrentModel).SetDefaults(character);

			e.Result = new KeyValuePair<Character, Dictionary<CharacterSlot, ItemInstance>>(character, items);
		}
	}
}





/*
<?xml version='1.0' encoding='utf-8' ?>
<characterInfo>
<character class_id='11' name='Astrylian' race_id='4' realm='Suramar' region='us'>
<equipInfo>
<equipment enchant_id='3818' enchant_item_id='44878' gem1_id='41380' gem2_id='40130' item_id='51143' slot='0'></equipment>
<equipment gem1_id='40130' item_id='47133' slot='1'></equipment>
<equipment enchant_id='3837' gem1_id='40119' item_id='51140' slot='2'></equipment>
<equipment enchant_id='3832' enchant_item_id='44465' gem1_id='40119' gem2_id='40119' gem3_id='40119' item_id='50001' slot='4'></equipment>
<equipment gem1_id='40119' gem2_id='40130' gem3_id='40119' item_id='50067' slot='5'></equipment>
<equipment enchant_id='3327' gem1_id='40119' gem2_id='40119' item_id='51142' slot='6'></equipment>
<equipment enchant_id='3232' enchant_item_id='39006' gem1_id='40119' gem2_id='40119' item_id='50607' slot='7'></equipment>
<equipment enchant_id='3757' gem1_id='40119' item_id='50333' slot='8'></equipment>
<equipment enchant_id='3260' enchant_item_id='34207' gem1_id='40119' item_id='51144' slot='9'></equipment>
<equipment gem1_id='40119' item_id='50404' slot='10'></equipment>
<equipment gem1_id='40119' item_id='47955' slot='11'></equipment>
<equipment item_id='47088' slot='12'></equipment>
<equipment item_id='47735' slot='13'></equipment>
<equipment enchant_id='3294' enchant_item_id='39001' gem1_id='40119' item_id='50466' slot='14'></equipment>
<equipment enchant_id='2673' enchant_item_id='35458' gem1_id='40130' gem2_id='40119' item_id='50040' slot='15'></equipment>
<equipment item_id='50456' slot='17'></equipment>
</equipInfo>
<talentInfo>
<talent active='1' value='0000000000000000000000000000503232130322010353120303213511203500010000000000000000000000000000000000'>
<glyphInfo>
<glyph id='162' name='Glyph of Maul' type='major'></glyph>
<glyph id='432' name='Glyph of Challenging Roar' type='minor'></glyph>
<glyph id='431' name='Glyph of Aquatic Form' type='minor'></glyph>
<glyph id='163' name='Glyph of Growling' type='major'></glyph>
<glyph id='434' name='Glyph of Unburdened Rebirth' type='minor'></glyph>
<glyph id='811' name='Glyph of Survival Instincts' type='major'></glyph>
</glyphInfo>
</talent>
<talent active='0' value='0000000000000000000000000000503202132322212053120003313511205500010000000000000000000000000000000000'>
<glyphInfo>
<glyph id='671' name='Glyph of Berserk' type='major'></glyph>
<glyph id='434' name='Glyph of Unburdened Rebirth' type='minor'></glyph>
<glyph id='551' name='Glyph of Dash' type='minor'></glyph>
<glyph id='811' name='Glyph of Survival Instincts' type='major'></glyph>
<glyph id='435' name='Glyph of Thorns' type='minor'></glyph>
<glyph id='164' name='Glyph of Mangle' type='major'></glyph>
</glyphInfo>
</talent>
</talentInfo>
<petTalentInfo>
</petTalentInfo>
<professionInfo>
<profession current='450' id='773'></profession>
<profession current='450' id='165'></profession>
</professionInfo>
</character>
</characterInfo>

*/