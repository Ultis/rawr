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
        private RogueTalents talents;

		public CalculationOptionsPanelRogue()
		{
			InitializeComponent();

            talents = new RogueTalents(this);

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

            if (talents != null) talents.LoadCalculationOptions();

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
			//if (Character != null && Character.AllTalents != null && Character.AllTalents.Trees != null && Character.AllTalents.Trees.Count > 0) {
			//    talents.Show();
			//}
            //else {
                MessageBox.Show("Error: There are no talents, which is a problem you know!");
            //}
        }

        private void OnCheckedChanged(object sender, EventArgs e) {
        }

        private void OnMHPoisonChanged(object sender, EventArgs e) {
            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempMainHandEnchant = ((ComboBox)sender).SelectedItem.ToString();
            }
        }

        private void OnOHPoisonChanged(object sender, EventArgs e) {
            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempOffHandEnchant = ((ComboBox)sender).SelectedItem.ToString();
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
            DPSCycle = new Cycle("4s5r");

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
                        ImprovedEviscerate = int.Parse(talentCode.Substring(0, 1));
                        RemorselessAttacks = int.Parse(talentCode.Substring(1, 1));
                        Malice = int.Parse(talentCode.Substring(2, 1));
                        Ruthlessness = int.Parse(talentCode.Substring(3, 1));
                        Murder = int.Parse(talentCode.Substring(4, 1));
                        PuncturingWounds = int.Parse(talentCode.Substring(5, 1));
                        RelentlessStrikes = int.Parse(talentCode.Substring(6, 1));
                        ImprovedExposeArmor = int.Parse(talentCode.Substring(7, 1));
                        Lethality = int.Parse(talentCode.Substring(8, 1));
                        VilePoisons = int.Parse(talentCode.Substring(9, 1));
                        ImprovedPoisons = int.Parse(talentCode.Substring(10, 1));
                        FleetFooted = int.Parse(talentCode.Substring(11, 1));
                        ColdBlood = int.Parse(talentCode.Substring(12, 1));
                        ImprovedKidneyShot = int.Parse(talentCode.Substring(13, 1));
                        QuickRecovery = int.Parse(talentCode.Substring(14, 1));
                        SealFate = int.Parse(talentCode.Substring(15, 1));
                        MasterPoisoner = int.Parse(talentCode.Substring(16, 1));
                        Vigor = int.Parse(talentCode.Substring(17, 1));
                        DeadenedNerves = int.Parse(talentCode.Substring(18, 1));
                        FindWeakness = int.Parse(talentCode.Substring(19, 1));
                        Mutilate = int.Parse(talentCode.Substring(20, 1));

                        ImprovedGouge = int.Parse(talentCode.Substring(21, 1));
                        ImprovedSinisterStrike = int.Parse(talentCode.Substring(22, 1));
                        LightningReflexes = int.Parse(talentCode.Substring(23, 1));
                        ImprovedSliceandDice = int.Parse(talentCode.Substring(24, 1));
                        Deflection = int.Parse(talentCode.Substring(25, 1));
                        Precision = int.Parse(talentCode.Substring(26, 1));
                        Endurance = int.Parse(talentCode.Substring(27, 1));
                        Riposte = int.Parse(talentCode.Substring(28, 1));
                        ImprovedSprint = int.Parse(talentCode.Substring(29, 1));
                        ImprovedKick = int.Parse(talentCode.Substring(30, 1));
                        DaggerSpecialization = int.Parse(talentCode.Substring(31, 1));
                        DualWieldSpecialization = int.Parse(talentCode.Substring(32, 1));
                        MaceSpecialization = int.Parse(talentCode.Substring(33, 1));
                        BladeFlurry = int.Parse(talentCode.Substring(34, 1));
                        SwordSpecialization = int.Parse(talentCode.Substring(35, 1));
                        FistSpecialization = int.Parse(talentCode.Substring(36, 1));
                        BladeTwisting = int.Parse(talentCode.Substring(37, 1));
                        WeaponExpertise = int.Parse(talentCode.Substring(38, 1));
                        Aggression = int.Parse(talentCode.Substring(39, 1));
                        Vitality = int.Parse(talentCode.Substring(40, 1));
                        AdrenalineRush = int.Parse(talentCode.Substring(41, 1));
                        NervesOfSteel = int.Parse(talentCode.Substring(42, 1));
                        CombatPotency = int.Parse(talentCode.Substring(43, 1));
                        SurpriseAttacks = int.Parse(talentCode.Substring(44, 1));

                        MasterOfDeception = int.Parse(talentCode.Substring(45, 1));
                        Opportunity = int.Parse(talentCode.Substring(46, 1));
                        SleightOfHand = int.Parse(talentCode.Substring(47, 1));
                        DirtyTricks = int.Parse(talentCode.Substring(48, 1));
                        Camouflage = int.Parse(talentCode.Substring(49, 1));
                        Initiative = int.Parse(talentCode.Substring(50, 1));
                        GhostlyStrike = int.Parse(talentCode.Substring(51, 1));
                        ImprovedAmbush = int.Parse(talentCode.Substring(52, 1));
                        Setup = int.Parse(talentCode.Substring(53, 1));
                        Elusiveness = int.Parse(talentCode.Substring(54, 1));
                        SerratedBlades = int.Parse(talentCode.Substring(55, 1));
                        HeightenedSenses = int.Parse(talentCode.Substring(56, 1));
                        Preparation = int.Parse(talentCode.Substring(57, 1));
                        DirtyDeeds = int.Parse(talentCode.Substring(58, 1));
                        Hemorrhage = int.Parse(talentCode.Substring(59, 1));
                        MasterOfSubtlety = int.Parse(talentCode.Substring(60, 1));
                        Deadliness = int.Parse(talentCode.Substring(61, 1));
                        EnvelopingShadows = int.Parse(talentCode.Substring(62, 1));
                        Premeditation = int.Parse(talentCode.Substring(63, 1));
                        CheatDeath = int.Parse(talentCode.Substring(64, 1));
                        SinisterCalling = int.Parse(talentCode.Substring(65, 1));
                        Shadowstep = int.Parse(talentCode.Substring(66, 1));
                        TalentsSaved = true;
                    }
                }
            }
            catch (Exception) {
            }
            #endregion
        }

        public int GetTalentByName(string name) {
            Type t = typeof(CalculationOptionsRogue);
            return (int)t.GetProperty(name).GetValue(this, null);
        }

        public void SetTalentByName(string name, int value) {
            Type t = typeof(CalculationOptionsRogue);
            t.GetProperty(name).SetValue(this, value, null);
        }

        public int TargetLevel = 73;
        public int TargetArmor = 7700;
        public Cycle DPSCycle;
        public string TempMainHandEnchant;
        public string TempOffHandEnchant;

        public bool TalentsSaved = false;

        // assassination talents
        public int ImprovedEviscerate { get; set; }
        public int RemorselessAttacks { get; set; }
        public int Malice { get; set; }
        public int Ruthlessness { get; set; }
        public int Murder { get; set; }
        public int PuncturingWounds { get; set; }
        public int RelentlessStrikes { get; set; }
        public int ImprovedExposeArmor { get; set; }
        public int Lethality { get; set; }
        public int VilePoisons { get; set; }
        public int ImprovedPoisons { get; set; }
        public int FleetFooted { get; set; }
        public int ColdBlood { get; set; }
        public int ImprovedKidneyShot { get; set; }
        public int QuickRecovery { get; set; }
        public int SealFate { get; set; }
        public int MasterPoisoner { get; set; }
        public int Vigor { get; set; }
        public int DeadenedNerves { get; set; }
        public int FindWeakness { get; set; }
        public int Mutilate { get; set; }

        // combat talents
        public int ImprovedGouge { get; set; }
        public int ImprovedSinisterStrike { get; set; }
        public int LightningReflexes { get; set; }
        public int ImprovedSliceandDice { get; set; }
        public int Deflection { get; set; }
        public int Precision { get; set; }
        public int Endurance { get; set; }
        public int Riposte { get; set; }
        public int ImprovedSprint { get; set; }
        public int ImprovedKick { get; set; }
        public int DaggerSpecialization { get; set; }
        public int DualWieldSpecialization { get; set; }
        public int MaceSpecialization { get; set; }
        public int BladeFlurry { get; set; }
        public int SwordSpecialization { get; set; }
        public int FistSpecialization { get; set; }
        public int BladeTwisting { get; set; }
        public int WeaponExpertise { get; set; }
        public int Aggression { get; set; }
        public int Vitality { get; set; }
        public int AdrenalineRush { get; set; }
        public int NervesOfSteel { get; set; }
        public int CombatPotency { get; set; }
        public int SurpriseAttacks { get; set; }

        // subtley talents
        public int MasterOfDeception { get; set; }
        public int Opportunity { get; set; }
        public int SleightOfHand { get; set; }
        public int DirtyTricks { get; set; }
        public int Camouflage { get; set; }
        public int Initiative { get; set; }
        public int GhostlyStrike { get; set; }
        public int ImprovedAmbush { get; set; }
        public int Setup { get; set; }
        public int Elusiveness { get; set; }
        public int SerratedBlades { get; set; }
        public int HeightenedSenses { get; set; }
        public int Preparation { get; set; }
        public int DirtyDeeds { get; set; }
        public int Hemorrhage { get; set; }
        public int MasterOfSubtlety { get; set; }
        public int Deadliness { get; set; }
        public int EnvelopingShadows { get; set; }
        public int Premeditation { get; set; }
        public int CheatDeath { get; set; }
        public int SinisterCalling { get; set; }
        public int Shadowstep { get; set; } // and I hope it stays that way
    }
}