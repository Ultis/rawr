using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Hunter
{
    public partial class CalculationOptionsPanelHunter : CalculationOptionsPanelBase
    {
        #region Instance Variables

        private bool loadingOptions = false;
		private CalculationOptionsHunter options = null;
        #endregion

        #region Constructors

        public CalculationOptionsPanelHunter()
        {
            InitializeComponent();

            InitializeShotList(cmbPriority1);
            InitializeShotList(cmbPriority2);
            InitializeShotList(cmbPriority3);
            InitializeShotList(cmbPriority4);
            InitializeShotList(cmbPriority5);
            InitializeShotList(cmbPriority6);
            InitializeShotList(cmbPriority7);
            InitializeShotList(cmbPriority8);
            InitializeShotList(cmbPriority9);
            InitializeShotList(cmbPriority10);

        }

        private void InitializeShotList(ComboBox cb)
        {
            cb.Items.Add("None");
            cb.Items.Add("Aimed Shot");
            cb.Items.Add("Arcane Shot");
            cb.Items.Add("Multi-Shot");
            cb.Items.Add("Serpent Sting");
            cb.Items.Add("Scorpid Sting");
            cb.Items.Add("Viper Sting");
            cb.Items.Add("Silencing Shot");
            cb.Items.Add("Steady Shot");
            cb.Items.Add("Kill Shot");
            cb.Items.Add("Explosive Shot");
            cb.Items.Add("Black Arrow");
            cb.Items.Add("Immolation Trap");
            cb.Items.Add("Chimera Shot");
            cb.Items.Add("Rapid Fire");
            cb.Items.Add("Readiness");
            cb.Items.Add("Beastial Wrath");
            cb.Items.Add("Orc - Blood Fury");
            cb.Items.Add("Troll - Berserk");
        }

        #endregion

        #region Overrides

        protected override void LoadCalculationOptions()
        {
            loadingOptions = true;
			options = Character.CalculationOptions as CalculationOptionsHunter;
			if (options == null)
			{
				options = new CalculationOptionsHunter();
				Character.CalculationOptions = options;
			}
			for (int i = 0; i < cmbTargetLevel.Items.Count; i++)
			{
				if (cmbTargetLevel.Items[i] as string == options.TargetLevel.ToString())
				{
					cmbTargetLevel.SelectedItem = cmbTargetLevel.Items[i];
					break;
				}
			}
            comboPetFamily.SelectedItem = options.PetFamily;
            numericUpDownLatency.Value = (decimal)(options.Latency * 1000.0);
            numCobraReflexes.Value = (options.CobraReflexes);
            numSpikedCollar.Value = (options.SpikedCollar);
            numSpidersBite.Value = (options.SpidersBite);
            numRabid.Value = (options.Rabid);
            numCallOfTheWild.Value = (options.CallOfTheWild);
            numSharkAttack.Value = (options.SharkAttack);
            numWildHunt.Value = (options.WildHunt);
            trackBarTargetArmor.Value = options.TargetArmor;
            lblTargetArmorValue.Text = options.TargetArmor.ToString();

            foreach (PetFamily f in Enum.GetValues(typeof(PetFamily)))
                comboPetFamily.Items.Add(f);

            comboPetFamily.SelectedItem = PetFamily.Cat;

            numSpidersBite.Enabled = true;
            numRabid.Enabled = true;
            numCallOfTheWild.Enabled = true;
            numSharkAttack.Enabled = true;
            numCornered.Enabled = false;
            numFeedingFrenzy.Enabled = false;
            numWolverineBite.Enabled = false;
            numThunderstomp.Enabled = false;

            numCobraReflexes.Value = 2;
            options.CobraReflexes = 2;
            numSpikedCollar.Value = 3;
            options.SpikedCollar = 3;
            numSpidersBite.Value = 0;
            options.SpidersBite = 0;
            numRabid.Value = 0;
            options.Rabid = 0;
            numCallOfTheWild.Value = 0;
            options.CallOfTheWild = 0;
            numSharkAttack.Value = 0;
            options.SharkAttack = 0;
            numWildHunt.Value = 0;
            options.WildHunt = 0;
            numCornered.Value = 0;
            options.Cornered = 0;
            numFeedingFrenzy.Value = 0;
            options.FeedingFrenzy = 0;
            numWolverineBite.Value = 0;
            options.WolverineBite = 0;
            numThunderstomp.Value = 0;
            options.Thunderstomp = 0;
            options.duration = 360;

            PopulateAbilities();

            cmbPriority1.SelectedIndex = options.PriorityIndex1;
            cmbPriority2.SelectedIndex = options.PriorityIndex2;
            cmbPriority3.SelectedIndex = options.PriorityIndex3;
            cmbPriority4.SelectedIndex = options.PriorityIndex4;
            cmbPriority5.SelectedIndex = options.PriorityIndex5;
            cmbPriority6.SelectedIndex = options.PriorityIndex6;
            cmbPriority7.SelectedIndex = options.PriorityIndex7;
            cmbPriority8.SelectedIndex = options.PriorityIndex8;
            cmbPriority9.SelectedIndex = options.PriorityIndex9;
            cmbPriority10.SelectedIndex = options.PriorityIndex10;
            cmbPriorityDefaults.SelectedIndex = 0;

            loadingOptions = false;
        }

        #endregion

        #region Event Handlers

        private PetAttacks[] familyList = null;

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }

        private void PopulateAbilities()
        {
            switch (options.PetFamily)
            {
                case PetFamily.Bat:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.SonicBlast, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Bear:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Swipe, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.BirdOfPrey:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Snatch, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Boar:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Gore, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.CarrionBird:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Screech, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Cat:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Rake, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Chimera:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Frost, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.CoreHound:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Lava, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Crab:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Pin, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Crocolisk:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Attitude, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Devilsaur:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Monstrous, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Dragonhawk:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.FireBreath, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Gorilla:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Smack, PetAttacks.Pummel, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Hyena:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Tendon, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Moth:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Smack, PetAttacks.Serenity, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.NetherRay:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Shock, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Raptor:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Savage, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Ravager:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Ravage, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Rhino:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Smack, PetAttacks.Stampede, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Scorpid:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Scorpid, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Serpent:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Poison, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Silithid:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Web, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Spider:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Web, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.SpiritBeast:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Spirit, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.SporeBat:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Smack, PetAttacks.Spore, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Tallstrider:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Claw, PetAttacks.Dust, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Turtle:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Shell, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.WarpStalker:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Warp, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
                case PetFamily.Wasp:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Smack, PetAttacks.Sting, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.WindSerpent:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.LightningBreath, PetAttacks.Wolverine, PetAttacks.None };
                    break;
                case PetFamily.Wolf:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Howl, PetAttacks.Rabid, PetAttacks.None };
                    break;
                case PetFamily.Worm:
                    familyList = new PetAttacks[] { PetAttacks.Growl, PetAttacks.Bite, PetAttacks.Acid, PetAttacks.Thunderstomp, PetAttacks.None };
                    break;
            }
            comboBoxPet1.Items.Clear();
            comboBoxPet2.Items.Clear();
            comboBoxPet3.Items.Clear();
            comboBoxPet4.Items.Clear();

            foreach (PetAttacks A in familyList)
            {
                comboBoxPet1.Items.Add(A);
                comboBoxPet2.Items.Add(A);
                comboBoxPet3.Items.Add(A);
                comboBoxPet4.Items.Add(A);
            }
            comboBoxPet1.SelectedIndex = 1;
            comboBoxPet2.SelectedIndex = 2;
            comboBoxPet3.SelectedItem = PetAttacks.None;
            comboBoxPet4.SelectedItem = PetAttacks.None;
        }

        private void comboPetFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboPetFamily.SelectedItem != null)
            {
                options.PetFamily = (PetFamily)comboPetFamily.SelectedItem;
                PopulateAbilities();
                switch (options.PetFamily)
                {
                        //Cunning
                    case PetFamily.Bat:
                    case PetFamily.Chimera:
                    case PetFamily.Dragonhawk:
                    case PetFamily.NetherRay:
                    case PetFamily.Ravager:
                    case PetFamily.Serpent:
                    case PetFamily.Silithid:
                    case PetFamily.Spider:
                    case PetFamily.SporeBat:
                    case PetFamily.WindSerpent:
                        numSpidersBite.Enabled = false;
                        numRabid.Enabled = false;
                        numCallOfTheWild.Enabled = false;
                        numSharkAttack.Enabled = false;
                        numCornered.Enabled = true;
                        numFeedingFrenzy.Enabled = true;             
                        numWolverineBite.Enabled = true;
                        numThunderstomp.Enabled = false;

                        numCobraReflexes.Value = 2;
                        options.CobraReflexes = 2;
                        numSpikedCollar.Value = 3;
                        options.SpikedCollar = 3;
                        numSpidersBite.Value = 0;
                        options.SpidersBite = 0;
                        numRabid.Value = 0;
                        options.Rabid = 0;
                        numCallOfTheWild.Value = 0;
                        options.CallOfTheWild = 0;
                        numSharkAttack.Value = 0;
                        options.SharkAttack = 0;
                        numWildHunt.Value = 0;
                        options.WildHunt = 0;
                        numCornered.Value = 2;
                        options.Cornered = 2;
                        numFeedingFrenzy.Value = 2;
                        options.FeedingFrenzy = 2;
                        numWolverineBite.Value = 1;
                        options.WolverineBite = 1;
                        numThunderstomp.Value = 0;
                        options.Thunderstomp = 0;

                        break;
                        //Tenacity
                    case PetFamily.Bear:
                    case PetFamily.Boar:
                    case PetFamily.Crab:
                    case PetFamily.Crocolisk:
                    case PetFamily.Gorilla:
                    case PetFamily.Rhino:
                    case PetFamily.Scorpid:
                    case PetFamily.Turtle:
                    case PetFamily.WarpStalker:
                    case PetFamily.Worm:
                        numSpidersBite.Enabled = false;
                        numRabid.Enabled = false;
                        numCallOfTheWild.Enabled = false;
                        numSharkAttack.Enabled = false;
                        numCornered.Enabled = false;
                        numFeedingFrenzy.Enabled = false;
                        numWolverineBite.Enabled = false;
                        numThunderstomp.Enabled = true;

                        numCobraReflexes.Value = 2;
                        options.CobraReflexes = 0;
                        numSpikedCollar.Value = 0;
                        options.SpikedCollar = 0;
                        numSpidersBite.Value = 0;
                        options.SpidersBite = 0;
                        numRabid.Value = 0;
                        options.Rabid = 0;
                        numCallOfTheWild.Value = 0;
                        options.CallOfTheWild = 0;
                        numSharkAttack.Value = 0;
                        options.SharkAttack = 0;
                        numWildHunt.Value = 0;
                        options.WildHunt = 0;
                        numCornered.Value = 0;
                        options.Cornered = 0;
                        numFeedingFrenzy.Value = 0;
                        options.FeedingFrenzy = 0;
                        numWolverineBite.Value = 0;
                        options.WolverineBite = 0;
                        numThunderstomp.Value = 0;
                        options.Thunderstomp = 0;
                        break;
                        //Ferocity
                    case PetFamily.BirdOfPrey:
                    case PetFamily.CarrionBird:
                    case PetFamily.Cat:
                    case PetFamily.CoreHound:
                    case PetFamily.Devilsaur:
                    case PetFamily.Hyena:
                    case PetFamily.Moth:
                    case PetFamily.Raptor:
                    case PetFamily.SpiritBeast:
                    case PetFamily.Tallstrider:
                    case PetFamily.Wasp:
                    case PetFamily.Wolf:
                        numSpidersBite.Enabled = true;
                        numRabid.Enabled = true;
                        numCallOfTheWild.Enabled = true;
                        numSharkAttack.Enabled = true;
                        numCornered.Enabled = false;
                        numFeedingFrenzy.Enabled = false;
                        numWolverineBite.Enabled = false;
                        numThunderstomp.Enabled = false;

                        numCobraReflexes.Value = 2;
                        options.CobraReflexes = 2;
                        numSpikedCollar.Value = 3;
                        options.SpikedCollar = 3;
                        numSpidersBite.Value = 0;
                        options.SpidersBite = 0;
                        numRabid.Value = 0;
                        options.Rabid = 0;
                        numCallOfTheWild.Value = 0;
                        options.CallOfTheWild = 0;
                        numSharkAttack.Value = 0;
                        options.SharkAttack = 0;
                        numWildHunt.Value = 0;
                        options.WildHunt = 0;
                        numCornered.Value = 0;
                        options.Cornered = 0;
                        numFeedingFrenzy.Value = 0;
                        options.FeedingFrenzy = 0;
                        numWolverineBite.Value = 0;
                        options.WolverineBite = 0;
                        numThunderstomp.Value = 0;
                        options.Thunderstomp = 0;
                        break;
                }
                
                Character.OnCalculationsInvalidated();
            }
        }

        #endregion

        private void trackBarTargetArmor_Scroll(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.TargetArmor = trackBarTargetArmor.Value;
                Character.OnCalculationsInvalidated();
                lblTargetArmorValue.Text = trackBarTargetArmor.Value.ToString();
            }
        }

        private void numericUpDownLatency_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.Latency = (float)numericUpDownLatency.Value/1000.0f;
                Character.OnCalculationsInvalidated();
            }
        }
        
        private void duration_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
            	options.duration = (int)duration.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
            	options.CobraReflexes = (int)numCobraReflexes.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.SpikedCollar = (int)numSpikedCollar.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.SpidersBite = (int)numSpidersBite.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.Rabid = (int)numRabid.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.CallOfTheWild = (int)numCallOfTheWild.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.SharkAttack = (int)numSharkAttack.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.WildHunt = (int)numWildHunt.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.Cornered = (int)numCornered.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.FeedingFrenzy = (int)numFeedingFrenzy.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.Thunderstomp = (int)numWolverineBite.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.WolverineBite = (int)numThunderstomp.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numOwlsFocus_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.OwlsFocus = (int)numOwlsFocus.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxPet1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.PetPriority1 = (PetAttacks)comboBoxPet1.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }
        private void comboBoxPet2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.PetPriority2 = (PetAttacks)comboBoxPet2.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }
        private void comboBoxPet3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.PetPriority3 = (PetAttacks)comboBoxPet3.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }
        private void comboBoxPet4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.PetPriority4 = (PetAttacks)comboBoxPet4.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void PrioritySelectedIndexChanged(object sender, EventArgs e)
        {
            // this is called whenever any of the priority dropdowns are modified
            if (loadingOptions) return;

            options.PriorityIndex1 = cmbPriority1.SelectedIndex;
            options.PriorityIndex2 = cmbPriority2.SelectedIndex;
            options.PriorityIndex3 = cmbPriority3.SelectedIndex;
            options.PriorityIndex4 = cmbPriority4.SelectedIndex;
            options.PriorityIndex5 = cmbPriority5.SelectedIndex;
            options.PriorityIndex6 = cmbPriority6.SelectedIndex;
            options.PriorityIndex7 = cmbPriority7.SelectedIndex;
            options.PriorityIndex8 = cmbPriority8.SelectedIndex;
            options.PriorityIndex9 = cmbPriority9.SelectedIndex;
            options.PriorityIndex10 = cmbPriority10.SelectedIndex;

            cmbPriorityDefaults.SelectedIndex = 0;

            Character.OnCalculationsInvalidated();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void cmbPriorityDefaults_SelectedIndexChanged(object sender, EventArgs e)
        {
            // only do anything if we weren't set to 0
            if (cmbPriorityDefaults.SelectedIndex == 0) return;
            if (loadingOptions) return;

            loadingOptions = true;

            if (cmbPriorityDefaults.SelectedIndex == 1) // beast master
            {
                cmbPriority1.SelectedIndex = 14; // Rapid Fire
                cmbPriority2.SelectedIndex = 16; // Bestial Wrath
                cmbPriority3.SelectedIndex = 9; // Kill Shot
                cmbPriority4.SelectedIndex = 1; // Aimed Shot
                cmbPriority5.SelectedIndex = 2; // Arcane Shot
                cmbPriority6.SelectedIndex = 4; // Serpent Sting
                cmbPriority7.SelectedIndex = 8; // Steady Shot
                cmbPriority8.SelectedIndex = 0;
                cmbPriority9.SelectedIndex = 0;
                cmbPriority10.SelectedIndex = 0;
            }

            if (cmbPriorityDefaults.SelectedIndex == 2) // marksman
            {
                cmbPriority1.SelectedIndex = 14; // Rapid Fire
                cmbPriority2.SelectedIndex = 15; // Readiness
                cmbPriority3.SelectedIndex = 4; // Serpent Sting
                cmbPriority4.SelectedIndex = 13; // Chimera Shot
                cmbPriority5.SelectedIndex = 9; // Kill Shot
                cmbPriority6.SelectedIndex = 1; // Aimed Shot
                cmbPriority7.SelectedIndex = 7; // Silencing Shot
                cmbPriority8.SelectedIndex = 8; // Steady Shot
                cmbPriority9.SelectedIndex = 0;
                cmbPriority10.SelectedIndex = 0;
            }

            if (cmbPriorityDefaults.SelectedIndex == 3) // survival
            {
                cmbPriority1.SelectedIndex = 14; // Rapid Fire
                cmbPriority2.SelectedIndex = 9; // Kill Shot
                cmbPriority3.SelectedIndex = 10; // Explosive Shot
                cmbPriority4.SelectedIndex = 11; // Black Arrow
                cmbPriority5.SelectedIndex = 4; // Serpent Sting
                cmbPriority6.SelectedIndex = 1; // Aimed Shot
                cmbPriority7.SelectedIndex = 8; // Steady Shot
                cmbPriority8.SelectedIndex = 0;
                cmbPriority9.SelectedIndex = 0;
                cmbPriority10.SelectedIndex = 0;
            }

            loadingOptions = false;

            options.PriorityIndex1 = cmbPriority1.SelectedIndex;
            options.PriorityIndex2 = cmbPriority2.SelectedIndex;
            options.PriorityIndex3 = cmbPriority3.SelectedIndex;
            options.PriorityIndex4 = cmbPriority4.SelectedIndex;
            options.PriorityIndex5 = cmbPriority5.SelectedIndex;
            options.PriorityIndex6 = cmbPriority6.SelectedIndex;
            options.PriorityIndex7 = cmbPriority7.SelectedIndex;
            options.PriorityIndex8 = cmbPriority8.SelectedIndex;
            options.PriorityIndex9 = cmbPriority9.SelectedIndex;
            options.PriorityIndex10 = cmbPriority10.SelectedIndex;

            Character.OnCalculationsInvalidated();

        }
    }
}