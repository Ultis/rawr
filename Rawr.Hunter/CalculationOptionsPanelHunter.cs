using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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

            numericUpDownLatency.Value = (decimal)(options.Latency * 1000.0);

            trackBarTargetArmor.Value = options.TargetArmor;
            lblTargetArmorValue.Text = options.TargetArmor.ToString();

            comboPetFamily.Items.Clear();
            foreach (PetFamily f in Enum.GetValues(typeof(PetFamily))) comboPetFamily.Items.Add(f);
            comboPetFamily.SelectedItem = options.PetFamily;

            duration.Value = options.duration;
            numericTime20.Value = options.timeSpentSub20;
            numericTime35.Value = options.timeSpent35To20;
            numericBossHP.Value = (decimal)Math.Round(100 * options.bossHPPercentage);

            numericTime20.Maximum = duration.Value;
            numericTime35.Maximum = duration.Value;

            if (options.useManaPotion == ManaPotionType.None) cmbManaPotion.SelectedIndex = 0;
            if (options.useManaPotion == ManaPotionType.RunicManaPotion) cmbManaPotion.SelectedIndex = 1;
            if (options.useManaPotion == ManaPotionType.SuperManaPotion) cmbManaPotion.SelectedIndex = 2;

            if (options.selectedAspect == Aspect.None) cmbAspect.SelectedIndex = 0;
            if (options.selectedAspect == Aspect.Beast) cmbAspect.SelectedIndex = 1;
            if (options.selectedAspect == Aspect.Hawk) cmbAspect.SelectedIndex = 2;
            if (options.selectedAspect == Aspect.Viper) cmbAspect.SelectedIndex = 3;
            if (options.selectedAspect == Aspect.Monkey) cmbAspect.SelectedIndex = 4;
            if (options.selectedAspect == Aspect.Dragonhawk) cmbAspect.SelectedIndex = 5;

            if (options.aspectUsage == AspectUsage.AlwaysOn) cmbAspectUsage.SelectedIndex = 0;
            if (options.aspectUsage == AspectUsage.ViperToOOM) cmbAspectUsage.SelectedIndex = 1;
            if (options.aspectUsage == AspectUsage.ViperRegen) cmbAspectUsage.SelectedIndex = 2;

            chkUseBeastDuringBW.Checked = options.useBeastDuringBeastialWrath;
            chkEmulateBugs.Checked = options.emulateSpreadsheetBugs;
            chkSpreadsheetUptimes.Checked = options.calculateUptimesLikeSpreadsheet;


            PopulateAbilities();

            // Cunning
            initTalentValues(cmbCunningCorbaReflexes, 2);
            initTalentValues(cmbCunningDiveDash, 1);
            initTalentValues(cmbCunningGreatStamina, 3);
            initTalentValues(cmbCunningNaturalArmor, 2);
            initTalentValues(cmbCunningBoarsSpeed, 1);
            initTalentValues(cmbCunningMobility, 2);
            initTalentValues(cmbCunningSpikedCollar, 3);
            initTalentValues(cmbCunningAvoidance, 3);
            initTalentValues(cmbCunningLionhearted, 2);
            initTalentValues(cmbCunningCarrionFeeder, 1);
            initTalentValues(cmbCunningGreatResistance, 3);
            initTalentValues(cmbCunningOwlsFocus, 2);
            initTalentValues(cmbCunningCornered, 2);
            initTalentValues(cmbCunningFeedingFrenzy, 2);
            initTalentValues(cmbCunningWolverineBite, 1);
            initTalentValues(cmbCunningRoarOfRecovery, 1);
            initTalentValues(cmbCunningBullheaded, 1);
            initTalentValues(cmbCunningGraceOfTheMantis, 2);
            initTalentValues(cmbCunningWildHunt, 2);
            initTalentValues(cmbCunningRoarOfSacrifice, 1);
            // Ferocity
            initTalentValues(cmbFerocityAvoidance, 3);
            initTalentValues(cmbFerocityBloodthirsty, 2);
            initTalentValues(cmbFerocityBoarsSpeed, 1);
            initTalentValues(cmbFerocityCallOfTheWild, 1);
            initTalentValues(cmbFerocityCharge, 1);
            initTalentValues(cmbFerocityCobraReflexes, 2);
            initTalentValues(cmbFerocityDiveDash, 1);
            initTalentValues(cmbFerocityGreatResistance, 3);
            initTalentValues(cmbFerocityGreatStamina, 3);
            initTalentValues(cmbFerocityHeartOfThePhoenix, 1);
            initTalentValues(cmbFerocityImprovedCower, 2);
            initTalentValues(cmbFerocityLickYourWounds, 1);
            initTalentValues(cmbFerocityLionhearted, 2);
            initTalentValues(cmbFerocityNaturalArmor, 2);
            initTalentValues(cmbFerocityRabid, 1);
            initTalentValues(cmbFerocitySharkAttack, 2);
            initTalentValues(cmbFerocitySpidersBite, 3);
            initTalentValues(cmbFerocitySpikedCollar, 3);
            initTalentValues(cmbFerocityWildHunt, 2);
            // Tenacity
            initTalentValues(cmbTenacityAvoidance, 3);
            initTalentValues(cmbTenacityBloodOfTheRhino, 2);
            initTalentValues(cmbTenacityBoarsSpeed, 1);
            initTalentValues(cmbTenacityCharge, 1);
            initTalentValues(cmbTenacityCobraReflexes, 2);
            initTalentValues(cmbTenacityGraceOfTheMantis, 2);
            initTalentValues(cmbTenacityGreatResistance, 3);
            initTalentValues(cmbTenacityGreatStamina, 3);
            initTalentValues(cmbTenacityGuardDog, 2);
            initTalentValues(cmbTenacityIntervene, 1);
            initTalentValues(cmbTenacityLastStand, 1);
            initTalentValues(cmbTenacityLiohearted, 2);
            initTalentValues(cmbTenacityNaturalArmor, 2);
            initTalentValues(cmbTenacityPetBarding, 2);
            initTalentValues(cmbTenacityRoarOfSacrifice, 1);
            initTalentValues(cmbTenacitySilverback, 2);
            initTalentValues(cmbTenacitySpikedCollar, 3);
            initTalentValues(cmbTenacityTaunt, 1);
            initTalentValues(cmbTenacityThunderstomp, 1);
            initTalentValues(cmbTenacityWildHunt, 2);

            populatePetTalentCombos();

            // set up shot priorities
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

            // TODO: if this ever works, we need to call this *after* the model has been calculated
            ShotPrioritiesStatusUpdate(); 

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
            // the abilities should be in this order:
            //
            // 1) focus dump (bite / claw / smack)
            // 2) movement (dash / dive / charge)
            // 3) family skill (the one selected by default)
            // 4) second family skill (not selected by default)
            //   
            // only Cat and SpiritBeast currently have a #4!
            //

            switch (options.PetFamily)
            {
                case PetFamily.Bat:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.SonicBlast };
                    break;
                case PetFamily.Bear:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Charge, PetAttacks.Swipe };
                    break;
                case PetFamily.BirdOfPrey:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dive, PetAttacks.Snatch };
                    break;
                case PetFamily.Boar:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Charge, PetAttacks.Gore };
                    break;
                case PetFamily.CarrionBird:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.DemoralizingScreech };
                    break;
                case PetFamily.Cat:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.Rake, PetAttacks.Prowl };
                    break;
                case PetFamily.Chimera:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.FroststormBreath };
                    break;
                case PetFamily.CoreHound:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.LavaBreath };
                    break;
                case PetFamily.Crab:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Charge, PetAttacks.Pin };
                    break;
                case PetFamily.Crocolisk:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Charge, PetAttacks.BadAttitude };
                    break;
                case PetFamily.Devilsaur:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.MonstrousBite };
                    break;
                case PetFamily.Dragonhawk:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.FireBreath };
                    break;
                case PetFamily.Gorilla:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Charge, PetAttacks.Pummel };
                    break;
                case PetFamily.Hyena:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.TendonRip };
                    break;
                case PetFamily.Moth:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.SerenityDust };
                    break;
                case PetFamily.NetherRay:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.NetherShock };
                    break;
                case PetFamily.Raptor:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.SavageRend };
                    break;
                case PetFamily.Ravager:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Ravage };
                    break;
                case PetFamily.Rhino:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Charge, PetAttacks.Stampede };
                    break;
                case PetFamily.Scorpid:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Charge, PetAttacks.ScorpidPoison };
                    break;
                case PetFamily.Serpent:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.PoisonSpit };
                    break;
                case PetFamily.Silithid:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.VenomWebSpray };
                    break;
                case PetFamily.Spider:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Web };
                    break;
                case PetFamily.SpiritBeast:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.SpiritStrike, PetAttacks.Prowl };
                    break;
                case PetFamily.SporeBat:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.SporeCloud };
                    break;
                case PetFamily.Tallstrider:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.DustCloud };
                    break;
                case PetFamily.Turtle:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Charge, PetAttacks.ShellShield };
                    break;
                case PetFamily.WarpStalker:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Charge, PetAttacks.Warp };
                    break;
                case PetFamily.Wasp:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.Sting };
                    break;
                case PetFamily.WindSerpent:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.LightningBreath };
                    break;
                case PetFamily.Wolf:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.FuriousHowl };
                    break;
                case PetFamily.Worm:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Charge, PetAttacks.AcidSpit };
                    break;
            }

            comboBoxPet1.Items.Clear();
            comboBoxPet2.Items.Clear();
            comboBoxPet3.Items.Clear();
            comboBoxPet4.Items.Clear();

            comboBoxPet1.Items.Add(PetAttacks.None);
            comboBoxPet2.Items.Add(PetAttacks.None);
            comboBoxPet3.Items.Add(PetAttacks.None);
            comboBoxPet4.Items.Add(PetAttacks.None);

            comboBoxPet1.Items.Add(PetAttacks.Growl);
            comboBoxPet2.Items.Add(PetAttacks.Growl);
            comboBoxPet3.Items.Add(PetAttacks.Growl);
            comboBoxPet4.Items.Add(PetAttacks.Growl);

            comboBoxPet1.Items.Add(PetAttacks.Cower);
            comboBoxPet2.Items.Add(PetAttacks.Cower);
            comboBoxPet3.Items.Add(PetAttacks.Cower);
            comboBoxPet4.Items.Add(PetAttacks.Cower);

            foreach (PetAttacks A in familyList)
            {
                comboBoxPet1.Items.Add(A);
                comboBoxPet2.Items.Add(A);
                comboBoxPet3.Items.Add(A);
                comboBoxPet4.Items.Add(A);
            }
            comboBoxPet1.SelectedIndex = 5; // family skill 1
            comboBoxPet2.SelectedIndex = 3; // focus dump
            comboBoxPet3.SelectedIndex = 0; // none
            comboBoxPet4.SelectedIndex = 0; // none
        }

        private void comboPetFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingOptions) updateTalentDisplay();

            if (!loadingOptions && comboPetFamily.SelectedItem != null)
            {
                options.PetFamily = (PetFamily)comboPetFamily.SelectedItem;
                PopulateAbilities();
                updateTalentDisplay();
                resetTalents();
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
                numericTime20.Maximum = duration.Value; // don't allow these two to be 
                numericTime35.Maximum = duration.Value; // longer than than the fight!
                Character.OnCalculationsInvalidated();
            }
        }
        
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
            	//options.CobraReflexes = (int)numCobraReflexes.Value;
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
            ShotPrioritiesStatusUpdate();
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
            ShotPrioritiesStatusUpdate();
        }

        private void ShotPrioritiesStatusUpdate()
        {
            //TODO: get the status of this shot in the rotation
            lblPriStatus1.Text = "";
            lblPriStatus2.Text = "";
            lblPriStatus3.Text = "";
            lblPriStatus4.Text = "";
            lblPriStatus5.Text = "";
            lblPriStatus6.Text = "";
            lblPriStatus7.Text = "";
            lblPriStatus8.Text = "";
            lblPriStatus9.Text = "";
            lblPriStatus10.Text = "";
        }

        private void initTalentValues(ComboBox cmbBox, int max)
        {
            cmbBox.Items.Clear();
            for (int i = 0; i <= max; i++)
            {
                cmbBox.Items.Add(i);
            }
            cmbBox.SelectedIndex = 0;
        }

        private PetFamilyTree getPetFamilyTree()
        {
            switch ((PetFamily)comboPetFamily.SelectedItem)
            {
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
                    return PetFamilyTree.Cunning;

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
                    return PetFamilyTree.Tenacity;

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
                    return PetFamilyTree.Ferocity;
            }

            // hmmm!
            return PetFamilyTree.Unknown;
        }

        private void updateTalentDisplay()
        {
            PetFamilyTree tree = getPetFamilyTree();
            grpTalentsCunning.Visible = tree == PetFamilyTree.Cunning;
            grpTalentsFerocity.Visible = tree == PetFamilyTree.Ferocity;
            grpTalentsTenacity.Visible = tree == PetFamilyTree.Tenacity;
        }

        private void resetTalents()
        {
            options.petCobraReflexes = 0;
            options.petDiveDash = 0;
            options.petCharge = 0;
            options.petGreatStamina = 0;
            options.petNaturalArmor = 0;
            options.petBoarsSpeed = 0;
            options.petMobility = 0;
            options.petSpikedCollar = 0;
            options.petImprovedCower = 0;
            options.petBloodthirsty = 0;
            options.petBloodOfTheRhino = 0;
            options.petPetBarding = 0;
            options.petAvoidance = 0;
            options.petLionhearted = 0;
            options.petCarrionFeeder = 0;
            options.petGuardDog = 0;
            options.petThunderstomp = 0;
            options.petGreatResistance = 0;
            options.petOwlsFocus = 0;
            options.petCornered = 0;
            options.petFeedingFrenzy = 0;
            options.petHeartOfThePhoenix = 0;
            options.petSpidersBite = 0;
            options.petWolverineBite = 0;
            options.petRoarOfRecovery = 0;
            options.petBullheaded = 0;
            options.petGraceOfTheMantis = 0;
            options.petRabid = 0;
            options.petLickYourWounds = 0;
            options.petCallOfTheWild = 0;
            options.petLastStand = 0;
            options.petTaunt = 0;
            options.petIntervene = 0;
            options.petWildHunt = 0;
            options.petRoarOfSacrifice = 0;
            options.petSharkAttack = 0;
            options.petSilverback = 0;

            populatePetTalentCombos();
        }

        private void talentComboChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            // one of the (many) talent combo boxes has been updated
            // so we need to update the options

            PetFamilyTree tree = getPetFamilyTree();

            if (tree == PetFamilyTree.Cunning)
            {
                options.petCobraReflexes = cmbCunningCorbaReflexes.SelectedIndex;
                options.petDiveDash = cmbCunningDiveDash.SelectedIndex;
                options.petCharge = 0;
                options.petGreatStamina = cmbCunningGreatStamina.SelectedIndex;
                options.petNaturalArmor = cmbCunningNaturalArmor.SelectedIndex;
                options.petBoarsSpeed = cmbCunningBoarsSpeed.SelectedIndex;
                options.petMobility = cmbCunningMobility.SelectedIndex;
                options.petSpikedCollar = cmbCunningSpikedCollar.SelectedIndex;
                options.petImprovedCower = 0;
                options.petBloodthirsty = 0;
                options.petBloodOfTheRhino = 0;
                options.petPetBarding = 0;
                options.petAvoidance = cmbCunningAvoidance.SelectedIndex;
                options.petLionhearted = cmbCunningLionhearted.SelectedIndex;
                options.petCarrionFeeder = cmbCunningCarrionFeeder.SelectedIndex;
                options.petGuardDog = 0;
                options.petThunderstomp = 0;
                options.petGreatResistance = cmbCunningGreatResistance.SelectedIndex;
                options.petOwlsFocus = cmbCunningOwlsFocus.SelectedIndex;
                options.petCornered = cmbCunningCornered.SelectedIndex;
                options.petFeedingFrenzy = cmbCunningFeedingFrenzy.SelectedIndex;
                options.petHeartOfThePhoenix = 0;
                options.petSpidersBite = 0;
                options.petWolverineBite = cmbCunningWolverineBite.SelectedIndex;
                options.petRoarOfRecovery = cmbCunningRoarOfRecovery.SelectedIndex;
                options.petBullheaded = cmbCunningBullheaded.SelectedIndex;
                options.petGraceOfTheMantis = cmbCunningGraceOfTheMantis.SelectedIndex;
                options.petRabid = 0;
                options.petLickYourWounds = 0;
                options.petCallOfTheWild = 0;
                options.petLastStand = 0;
                options.petTaunt = 0;
                options.petIntervene = 0;
                options.petWildHunt = cmbCunningWildHunt.SelectedIndex;
                options.petRoarOfSacrifice = cmbCunningRoarOfSacrifice.SelectedIndex;
                options.petSharkAttack = 0;
                options.petSilverback = 0;
            }

            if (tree == PetFamilyTree.Ferocity)
            {
                options.petCobraReflexes = cmbFerocityCobraReflexes.SelectedIndex;
                options.petDiveDash = cmbFerocityDiveDash.SelectedIndex;
                options.petCharge = cmbFerocityCharge.SelectedIndex;
                options.petGreatStamina = cmbFerocityGreatStamina.SelectedIndex;
                options.petNaturalArmor = cmbFerocityNaturalArmor.SelectedIndex;
                options.petBoarsSpeed = cmbFerocityBoarsSpeed.SelectedIndex;
                options.petMobility = 0;
                options.petSpikedCollar = cmbFerocitySpikedCollar.SelectedIndex;
                options.petImprovedCower = cmbFerocityImprovedCower.SelectedIndex;
                options.petBloodthirsty = cmbFerocityBloodthirsty.SelectedIndex;
                options.petBloodOfTheRhino = 0;
                options.petPetBarding = 0;
                options.petAvoidance = cmbFerocityAvoidance.SelectedIndex;
                options.petLionhearted = cmbFerocityLionhearted.SelectedIndex;
                options.petCarrionFeeder = 0;
                options.petGuardDog = 0;
                options.petThunderstomp = 0;
                options.petGreatResistance = cmbFerocityGreatResistance.SelectedIndex;
                options.petOwlsFocus = 0;
                options.petCornered = 0;
                options.petFeedingFrenzy = 0;
                options.petHeartOfThePhoenix = cmbFerocityHeartOfThePhoenix.SelectedIndex;
                options.petSpidersBite = cmbFerocitySpidersBite.SelectedIndex;
                options.petWolverineBite = 0;
                options.petRoarOfRecovery = 0;
                options.petBullheaded = 0;
                options.petGraceOfTheMantis = 0;
                options.petRabid = cmbFerocityRabid.SelectedIndex;
                options.petLickYourWounds = cmbFerocityLickYourWounds.SelectedIndex;
                options.petCallOfTheWild = cmbFerocityCallOfTheWild.SelectedIndex;
                options.petLastStand = 0;
                options.petTaunt = 0;
                options.petIntervene = 0;
                options.petWildHunt = cmbFerocityWildHunt.SelectedIndex;
                options.petRoarOfSacrifice = 0;
                options.petSharkAttack = cmbFerocitySharkAttack.SelectedIndex;
                options.petSilverback = 0;
            }

            if (tree == PetFamilyTree.Tenacity)
            {
                options.petCobraReflexes = cmbTenacityCobraReflexes.SelectedIndex;
                options.petDiveDash = 0;
                options.petCharge = cmbTenacityCharge.SelectedIndex;
                options.petGreatStamina = cmbTenacityGreatStamina.SelectedIndex;
                options.petNaturalArmor = cmbTenacityNaturalArmor.SelectedIndex;
                options.petBoarsSpeed = cmbTenacityBoarsSpeed.SelectedIndex;
                options.petMobility = 0;
                options.petSpikedCollar = cmbTenacitySpikedCollar.SelectedIndex;
                options.petImprovedCower = 0;
                options.petBloodthirsty = 0;
                options.petBloodOfTheRhino = cmbTenacityBloodOfTheRhino.SelectedIndex;
                options.petPetBarding = cmbTenacityPetBarding.SelectedIndex;
                options.petAvoidance = cmbTenacityAvoidance.SelectedIndex;
                options.petLionhearted = cmbTenacityLiohearted.SelectedIndex;
                options.petCarrionFeeder = 0;
                options.petGuardDog = cmbTenacityGuardDog.SelectedIndex;
                options.petThunderstomp = cmbTenacityThunderstomp.SelectedIndex;
                options.petGreatResistance = cmbTenacityGreatResistance.SelectedIndex;
                options.petOwlsFocus = 0;
                options.petCornered = 0;
                options.petFeedingFrenzy = 0;
                options.petHeartOfThePhoenix = 0;
                options.petSpidersBite = 0;
                options.petWolverineBite = 0;
                options.petRoarOfRecovery = 0;
                options.petBullheaded = 0;
                options.petGraceOfTheMantis = cmbTenacityGraceOfTheMantis.SelectedIndex;
                options.petRabid = 0;
                options.petLickYourWounds = 0;
                options.petCallOfTheWild = 0;
                options.petLastStand = cmbTenacityLastStand.SelectedIndex;
                options.petTaunt = cmbTenacityTaunt.SelectedIndex;
                options.petIntervene = cmbTenacityIntervene.SelectedIndex;
                options.petWildHunt = cmbTenacityWildHunt.SelectedIndex;
                options.petRoarOfSacrifice = cmbTenacityRoarOfSacrifice.SelectedIndex;
                options.petSharkAttack = 0;
                options.petSilverback = cmbTenacitySilverback.SelectedIndex;
            }

            Character.OnCalculationsInvalidated();
        }

        private void populatePetTalentCombos()
        {
            // called when options are loaded to allow us to set the selected indexes
            
            PetFamilyTree tree = getPetFamilyTree();
            
            cmbCunningAvoidance.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petAvoidance : 0;
            cmbCunningBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petBoarsSpeed : 0;
            cmbCunningBullheaded.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petBullheaded : 0;
            cmbCunningCarrionFeeder.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petCarrionFeeder : 0;
            cmbCunningCorbaReflexes.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petCobraReflexes : 0;
            cmbCunningCornered.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petCornered : 0;
            cmbCunningDiveDash.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petDiveDash : 0;
            cmbCunningFeedingFrenzy.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petFeedingFrenzy : 0;
            cmbCunningGraceOfTheMantis.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petGraceOfTheMantis : 0;
            cmbCunningGreatResistance.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petGreatResistance : 0;
            cmbCunningGreatStamina.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petGreatStamina : 0;
            cmbCunningLionhearted.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petLionhearted : 0;
            cmbCunningMobility.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petMobility : 0;
            cmbCunningNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petNaturalArmor : 0;
            cmbCunningOwlsFocus.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petOwlsFocus : 0;
            cmbCunningRoarOfRecovery.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petRoarOfRecovery : 0;
            cmbCunningRoarOfSacrifice.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petRoarOfSacrifice : 0;
            cmbCunningSpikedCollar.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petSpikedCollar : 0;
            cmbCunningWildHunt.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petWildHunt : 0;
            cmbCunningWolverineBite.SelectedIndex = (tree == PetFamilyTree.Cunning) ? options.petWolverineBite : 0;

            cmbFerocityAvoidance.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petAvoidance : 0;
            cmbFerocityBloodthirsty.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petBloodthirsty : 0;
            cmbFerocityBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petBoarsSpeed : 0;
            cmbFerocityCallOfTheWild.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petCallOfTheWild : 0;
            cmbFerocityCharge.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petCharge : 0;
            cmbFerocityCobraReflexes.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petCobraReflexes : 0;
            cmbFerocityDiveDash.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petDiveDash : 0;
            cmbFerocityGreatResistance.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petGreatResistance : 0;
            cmbFerocityGreatStamina.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petGreatStamina : 0;
            cmbFerocityHeartOfThePhoenix.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petHeartOfThePhoenix : 0;
            cmbFerocityImprovedCower.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petImprovedCower : 0;
            cmbFerocityLickYourWounds.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petLickYourWounds : 0;
            cmbFerocityLionhearted.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petLionhearted : 0;
            cmbFerocityNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petNaturalArmor : 0;
            cmbFerocityRabid.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petRabid : 0;
            cmbFerocitySharkAttack.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petSharkAttack : 0;
            cmbFerocitySpidersBite.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petSpidersBite : 0;
            cmbFerocitySpikedCollar.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petSpikedCollar : 0;
            cmbFerocityWildHunt.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? options.petWildHunt : 0;

            cmbTenacityAvoidance.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petAvoidance : 0;
            cmbTenacityBloodOfTheRhino.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petBloodOfTheRhino : 0;
            cmbTenacityBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petBoarsSpeed : 0;
            cmbTenacityCharge.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petCharge : 0;
            cmbTenacityCobraReflexes.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petCobraReflexes : 0;
            cmbTenacityGraceOfTheMantis.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petGraceOfTheMantis : 0;
            cmbTenacityGreatResistance.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petGreatResistance : 0;
            cmbTenacityGreatStamina.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petGreatStamina : 0;
            cmbTenacityGuardDog.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petGuardDog : 0;
            cmbTenacityIntervene.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petIntervene : 0;
            cmbTenacityLastStand.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petLastStand : 0;
            cmbTenacityLiohearted.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petLionhearted : 0;
            cmbTenacityNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petNaturalArmor : 0;
            cmbTenacityPetBarding.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petPetBarding : 0;
            cmbTenacityRoarOfSacrifice.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petRoarOfSacrifice : 0;
            cmbTenacitySilverback.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petSilverback : 0;
            cmbTenacitySpikedCollar.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petSpikedCollar : 0;
            cmbTenacityTaunt.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petTaunt : 0;
            cmbTenacityThunderstomp.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petThunderstomp : 0;
            cmbTenacityWildHunt.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? options.petWildHunt : 0;
        }

        private void numericTime20_ValueChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.timeSpentSub20 = (int)numericTime20.Value;
            Character.OnCalculationsInvalidated();
        }

        private void numericTime35_ValueChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.timeSpent35To20 = (int)numericTime35.Value;
            Character.OnCalculationsInvalidated();
        }

        private void numericBossHP_ValueChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.bossHPPercentage = (float)(numericBossHP.Value / 100);
            Character.OnCalculationsInvalidated();
        }

        private void cmbManaPotion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            if (cmbManaPotion.SelectedIndex == 0) options.useManaPotion = ManaPotionType.None;
            if (cmbManaPotion.SelectedIndex == 1) options.useManaPotion = ManaPotionType.RunicManaPotion;
            if (cmbManaPotion.SelectedIndex == 2) options.useManaPotion = ManaPotionType.SuperManaPotion;
            Character.OnCalculationsInvalidated();
        }

        private void cmbAspect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            if (cmbAspect.SelectedIndex == 0) options.selectedAspect = Aspect.None;
            if (cmbAspect.SelectedIndex == 1) options.selectedAspect = Aspect.Beast;
            if (cmbAspect.SelectedIndex == 2) options.selectedAspect = Aspect.Hawk;
            if (cmbAspect.SelectedIndex == 3) options.selectedAspect = Aspect.Viper;
            if (cmbAspect.SelectedIndex == 4) options.selectedAspect = Aspect.Monkey;
            if (cmbAspect.SelectedIndex == 5) options.selectedAspect = Aspect.Dragonhawk;
            Character.OnCalculationsInvalidated();
        }

        private void cmbAspectUsage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            if (cmbAspectUsage.SelectedIndex == 0) options.aspectUsage = AspectUsage.AlwaysOn;
            if (cmbAspectUsage.SelectedIndex == 1) options.aspectUsage = AspectUsage.ViperToOOM;
            if (cmbAspectUsage.SelectedIndex == 2) options.aspectUsage = AspectUsage.ViperRegen;
            Character.OnCalculationsInvalidated();
        }

        private void chkUseBeastDuringBW_CheckedChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.useBeastDuringBeastialWrath = chkUseBeastDuringBW.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chkEmulateBugs_CheckedChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.emulateSpreadsheetBugs = chkEmulateBugs.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chkSpreadsheetUptimes_CheckedChanged(object sender, EventArgs e)
        {
            if (loadingOptions) return;
            options.calculateUptimesLikeSpreadsheet = chkSpreadsheetUptimes.Checked;
            Character.OnCalculationsInvalidated();
        }

    }
}