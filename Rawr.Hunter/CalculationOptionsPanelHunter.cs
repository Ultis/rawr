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
            
            foreach (Enum e in Enum.GetValues(typeof(Shots)))
            {
                comboShotPrio1.Items.Add(e);
                comboShotPrio2.Items.Add(e);
                comboShotPrio3.Items.Add(e);
                //comboShotPrio4.Items.Add(e);
            }
            comboShotPrio4.Items.Add(Shots.SteadyShot);
            comboShotPrio4.SelectedIndex = 0;
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
            comboShotPrio1.SelectedItem = options.ShotPriority1;
            comboShotPrio2.SelectedItem = options.ShotPriority2;
            comboShotPrio3.SelectedItem = options.ShotPriority3;
            comboShotPrio4.SelectedItem = options.ShotPriority4;
            numericUpDownLatency.Value = (decimal)(options.Latency * 1000.0);
            numCobraReflexes.Value = (options.CobraReflexes);
            numSpikedCollar.Value = (options.SpikedCollar);
            numSpidersBite.Value = (options.SpidersBite);
            numRabid.Value = (options.Rabid);
            numCallOfTheWild.Value = (options.CallOfTheWild);
            numSharkAttack.Value = (options.SharkAttack);
            numWildHunt.Value = (options.WildHunt);
            checkBoxUseCustomShotRotation.Checked = options.UseCustomShotRotation;
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

        private void comboShotPrio1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio1.SelectedItem != null)
            {
                options.ShotPriority1 = (Shots)comboShotPrio1.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio2.SelectedItem != null)
            {
                options.ShotPriority2 = (Shots)comboShotPrio2.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio3.SelectedItem != null)
            {
                options.ShotPriority3 = (Shots)comboShotPrio3.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio4_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (!loadingOptions && comboShotPrio4.SelectedItem != null)
            {
                options.ShotPriority4 = (Shots)comboShotPrio4.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
            */
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

        private void checkBoxUseCustomShotRotation_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.UseCustomShotRotation = checkBoxUseCustomShotRotation.Checked;
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
    }
}
