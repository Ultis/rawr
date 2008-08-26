using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Rawr.Rogue {
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase {
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelRogue()
		{
			InitializeComponent();

			armorBosses.Add(3800, ": Shade of Aran");
			armorBosses.Add(4700, ": Roar");
			armorBosses.Add(5500, ": Netherspite");
			armorBosses.Add(6100, ": Julianne, Curator");
			armorBosses.Add(6200, ": Karathress, Vashj, Solarian, Kael'thas, Winterchill, Anetheron, Kaz'rogal, Azgalor, Archimonde, Teron, Shahraz");
			armorBosses.Add(6700, ": Maiden, Illhoof");
			armorBosses.Add(7300, ": Strawman");
			armorBosses.Add(7500, ": Attumen");
			armorBosses.Add(7600, ": Romulo, Nightbane, Malchezaar, Doomwalker");
			armorBosses.Add(7700, ": Hydross, Lurker, Leotheras, Tidewalker, Al'ar, Naj'entus, Supremus, Akama, Gurtogg");
			armorBosses.Add(8200, ": Midnight");
			armorBosses.Add(8800, ": Void Reaver");            
		}

        private bool _loadingCalculationOptions = false;

        protected override void LoadCalculationOptions() {
            _loadingCalculationOptions = true;

            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRogue(Character);

            CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
            comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();

            _loadingCalculationOptions = false;
        }


        private void trackBarTargetArmor_Scroll(object sender, EventArgs e) {

        }

        private void trackBarTargetArmor_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
                labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");

                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetArmor = trackBarTargetArmor.Value;

                Character.OnItemsChanged();
            }
        }

        private void buttonTalents_Click(object sender, EventArgs e) {
            if (Character != null && Character.Talents != null && Character.Talents.Trees != null && Character.Talents.Trees.Count > 0) {
                RogueTalents rt = new RogueTalents();
                rt.SetTalents(Character);
                rt.Show();
            }
            else {
                MessageBox.Show("Error: There are no talents, which is a problem you know!");
            }
        }
    }

    [Serializable]
    public class CalculationOptionsRogue : ICalculationOptionBase {
        public string GetXml() {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }

        public CalculationOptionsRogue() { }
        public CalculationOptionsRogue(Character character): this() {

            // talent import from DPSWarr module
            #region Rogue Talents Import
            try {
                WebRequestWrapper wrw = new WebRequestWrapper();

                if (character.Class == Character.CharacterClass.Rogue && character.Name != null && character.Realm != null) {
                    XmlDocument docTalents = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm);

                    //<talentTab>
                    //  <talentTree value="2550050300230151333125100000000000000000000002030302010000000000000"/>
                    //</talentTab>
                    if (docTalents != null) {
                        string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                        TalentsSaved = true;
                        ImprovedEviscerate = int.Parse(talentCode.Substring(0, 1));
                        Malice = int.Parse(talentCode.Substring(2, 1));
                        Ruthlessness = int.Parse(talentCode.Substring(3, 1));
                        Murder = int.Parse(talentCode.Substring(4, 1));
                        PuncturingWounds = int.Parse(talentCode.Substring(5, 1));
                        RelentlessStrikes = int.Parse(talentCode.Substring(6, 1));
                        Lethality = int.Parse(talentCode.Substring(8, 1));
                        VilePoisons = int.Parse(talentCode.Substring(9, 1));
                        ImprovedPoisons = int.Parse(talentCode.Substring(10, 1));
                        ColdBlood = int.Parse(talentCode.Substring(12, 1));
                        QuickRecovery = int.Parse(talentCode.Substring(14, 1));
                        SealFate = int.Parse(talentCode.Substring(15, 1));
                        MasterPoisoner = int.Parse(talentCode.Substring(16, 1));
                        Vigor = int.Parse(talentCode.Substring(17, 1));
                        FindWeakness = int.Parse(talentCode.Substring(19, 1));

                        ImprovedSinisterStrike = int.Parse(talentCode.Substring(22, 1));
                        LightningReflexes = int.Parse(talentCode.Substring(23, 1));
                        ImprovedSliceandDice = int.Parse(talentCode.Substring(24, 1));
                        Deflection = int.Parse(talentCode.Substring(25, 1));
                        Precision = int.Parse(talentCode.Substring(26, 1));
                        DaggerSpecialization = int.Parse(talentCode.Substring(31, 1));
                        DualWieldSpecialization = int.Parse(talentCode.Substring(32, 1));
                        MaceSpecialization = int.Parse(talentCode.Substring(33, 1));
                        BladeFlurry = int.Parse(talentCode.Substring(34, 1));
                        SwordSpecialization = int.Parse(talentCode.Substring(35, 1));
                        FistSpecialization = int.Parse(talentCode.Substring(36, 1));
                        WeaponExpertise = int.Parse(talentCode.Substring(38, 1));
                        Aggression = int.Parse(talentCode.Substring(39, 1));
                        Vitality = int.Parse(talentCode.Substring(40, 1));
                        AdrenalineRush = int.Parse(talentCode.Substring(41, 1));
                        CombatPotency = int.Parse(talentCode.Substring(43, 1));
                        SurpriseAttacks = int.Parse(talentCode.Substring(44, 1));

                        Opportunity = int.Parse(talentCode.Substring(46, 1));
                        Initiative = int.Parse(talentCode.Substring(50, 1));
                        ImprovedAmbush = int.Parse(talentCode.Substring(52, 1));
                        Setup = int.Parse(talentCode.Substring(53, 1));
                        SerratedBlades = int.Parse(talentCode.Substring(55, 1));
                        Preparation = int.Parse(talentCode.Substring(57, 1));
                        DirtyDeeds = int.Parse(talentCode.Substring(58, 1));
                        MasterOfSubtlety = int.Parse(talentCode.Substring(60, 1));
                        Deadliness = int.Parse(talentCode.Substring(61, 1));
                        SinisterCalling = int.Parse(talentCode.Substring(65, 1));
                        Shadowstep = int.Parse(talentCode.Substring(66, 1));
                    }
                }
            }
            catch (Exception) {
            }
            #endregion
        }

        public int TargetLevel = 73;
        public int TargetArmor = 7700;

        public bool TalentsSaved = false;

        // assassination talents
        public int ImprovedEviscerate = 0;
        public int Malice = 0;
        public int Ruthlessness = 0;
        public int Murder = 0;
        public int PuncturingWounds = 0;
        public int RelentlessStrikes = 0;
        public int Lethality = 0;
        public int VilePoisons = 0;
        public int ImprovedPoisons = 0;
        public int ColdBlood = 0;
        public int QuickRecovery = 0;
        public int SealFate = 0;
        public int MasterPoisoner = 0;
        public int Vigor = 0;
        public int FindWeakness = 0;

        // combat talents
        public int ImprovedSinisterStrike = 0;
        public int LightningReflexes = 0;
        public int ImprovedSliceandDice = 0;
        public int Deflection = 0;
        public int Precision = 0;
        public int DaggerSpecialization = 0;
        public int DualWieldSpecialization = 0;
        public int MaceSpecialization = 0;
        public int BladeFlurry = 0;
        public int SwordSpecialization = 0;
        public int FistSpecialization = 0;
        public int WeaponExpertise = 0;
        public int Aggression = 0;
        public int Vitality = 0;
        public int AdrenalineRush = 0;
        public int CombatPotency = 0;
        public int SurpriseAttacks = 0;

        // subtley talents
        public int Opportunity = 0;
        public int Initiative = 0;
        public int ImprovedAmbush = 0;
        public int Setup = 0;
        public int SerratedBlades = 0;
        public int Preparation = 0;
        public int DirtyDeeds = 0;
        public int MasterOfSubtlety = 0;
        public int Deadliness = 0;
        public int SinisterCalling = 0;
        public int Shadowstep = 0; // and I hope it stays that way
    }
}