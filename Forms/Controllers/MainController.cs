using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Rawr.Forms.Utilities;
using System.Reflection;

namespace Rawr.Forms.Controllers
{
	public class MainController
	{
		private Control _Parent;
        private const string BASE_TITLE = "Rawr (Beta {0})";
        private static readonly string versionNumber;

        static MainController()
        {
            Version version = Assembly.GetCallingAssembly().GetName().Version;
            versionNumber = version.Minor.ToString() +"."+ version.Build.ToString();
        }

		public MainController(Control parent)
		{
			_Parent = parent;
		}

        public string BaseTitle
        {
            get { return string.Format(BASE_TITLE, versionNumber); }
        }

        public void LoadModel(string displayName)
		{
			try
			{
				Calculations.LoadModel(Calculations.Models[displayName]);
			}
			finally
			{
				this.ConfigModel = displayName;
			}
		}

		public string ConfigModel
		{
			get
			{
				return Calculations.ValidModel(Properties.Recent.Default.RecentModel);
			}
			set { Properties.Recent.Default.RecentModel = value; }
		}

		public string[] ConfigRecentCharacters
		{
			get
			{
				string recentCharacters = Properties.Recent.Default.RecentFiles;
				if (string.IsNullOrEmpty(recentCharacters))
				{
					return new string[0];
				}
				else
				{
					return recentCharacters.Split(';');
				}
			}
			set { Properties.Recent.Default.RecentFiles = string.Join(";", value); }
		}

		public void AddRecentCharacter(string character)
		{
			List<string> recentCharacters = new List<string>(ConfigRecentCharacters);
			recentCharacters.Remove(character);
			recentCharacters.Add(character);
			while (recentCharacters.Count > 8)
				recentCharacters.RemoveRange(0, recentCharacters.Count - 8);
			ConfigRecentCharacters = recentCharacters.ToArray();
		}

		public void UpdateAllCachedItems()
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("UpdateAllCachedItems", "Beginning Update");
			StatusMessaging.UpdateStatus("CacheAllIcons", "Not Started");
			for (int i = 0; i < ItemCache.AllItems.Length; i++)
			{
				Item item = ItemCache.AllItems[i];
				StatusMessaging.UpdateStatus("UpdateAllCachedItems", "Updating " + i + " of " + ItemCache.AllItems.Length + " items");
				if (item.Id < 90000)
				{
					Item newItem = Item.LoadFromId(item.GemmedId, true, "Refreshing",false);
					if (newItem == null)
					{
						ItemCache.AddItem(item, true, false);
					}
				}

			}
			StatusMessaging.UpdateStatusFinished("UpdateAllCachedItems");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
		}

		public void GetArmoryUpgrades(Character currentCharacter)
		{
            WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("GetArmoryUpgrades", "Getting Armory Updates");
			Armory.LoadUpgradesFromArmory(currentCharacter);
			ItemCache.OnItemsChanged();
            StatusMessaging.UpdateStatusFinished("GetArmoryUpgrades");
		}


        public Character ReloadCharacterFromArmory(Character character)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            Character reload = GetCharacterFromArmory(character.Realm, character.Name, character.Region);
            if (reload != null)
            {
                //load values for gear from armory into original character
                foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
                {
                    character[slot] = reload[slot];
                }
            }
            return character;
        }

        public Character GetCharacterFromArmory(string realm, string name, Character.CharacterRegion region)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            StatusMessaging.UpdateStatus("GetCharacterFromArmory", " Getting Character Definition");
            StatusMessaging.UpdateStatus("CheckingItemCache", "Queued");
            string[] itemsOnChar;
            Character character = Armory.GetCharacter(region, realm, name, out itemsOnChar);
            StatusMessaging.UpdateStatusFinished("GetCharacterFromArmory");
            EnsureItemsLoaded(itemsOnChar);
            return character;
        }

        public void EnsureItemsLoaded(string[] ids)
        {
            StatusMessaging.UpdateStatus("CheckingItemCache", "Checking Item Cache for Definitions");
            if (ids != null)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    StatusMessaging.UpdateStatus("CheckingItemCache", string.Format("Checking Item Cache for Definitions - {0} of {1}", i, ids.Length));
                    Item.LoadFromId(ids[i], false, "Character from Armory", false);
                }
            }
            ItemCache.OnItemsChanged();
            StatusMessaging.UpdateStatusFinished("CheckingItemCache");
        }

        public void EnsureItemsLoaded(Character character)
        {
            EnsureItemsLoaded(character.GetAllEquipedGearIds());
        }

        public Character LoadSavedCharacter(string filePath)
        {
            Character ret = Character.Load(filePath);
            if (ret != null)
            {
                EnsureItemsLoaded(ret);
            }
            return ret;
        }
	}
}
