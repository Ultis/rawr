using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Rawr.HunterSE
{
    public partial class CalculationOptionsPanelHunterSE : CalculationOptionsPanelBase
    {
        #region Instance Variables
        private bool isLoading = false;
		private CalculationOptionsHunterSE CalcOpts = null;
        private PetBuffSelectorSE petBuffSelector = null;
        #endregion

        #region Constructors
        public CalculationOptionsPanelHunterSE()
        {
            isLoading = true;
            InitializeComponent();

            CB_Duration.Minimum = 0;
            CB_Duration.Maximum = 60 * 20; // 20 minutes

            // The PetBuffSelector doesn't work in the designer. bah
            petBuffSelector = new PetBuffSelectorSE();
            petBuffSelector.character = Character;

            Page_04_PetBuffs.Controls.Add(petBuffSelector);
            petBuffSelector.AutoScroll = true;
            petBuffSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            petBuffSelector.Location = new System.Drawing.Point(0, 0);
            petBuffSelector.Name = "petBuffSelector";
            petBuffSelector.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            petBuffSelector.Size = new System.Drawing.Size(303, 558);
            petBuffSelector.TabIndex = 0;

            InitializeShotList(CB_ShotPriority_01);
            InitializeShotList(CB_ShotPriority_02);
            InitializeShotList(CB_ShotPriority_03);
            InitializeShotList(CB_ShotPriority_04);
            InitializeShotList(CB_ShotPriority_05);
            InitializeShotList(CB_ShotPriority_06);
            InitializeShotList(CB_ShotPriority_07);
            InitializeShotList(CB_ShotPriority_08);
            InitializeShotList(CB_ShotPriority_09);
            InitializeShotList(CB_ShotPriority_10);
            isLoading = false;
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
        protected override void LoadCalculationOptions()
        {
            isLoading = true;
            if (Character != null && Character.CalculationOptions == null)
            {
                // If it's broke, make a new one with the defaults
                Character.CalculationOptions = new CalculationOptionsHunterSE();
                isLoading = true;
            }
            CalcOpts = Character.CalculationOptions as CalculationOptionsHunterSE;
            for (int i = 0; i < CB_TargetLevel.Items.Count; i++)
            {
                if (CB_TargetLevel.Items[i] as string == CalcOpts.TargetLevel.ToString())
                {
                    CB_TargetLevel.SelectedItem = CB_TargetLevel.Items[i];
                    break;
                }
            }

            // Hiding Gear based on Bad Stats
            CK_HideSplGear.Checked = CalcOpts.HideBadItems_Spl; CalculationsHunterSE.HidingBadStuff_Spl = CalcOpts.HideBadItems_Spl;
            CK_HidePvPGear.Checked = CalcOpts.HideBadItems_PvP; CalculationsHunterSE.HidingBadStuff_PvP = CalcOpts.HideBadItems_PvP;
            // Hiding Enchants based on Profession
            CK_HideProfEnchants.Checked = CalcOpts.HideProfEnchants;
            CalculationsHunterSE.HidingBadStuff_Prof = CalcOpts.HideProfEnchants;
            CB_Prof1.Enabled = CK_HideProfEnchants.Checked;
            CB_Prof2.Enabled = CK_HideProfEnchants.Checked;
            CB_Prof1.Text = ProfessionToString(Character.PrimaryProfession);
            CB_Prof2.Text = ProfessionToString(Character.SecondaryProfession);

            petBuffSelector.character = Character;
            petBuffSelector.LoadBuffsFromOptions();

            NUD_Latency.Value = (decimal)(CalcOpts.Latency * 1000.0);

            Bar_TargArmor.Value = CalcOpts.TargetArmor;
            LB_TargArmorValue.Text = CalcOpts.TargetArmor.ToString();

            CB_PetFamily.Items.Clear();
            foreach (PetFamily f in Enum.GetValues(typeof(PetFamily))) CB_PetFamily.Items.Add(f);
            CB_PetFamily.SelectedItem = CalcOpts.PetFamily;

            CB_Duration.Value = CalcOpts.duration;
            NUD_Time20.Value = CalcOpts.timeSpentSub20;
            NUD_35.Value = CalcOpts.timeSpent35To20;
            NUD_BossHP.Value = (decimal)Math.Round(100 * CalcOpts.bossHPPercentage);

            // Drizz: Added option for CDCutoff
            NUD_CDCutOff.Value = CalcOpts.cooldownCutoff;

            NUD_Time20.Maximum = CB_Duration.Value;
            NUD_35.Maximum = CB_Duration.Value;

            if (CalcOpts.useManaPotion == ManaPotionType.None) CB_ManaPotion.SelectedIndex = 0;
            if (CalcOpts.useManaPotion == ManaPotionType.RunicManaPotion) CB_ManaPotion.SelectedIndex = 1;
            if (CalcOpts.useManaPotion == ManaPotionType.SuperManaPotion) CB_ManaPotion.SelectedIndex = 2;

            if (CalcOpts.selectedAspect == Aspect.None) CB_Aspect.SelectedIndex = 0;
            if (CalcOpts.selectedAspect == Aspect.Beast) CB_Aspect.SelectedIndex = 1;
            if (CalcOpts.selectedAspect == Aspect.Hawk) CB_Aspect.SelectedIndex = 2;
            if (CalcOpts.selectedAspect == Aspect.Viper) CB_Aspect.SelectedIndex = 3;
            if (CalcOpts.selectedAspect == Aspect.Monkey) CB_Aspect.SelectedIndex = 4;
            if (CalcOpts.selectedAspect == Aspect.Dragonhawk) CB_Aspect.SelectedIndex = 5;

            if (CalcOpts.aspectUsage == AspectUsage.AlwaysOn) CB_AspectUsage.SelectedIndex = 0;
            if (CalcOpts.aspectUsage == AspectUsage.ViperToOOM) CB_AspectUsage.SelectedIndex = 1;
            if (CalcOpts.aspectUsage == AspectUsage.ViperRegen) CB_AspectUsage.SelectedIndex = 2;

            CK_UseBeastDuringBW.Checked = CalcOpts.useBeastDuringBeastialWrath;
            CK_EmulateBugs.Checked = CalcOpts.emulateSpreadsheetBugs;
            CK_SpreadsheetUptimes.Checked = CalcOpts.calculateUptimesLikeSpreadsheet;
            CK_RandomProcs.Checked = CalcOpts.randomizeProcs;
            CK_UseRotation.Checked = CalcOpts.useRotationTest;

            PopulateAbilities();

            CB_PetPrio_01.SelectedItem = CalcOpts.PetPriority1;
            CB_PetPrio_02.SelectedItem = CalcOpts.PetPriority2;
            CB_PetPrio_03.SelectedItem = CalcOpts.PetPriority3;
            CB_PetPrio_04.SelectedItem = CalcOpts.PetPriority4;
            CB_PetPrio_05.SelectedItem = CalcOpts.PetPriority5;
            CB_PetPrio_06.SelectedItem = CalcOpts.PetPriority6;
            CB_PetPrio_07.SelectedItem = CalcOpts.PetPriority7;


            // Cunning
            initTalentValues(CB_CunningCorbaReflexes, 2);
            initTalentValues(CB_CunningDiveDash, 1);
            initTalentValues(CB_CunningGreatStamina, 3);
            initTalentValues(CB_CunningNaturalArmor, 2);
            initTalentValues(CB_CunningBoarsSpeed, 1);
            initTalentValues(CB_CunningMobility, 2);
            initTalentValues(CB_CunningSpikedCollar, 3);
            initTalentValues(CB_CunningAvoidance, 3);
            initTalentValues(CB_CunningLionhearted, 2);
            initTalentValues(CB_CunningCarrionFeeder, 1);
            initTalentValues(CB_CunningGreatResistance, 3);
            initTalentValues(CB_CunningOwlsFocus, 2);
            initTalentValues(CB_CunningCornered, 2);
            initTalentValues(CB_CunningFeedingFrenzy, 2);
            initTalentValues(CB_CunningWolverineBite, 1);
            initTalentValues(CB_CunningRoarOfRecovery, 1);
            initTalentValues(CB_CunningBullheaded, 1);
            initTalentValues(CB_CunningGraceOfTheMantis, 2);
            initTalentValues(CB_CunningWildHunt, 2);
            initTalentValues(CB_CunningRoarOfSacrifice, 1);
            // Ferocity
            initTalentValues(CB_FerocityAvoidance, 3);
            initTalentValues(CB_FerocityBloodthirsty, 2);
            initTalentValues(CB_FerocityBoarsSpeed, 1);
            initTalentValues(CB_FerocityCallOfTheWild, 1);
            initTalentValues(CB_FerocityChargeSwoop, 1);
            initTalentValues(CB_FerocityCobraReflexes, 2);
            initTalentValues(CB_FerocityDiveDash, 1);
            initTalentValues(CB_FerocityGreatResistance, 3);
            initTalentValues(CB_FerocityGreatStamina, 3);
            initTalentValues(CB_FerocityHeartOfThePhoenix, 1);
            initTalentValues(CB_FerocityImprovedCower, 2);
            initTalentValues(CB_FerocityLickYourWounds, 1);
            initTalentValues(CB_FerocityLionhearted, 2);
            initTalentValues(CB_FerocityNaturalArmor, 2);
            initTalentValues(CB_FerocityRabid, 1);
            initTalentValues(CB_FerocitySharkAttack, 2);
            initTalentValues(CB_FerocitySpidersBite, 3);
            initTalentValues(CB_FerocitySpikedCollar, 3);
            initTalentValues(CB_FerocityWildHunt, 2);
            // Tenacity
            initTalentValues(CB_TenacityAvoidance, 3);
            initTalentValues(CB_TenacityBloodOfTheRhino, 2);
            initTalentValues(CB_TenacityBoarsSpeed, 1);
            initTalentValues(CB_TenacityCharge, 1);
            initTalentValues(CB_TenacityCobraReflexes, 2);
            initTalentValues(CB_TenacityGraceOfTheMantis, 2);
            initTalentValues(CB_TenacityGreatResistance, 3);
            initTalentValues(CB_TenacityGreatStamina, 3);
            initTalentValues(CB_TenacityGuardDog, 2);
            initTalentValues(CB_TenacityIntervene, 1);
            initTalentValues(CB_TenacityLastStand, 1);
            initTalentValues(CB_TenacityLiohearted, 2);
            initTalentValues(CB_TenacityNaturalArmor, 2);
            initTalentValues(CB_TenacityPetBarding, 2);
            initTalentValues(CB_TenacityRoarOfSacrifice, 1);
            initTalentValues(CB_TenacitySilverback, 2);
            initTalentValues(CB_TenacitySpikedCollar, 3);
            initTalentValues(CB_TenacityTaunt, 1);
            initTalentValues(CB_TenacityThunderstomp, 1);
            initTalentValues(CB_TenacityWildHunt, 2);

            populatePetTalentCombos();

            // set up shot priorities
            CB_ShotPriority_01.SelectedIndex = CalcOpts.PriorityIndex1;
            CB_ShotPriority_02.SelectedIndex = CalcOpts.PriorityIndex2;
            CB_ShotPriority_03.SelectedIndex = CalcOpts.PriorityIndex3;
            CB_ShotPriority_04.SelectedIndex = CalcOpts.PriorityIndex4;
            CB_ShotPriority_05.SelectedIndex = CalcOpts.PriorityIndex5;
            CB_ShotPriority_06.SelectedIndex = CalcOpts.PriorityIndex6;
            CB_ShotPriority_07.SelectedIndex = CalcOpts.PriorityIndex7;
            CB_ShotPriority_08.SelectedIndex = CalcOpts.PriorityIndex8;
            CB_ShotPriority_09.SelectedIndex = CalcOpts.PriorityIndex9;
            CB_ShotPriority_10.SelectedIndex = CalcOpts.PriorityIndex10;
            CB_PriorityDefaults.SelectedIndex = 0;

            isLoading = false;
        }
        #endregion

        #region Event Handlers

        private PetAttacks[] familyList = null;

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalcOpts.TargetLevel = int.Parse(CB_TargetLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }

        private void PopulateAbilities()
        {
            // the abilities should be in this order:
            //
            // 1) focus dump (bite / claw / smack)
            // 2) dash / dive / none
            // 3) charge / swoop / none
            // 4) family skill (the one selected by default)
            // 5) second family skill (not selected by default)
            //   
            // only Cat and SpiritBeast currently have a #5!
            //

            switch (CalcOpts.PetFamily)
            {
                case PetFamily.Bat:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.None, PetAttacks.SonicBlast };
                    break;
                case PetFamily.Bear:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.None, PetAttacks.Charge, PetAttacks.Swipe };
                    break;
                case PetFamily.BirdOfPrey:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dive, PetAttacks.None, PetAttacks.Snatch };
                    break;
                case PetFamily.Boar:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.None, PetAttacks.Charge, PetAttacks.Gore };
                    break;
                case PetFamily.CarrionBird:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.Swoop, PetAttacks.DemoralizingScreech };
                    break;
                case PetFamily.Cat:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.Rake, PetAttacks.Prowl };
                    break;
                case PetFamily.Chimera:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.None, PetAttacks.FroststormBreath };
                    break;
                case PetFamily.CoreHound:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.LavaBreath };
                    break;
                case PetFamily.Crab:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.None, PetAttacks.Charge, PetAttacks.Pin };
                    break;
                case PetFamily.Crocolisk:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.None, PetAttacks.Charge, PetAttacks.BadAttitude };
                    break;
                case PetFamily.Devilsaur:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.MonstrousBite };
                    break;
                case PetFamily.Dragonhawk:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.None, PetAttacks.FireBreath };
                    break;
                case PetFamily.Gorilla:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.None, PetAttacks.Charge, PetAttacks.Pummel };
                    break;
                case PetFamily.Hyena:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.TendonRip };
                    break;
                case PetFamily.Moth:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.Swoop, PetAttacks.SerenityDust };
                    break;
                case PetFamily.NetherRay:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.None, PetAttacks.NetherShock };
                    break;
                case PetFamily.Raptor:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.SavageRend };
                    break;
                case PetFamily.Ravager:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.None, PetAttacks.Ravage };
                    break;
                case PetFamily.Rhino:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.None, PetAttacks.Charge, PetAttacks.Stampede };
                    break;
                case PetFamily.Scorpid:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.None, PetAttacks.Charge, PetAttacks.ScorpidPoison };
                    break;
                case PetFamily.Serpent:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.None, PetAttacks.PoisonSpit };
                    break;
                case PetFamily.Silithid:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.None, PetAttacks.VenomWebSpray };
                    break;
                case PetFamily.Spider:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.None, PetAttacks.Web };
                    break;
                case PetFamily.SpiritBeast:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.SpiritStrike, PetAttacks.Prowl };
                    break;
                case PetFamily.SporeBat:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.None, PetAttacks.SporeCloud };
                    break;
                case PetFamily.Tallstrider:
                    familyList = new PetAttacks[] { PetAttacks.Claw, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.DustCloud };
                    break;
                case PetFamily.Turtle:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.None, PetAttacks.Charge, PetAttacks.ShellShield };
                    break;
                case PetFamily.WarpStalker:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.Warp };
                    break;
                case PetFamily.Wasp:
                    familyList = new PetAttacks[] { PetAttacks.Smack, PetAttacks.Dive, PetAttacks.Swoop, PetAttacks.Sting };
                    break;
                case PetFamily.WindSerpent:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dive, PetAttacks.None, PetAttacks.LightningBreath };
                    break;
                case PetFamily.Wolf:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.Dash, PetAttacks.Charge, PetAttacks.FuriousHowl };
                    break;
                case PetFamily.Worm:
                    familyList = new PetAttacks[] { PetAttacks.Bite, PetAttacks.None, PetAttacks.Charge, PetAttacks.AcidSpit };
                    break;
            }

            CB_PetPrio_01.Items.Clear();
            CB_PetPrio_01.Items.Add(PetAttacks.None);

            int family_mod = 0;

            if (CalcOpts.PetFamily != PetFamily.None)
            {
                CB_PetPrio_01.Items.Add(PetAttacks.Growl);
                CB_PetPrio_01.Items.Add(PetAttacks.Cower);

                foreach (PetAttacks A in familyList)
                {
                    if (A == PetAttacks.None)
                    {
                        family_mod++;
                    }
                    else
                    {
                        CB_PetPrio_01.Items.Add(A);
                    }
                }

                PetFamilyTree family = getPetFamilyTree();

                if (family == PetFamilyTree.Cunning)
                {
                    //comboBoxPet1.Items.Add(PetAttacks.RoarOfRecovery);
                    CB_PetPrio_01.Items.Add(PetAttacks.RoarOfSacrifice);
                    CB_PetPrio_01.Items.Add(PetAttacks.WolverineBite);
                    //comboBoxPet1.Items.Add(PetAttacks.Bullheaded);
                }

                if (family == PetFamilyTree.Ferocity)
                {
                    CB_PetPrio_01.Items.Add(PetAttacks.LickYourWounds);
                    //comboBoxPet1.Items.Add(PetAttacks.CallOfTheWild);
                    //comboBoxPet1.Items.Add(PetAttacks.Rabid);
                }

                if (family == PetFamilyTree.Tenacity)
                {
                    CB_PetPrio_01.Items.Add(PetAttacks.Thunderstomp);
                    CB_PetPrio_01.Items.Add(PetAttacks.LastStand);
                    CB_PetPrio_01.Items.Add(PetAttacks.Taunt);
                    CB_PetPrio_01.Items.Add(PetAttacks.RoarOfSacrifice);
                }
            }

            object[] attacks_picklist = new object[CB_PetPrio_01.Items.Count];
            CB_PetPrio_01.Items.CopyTo(attacks_picklist, 0);

            CB_PetPrio_02.Items.Clear();
            CB_PetPrio_03.Items.Clear();
            CB_PetPrio_04.Items.Clear();
            CB_PetPrio_05.Items.Clear();
            CB_PetPrio_06.Items.Clear();
            CB_PetPrio_07.Items.Clear();

            CB_PetPrio_02.Items.AddRange(attacks_picklist);
            CB_PetPrio_03.Items.AddRange(attacks_picklist);
            CB_PetPrio_04.Items.AddRange(attacks_picklist);
            CB_PetPrio_05.Items.AddRange(attacks_picklist);
            CB_PetPrio_06.Items.AddRange(attacks_picklist);
            CB_PetPrio_07.Items.AddRange(attacks_picklist);

            if (CalcOpts.PetFamily != PetFamily.None)
            {
                CB_PetPrio_01.SelectedIndex = 6 - family_mod; // family skill 1
                CB_PetPrio_02.SelectedIndex = 3; // focus dump
            }
            else
            {
                CB_PetPrio_01.SelectedIndex = 0; // none
                CB_PetPrio_02.SelectedIndex = 0; // none
            }

            CB_PetPrio_03.SelectedIndex = 0; // none
            CB_PetPrio_04.SelectedIndex = 0; // none
            CB_PetPrio_05.SelectedIndex = 0; // none
            CB_PetPrio_06.SelectedIndex = 0; // none
            CB_PetPrio_07.SelectedIndex = 0; // none
        }

        private void comboPetFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) updateTalentDisplay();

            if (!isLoading && CB_PetFamily.SelectedItem != null)
            {
                CalcOpts.PetFamily = (PetFamily)CB_PetFamily.SelectedItem;
                PopulateAbilities();
                updateTalentDisplay();
                resetTalents();
                Character.OnCalculationsInvalidated();
            }
        }

        #endregion

        private void trackBarTargetArmor_Scroll(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalcOpts.TargetArmor = Bar_TargArmor.Value;
                Character.OnCalculationsInvalidated();
                LB_TargArmorValue.Text = Bar_TargArmor.Value.ToString();
            }
        }

        private void numericUpDownLatency_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalcOpts.Latency = (float)NUD_Latency.Value/1000.0f;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numericUpDownCDCutOff_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalcOpts.cooldownCutoff = (int)NUD_CDCutOff.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void duration_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
            	CalcOpts.duration = (int)CB_Duration.Value;
                NUD_Time20.Maximum = CB_Duration.Value; // don't allow these two to be 
                NUD_35.Maximum = CB_Duration.Value; // longer than than the fight!
                Character.OnCalculationsInvalidated();
            }
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
            	//options.CobraReflexes = (int)numCobraReflexes.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxPets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.PetPriority1 = CB_PetPrio_01.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_01.SelectedItem;
            CalcOpts.PetPriority2 = CB_PetPrio_02.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_02.SelectedItem;
            CalcOpts.PetPriority3 = CB_PetPrio_03.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_03.SelectedItem;
            CalcOpts.PetPriority4 = CB_PetPrio_04.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_04.SelectedItem;
            CalcOpts.PetPriority5 = CB_PetPrio_05.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_05.SelectedItem;
            CalcOpts.PetPriority6 = CB_PetPrio_06.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_06.SelectedItem;
            CalcOpts.PetPriority7 = CB_PetPrio_07.SelectedItem == null ? PetAttacks.None : (PetAttacks)CB_PetPrio_07.SelectedItem;
            Character.OnCalculationsInvalidated();
        }

        private void PrioritySelectedIndexChanged(object sender, EventArgs e)
        {
            // this is called whenever any of the priority dropdowns are modified
            if (isLoading) return;

            CalcOpts.PriorityIndex1 = CB_ShotPriority_01.SelectedIndex;
            CalcOpts.PriorityIndex2 = CB_ShotPriority_02.SelectedIndex;
            CalcOpts.PriorityIndex3 = CB_ShotPriority_03.SelectedIndex;
            CalcOpts.PriorityIndex4 = CB_ShotPriority_04.SelectedIndex;
            CalcOpts.PriorityIndex5 = CB_ShotPriority_05.SelectedIndex;
            CalcOpts.PriorityIndex6 = CB_ShotPriority_06.SelectedIndex;
            CalcOpts.PriorityIndex7 = CB_ShotPriority_07.SelectedIndex;
            CalcOpts.PriorityIndex8 = CB_ShotPriority_08.SelectedIndex;
            CalcOpts.PriorityIndex9 = CB_ShotPriority_09.SelectedIndex;
            CalcOpts.PriorityIndex10 = CB_ShotPriority_10.SelectedIndex;

            CB_PriorityDefaults.SelectedIndex = 0;

            Character.OnCalculationsInvalidated();
        }

        private void cmbPriorityDefaults_SelectedIndexChanged(object sender, EventArgs e)
        {
            // only do anything if we weren't set to 0
            if (CB_PriorityDefaults.SelectedIndex == 0) return;
            if (isLoading) return;

            isLoading = true;

            if (CB_PriorityDefaults.SelectedIndex == 1) // beast master
            {
                CB_ShotPriority_01.SelectedIndex = 14; // Rapid Fire
                CB_ShotPriority_02.SelectedIndex = 16; // Bestial Wrath
                CB_ShotPriority_03.SelectedIndex = 9; // Kill Shot
                CB_ShotPriority_04.SelectedIndex = 1; // Aimed Shot
                CB_ShotPriority_05.SelectedIndex = 2; // Arcane Shot
                CB_ShotPriority_06.SelectedIndex = 4; // Serpent Sting
                CB_ShotPriority_07.SelectedIndex = 8; // Steady Shot
                CB_ShotPriority_08.SelectedIndex = 0;
                CB_ShotPriority_09.SelectedIndex = 0;
                CB_ShotPriority_10.SelectedIndex = 0;
            }

            if (CB_PriorityDefaults.SelectedIndex == 2) // marksman
            {
                CB_ShotPriority_01.SelectedIndex = 14; // Rapid Fire
                CB_ShotPriority_02.SelectedIndex = 15; // Readiness
                CB_ShotPriority_03.SelectedIndex = 4; // Serpent Sting
                CB_ShotPriority_04.SelectedIndex = 13; // Chimera Shot
                CB_ShotPriority_05.SelectedIndex = 9; // Kill Shot
                CB_ShotPriority_06.SelectedIndex = 1; // Aimed Shot
                CB_ShotPriority_07.SelectedIndex = 7; // Silencing Shot
                CB_ShotPriority_08.SelectedIndex = 8; // Steady Shot
                CB_ShotPriority_09.SelectedIndex = 0;
                CB_ShotPriority_10.SelectedIndex = 0;
            }

            if (CB_PriorityDefaults.SelectedIndex == 3) // survival
            {
                CB_ShotPriority_01.SelectedIndex = 14; // Rapid Fire
                CB_ShotPriority_02.SelectedIndex = 9; // Kill Shot
                CB_ShotPriority_03.SelectedIndex = 10; // Explosive Shot
                CB_ShotPriority_04.SelectedIndex = 11; // Black Arrow
                CB_ShotPriority_05.SelectedIndex = 4; // Serpent Sting
                CB_ShotPriority_06.SelectedIndex = 1; // Aimed Shot
                CB_ShotPriority_07.SelectedIndex = 8; // Steady Shot
                CB_ShotPriority_08.SelectedIndex = 0;
                CB_ShotPriority_09.SelectedIndex = 0;
                CB_ShotPriority_10.SelectedIndex = 0;
            }

            isLoading = false;

            CalcOpts.PriorityIndex1 = CB_ShotPriority_01.SelectedIndex;
            CalcOpts.PriorityIndex2 = CB_ShotPriority_02.SelectedIndex;
            CalcOpts.PriorityIndex3 = CB_ShotPriority_03.SelectedIndex;
            CalcOpts.PriorityIndex4 = CB_ShotPriority_04.SelectedIndex;
            CalcOpts.PriorityIndex5 = CB_ShotPriority_05.SelectedIndex;
            CalcOpts.PriorityIndex6 = CB_ShotPriority_06.SelectedIndex;
            CalcOpts.PriorityIndex7 = CB_ShotPriority_07.SelectedIndex;
            CalcOpts.PriorityIndex8 = CB_ShotPriority_08.SelectedIndex;
            CalcOpts.PriorityIndex9 = CB_ShotPriority_09.SelectedIndex;
            CalcOpts.PriorityIndex10 = CB_ShotPriority_10.SelectedIndex;

            Character.OnCalculationsInvalidated();
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
            switch ((PetFamily)CB_PetFamily.SelectedItem)
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
            return PetFamilyTree.None;
        }

        private void updateTalentDisplay()
        {
            PetFamilyTree tree = getPetFamilyTree();
            GB_PetTalents_Cunning.Visible = tree == PetFamilyTree.Cunning;
            GB_PetTalents_Ferocity.Visible = tree == PetFamilyTree.Ferocity;
            GB_PetTalents_Tenacity.Visible = tree == PetFamilyTree.Tenacity;
        }

        private void resetTalents()
        {
            CalcOpts.petCobraReflexes = 0;
            CalcOpts.petDiveDash = 0;
            CalcOpts.petChargeSwoop = 0;
            CalcOpts.petGreatStamina = 0;
            CalcOpts.petNaturalArmor = 0;
            CalcOpts.petBoarsSpeed = 0;
            CalcOpts.petMobility = 0;
            CalcOpts.petSpikedCollar = 0;
            CalcOpts.petImprovedCower = 0;
            CalcOpts.petBloodthirsty = 0;
            CalcOpts.petBloodOfTheRhino = 0;
            CalcOpts.petPetBarding = 0;
            CalcOpts.petAvoidance = 0;
            CalcOpts.petLionhearted = 0;
            CalcOpts.petCarrionFeeder = 0;
            CalcOpts.petGuardDog = 0;
            CalcOpts.petThunderstomp = 0;
            CalcOpts.petGreatResistance = 0;
            CalcOpts.petOwlsFocus = 0;
            CalcOpts.petCornered = 0;
            CalcOpts.petFeedingFrenzy = 0;
            CalcOpts.petHeartOfThePhoenix = 0;
            CalcOpts.petSpidersBite = 0;
            CalcOpts.petWolverineBite = 0;
            CalcOpts.petRoarOfRecovery = 0;
            CalcOpts.petBullheaded = 0;
            CalcOpts.petGraceOfTheMantis = 0;
            CalcOpts.petRabid = 0;
            CalcOpts.petLickYourWounds = 0;
            CalcOpts.petCallOfTheWild = 0;
            CalcOpts.petLastStand = 0;
            CalcOpts.petTaunt = 0;
            CalcOpts.petIntervene = 0;
            CalcOpts.petWildHunt = 0;
            CalcOpts.petRoarOfSacrifice = 0;
            CalcOpts.petSharkAttack = 0;
            CalcOpts.petSilverback = 0;

            populatePetTalentCombos();
        }

        private void talentComboChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            // one of the (many) talent combo boxes has been updated
            // so we need to update the options

            PetFamilyTree tree = getPetFamilyTree();

            if (tree == PetFamilyTree.Cunning)
            {
                CalcOpts.petCobraReflexes = CB_CunningCorbaReflexes.SelectedIndex;
                CalcOpts.petDiveDash = CB_CunningDiveDash.SelectedIndex;
                CalcOpts.petChargeSwoop = 0;
                CalcOpts.petGreatStamina = CB_CunningGreatStamina.SelectedIndex;
                CalcOpts.petNaturalArmor = CB_CunningNaturalArmor.SelectedIndex;
                CalcOpts.petBoarsSpeed = CB_CunningBoarsSpeed.SelectedIndex;
                CalcOpts.petMobility = CB_CunningMobility.SelectedIndex;
                CalcOpts.petSpikedCollar = CB_CunningSpikedCollar.SelectedIndex;
                CalcOpts.petImprovedCower = 0;
                CalcOpts.petBloodthirsty = 0;
                CalcOpts.petBloodOfTheRhino = 0;
                CalcOpts.petPetBarding = 0;
                CalcOpts.petAvoidance = CB_CunningAvoidance.SelectedIndex;
                CalcOpts.petLionhearted = CB_CunningLionhearted.SelectedIndex;
                CalcOpts.petCarrionFeeder = CB_CunningCarrionFeeder.SelectedIndex;
                CalcOpts.petGuardDog = 0;
                CalcOpts.petThunderstomp = 0;
                CalcOpts.petGreatResistance = CB_CunningGreatResistance.SelectedIndex;
                CalcOpts.petOwlsFocus = CB_CunningOwlsFocus.SelectedIndex;
                CalcOpts.petCornered = CB_CunningCornered.SelectedIndex;
                CalcOpts.petFeedingFrenzy = CB_CunningFeedingFrenzy.SelectedIndex;
                CalcOpts.petHeartOfThePhoenix = 0;
                CalcOpts.petSpidersBite = 0;
                CalcOpts.petWolverineBite = CB_CunningWolverineBite.SelectedIndex;
                CalcOpts.petRoarOfRecovery = CB_CunningRoarOfRecovery.SelectedIndex;
                CalcOpts.petBullheaded = CB_CunningBullheaded.SelectedIndex;
                CalcOpts.petGraceOfTheMantis = CB_CunningGraceOfTheMantis.SelectedIndex;
                CalcOpts.petRabid = 0;
                CalcOpts.petLickYourWounds = 0;
                CalcOpts.petCallOfTheWild = 0;
                CalcOpts.petLastStand = 0;
                CalcOpts.petTaunt = 0;
                CalcOpts.petIntervene = 0;
                CalcOpts.petWildHunt = CB_CunningWildHunt.SelectedIndex;
                CalcOpts.petRoarOfSacrifice = CB_CunningRoarOfSacrifice.SelectedIndex;
                CalcOpts.petSharkAttack = 0;
                CalcOpts.petSilverback = 0;
            }

            if (tree == PetFamilyTree.Ferocity)
            {
                CalcOpts.petCobraReflexes = CB_FerocityCobraReflexes.SelectedIndex;
                CalcOpts.petDiveDash = CB_FerocityDiveDash.SelectedIndex;
                CalcOpts.petChargeSwoop = CB_FerocityChargeSwoop.SelectedIndex;
                CalcOpts.petGreatStamina = CB_FerocityGreatStamina.SelectedIndex;
                CalcOpts.petNaturalArmor = CB_FerocityNaturalArmor.SelectedIndex;
                CalcOpts.petBoarsSpeed = CB_FerocityBoarsSpeed.SelectedIndex;
                CalcOpts.petMobility = 0;
                CalcOpts.petSpikedCollar = CB_FerocitySpikedCollar.SelectedIndex;
                CalcOpts.petImprovedCower = CB_FerocityImprovedCower.SelectedIndex;
                CalcOpts.petBloodthirsty = CB_FerocityBloodthirsty.SelectedIndex;
                CalcOpts.petBloodOfTheRhino = 0;
                CalcOpts.petPetBarding = 0;
                CalcOpts.petAvoidance = CB_FerocityAvoidance.SelectedIndex;
                CalcOpts.petLionhearted = CB_FerocityLionhearted.SelectedIndex;
                CalcOpts.petCarrionFeeder = 0;
                CalcOpts.petGuardDog = 0;
                CalcOpts.petThunderstomp = 0;
                CalcOpts.petGreatResistance = CB_FerocityGreatResistance.SelectedIndex;
                CalcOpts.petOwlsFocus = 0;
                CalcOpts.petCornered = 0;
                CalcOpts.petFeedingFrenzy = 0;
                CalcOpts.petHeartOfThePhoenix = CB_FerocityHeartOfThePhoenix.SelectedIndex;
                CalcOpts.petSpidersBite = CB_FerocitySpidersBite.SelectedIndex;
                CalcOpts.petWolverineBite = 0;
                CalcOpts.petRoarOfRecovery = 0;
                CalcOpts.petBullheaded = 0;
                CalcOpts.petGraceOfTheMantis = 0;
                CalcOpts.petRabid = CB_FerocityRabid.SelectedIndex;
                CalcOpts.petLickYourWounds = CB_FerocityLickYourWounds.SelectedIndex;
                CalcOpts.petCallOfTheWild = CB_FerocityCallOfTheWild.SelectedIndex;
                CalcOpts.petLastStand = 0;
                CalcOpts.petTaunt = 0;
                CalcOpts.petIntervene = 0;
                CalcOpts.petWildHunt = CB_FerocityWildHunt.SelectedIndex;
                CalcOpts.petRoarOfSacrifice = 0;
                CalcOpts.petSharkAttack = CB_FerocitySharkAttack.SelectedIndex;
                CalcOpts.petSilverback = 0;
            }

            if (tree == PetFamilyTree.Tenacity)
            {
                CalcOpts.petCobraReflexes = CB_TenacityCobraReflexes.SelectedIndex;
                CalcOpts.petDiveDash = 0;
                CalcOpts.petChargeSwoop = CB_TenacityCharge.SelectedIndex;
                CalcOpts.petGreatStamina = CB_TenacityGreatStamina.SelectedIndex;
                CalcOpts.petNaturalArmor = CB_TenacityNaturalArmor.SelectedIndex;
                CalcOpts.petBoarsSpeed = CB_TenacityBoarsSpeed.SelectedIndex;
                CalcOpts.petMobility = 0;
                CalcOpts.petSpikedCollar = CB_TenacitySpikedCollar.SelectedIndex;
                CalcOpts.petImprovedCower = 0;
                CalcOpts.petBloodthirsty = 0;
                CalcOpts.petBloodOfTheRhino = CB_TenacityBloodOfTheRhino.SelectedIndex;
                CalcOpts.petPetBarding = CB_TenacityPetBarding.SelectedIndex;
                CalcOpts.petAvoidance = CB_TenacityAvoidance.SelectedIndex;
                CalcOpts.petLionhearted = CB_TenacityLiohearted.SelectedIndex;
                CalcOpts.petCarrionFeeder = 0;
                CalcOpts.petGuardDog = CB_TenacityGuardDog.SelectedIndex;
                CalcOpts.petThunderstomp = CB_TenacityThunderstomp.SelectedIndex;
                CalcOpts.petGreatResistance = CB_TenacityGreatResistance.SelectedIndex;
                CalcOpts.petOwlsFocus = 0;
                CalcOpts.petCornered = 0;
                CalcOpts.petFeedingFrenzy = 0;
                CalcOpts.petHeartOfThePhoenix = 0;
                CalcOpts.petSpidersBite = 0;
                CalcOpts.petWolverineBite = 0;
                CalcOpts.petRoarOfRecovery = 0;
                CalcOpts.petBullheaded = 0;
                CalcOpts.petGraceOfTheMantis = CB_TenacityGraceOfTheMantis.SelectedIndex;
                CalcOpts.petRabid = 0;
                CalcOpts.petLickYourWounds = 0;
                CalcOpts.petCallOfTheWild = 0;
                CalcOpts.petLastStand = CB_TenacityLastStand.SelectedIndex;
                CalcOpts.petTaunt = CB_TenacityTaunt.SelectedIndex;
                CalcOpts.petIntervene = CB_TenacityIntervene.SelectedIndex;
                CalcOpts.petWildHunt = CB_TenacityWildHunt.SelectedIndex;
                CalcOpts.petRoarOfSacrifice = CB_TenacityRoarOfSacrifice.SelectedIndex;
                CalcOpts.petSharkAttack = 0;
                CalcOpts.petSilverback = CB_TenacitySilverback.SelectedIndex;
            }

            if (tree == PetFamilyTree.None)
            {
                CalcOpts.petCobraReflexes = 0;
                CalcOpts.petDiveDash = 0;
                CalcOpts.petChargeSwoop = 0;
                CalcOpts.petGreatStamina = 0;
                CalcOpts.petNaturalArmor = 0;
                CalcOpts.petBoarsSpeed = 0;
                CalcOpts.petMobility = 0;
                CalcOpts.petSpikedCollar = 0;
                CalcOpts.petImprovedCower = 0;
                CalcOpts.petBloodthirsty = 0;
                CalcOpts.petBloodOfTheRhino = 0;
                CalcOpts.petPetBarding = 0;
                CalcOpts.petAvoidance = 0;
                CalcOpts.petLionhearted = 0;
                CalcOpts.petCarrionFeeder = 0;
                CalcOpts.petGuardDog = 0;
                CalcOpts.petThunderstomp = 0;
                CalcOpts.petGreatResistance = 0;
                CalcOpts.petOwlsFocus = 0;
                CalcOpts.petCornered = 0;
                CalcOpts.petFeedingFrenzy = 0;
                CalcOpts.petHeartOfThePhoenix = 0;
                CalcOpts.petSpidersBite = 0;
                CalcOpts.petWolverineBite = 0;
                CalcOpts.petRoarOfRecovery = 0;
                CalcOpts.petBullheaded = 0;
                CalcOpts.petGraceOfTheMantis = 0;
                CalcOpts.petRabid = 0;
                CalcOpts.petLickYourWounds = 0;
                CalcOpts.petCallOfTheWild = 0;
                CalcOpts.petLastStand = 0;
                CalcOpts.petTaunt = 0;
                CalcOpts.petIntervene = 0;
                CalcOpts.petWildHunt = 0;
                CalcOpts.petRoarOfSacrifice = 0;
                CalcOpts.petSharkAttack = 0;
                CalcOpts.petSilverback = 0;
            }

            Character.OnCalculationsInvalidated();
        }

        private void populatePetTalentCombos()
        {
            // called when options are loaded to allow us to set the selected indexes
            
            PetFamilyTree tree = getPetFamilyTree();
            
            CB_CunningAvoidance.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petAvoidance : 0;
            CB_CunningBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petBoarsSpeed : 0;
            CB_CunningBullheaded.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petBullheaded : 0;
            CB_CunningCarrionFeeder.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petCarrionFeeder : 0;
            CB_CunningCorbaReflexes.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petCobraReflexes : 0;
            CB_CunningCornered.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petCornered : 0;
            CB_CunningDiveDash.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petDiveDash : 0;
            CB_CunningFeedingFrenzy.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petFeedingFrenzy : 0;
            CB_CunningGraceOfTheMantis.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petGraceOfTheMantis : 0;
            CB_CunningGreatResistance.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petGreatResistance : 0;
            CB_CunningGreatStamina.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petGreatStamina : 0;
            CB_CunningLionhearted.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petLionhearted : 0;
            CB_CunningMobility.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petMobility : 0;
            CB_CunningNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petNaturalArmor : 0;
            CB_CunningOwlsFocus.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petOwlsFocus : 0;
            CB_CunningRoarOfRecovery.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petRoarOfRecovery : 0;
            CB_CunningRoarOfSacrifice.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petRoarOfSacrifice : 0;
            CB_CunningSpikedCollar.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petSpikedCollar : 0;
            CB_CunningWildHunt.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petWildHunt : 0;
            CB_CunningWolverineBite.SelectedIndex = (tree == PetFamilyTree.Cunning) ? CalcOpts.petWolverineBite : 0;

            CB_FerocityAvoidance.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petAvoidance : 0;
            CB_FerocityBloodthirsty.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petBloodthirsty : 0;
            CB_FerocityBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petBoarsSpeed : 0;
            CB_FerocityCallOfTheWild.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petCallOfTheWild : 0;
            CB_FerocityChargeSwoop.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petChargeSwoop : 0;
            CB_FerocityCobraReflexes.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petCobraReflexes : 0;
            CB_FerocityDiveDash.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petDiveDash : 0;
            CB_FerocityGreatResistance.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petGreatResistance : 0;
            CB_FerocityGreatStamina.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petGreatStamina : 0;
            CB_FerocityHeartOfThePhoenix.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petHeartOfThePhoenix : 0;
            CB_FerocityImprovedCower.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petImprovedCower : 0;
            CB_FerocityLickYourWounds.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petLickYourWounds : 0;
            CB_FerocityLionhearted.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petLionhearted : 0;
            CB_FerocityNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petNaturalArmor : 0;
            CB_FerocityRabid.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petRabid : 0;
            CB_FerocitySharkAttack.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petSharkAttack : 0;
            CB_FerocitySpidersBite.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petSpidersBite : 0;
            CB_FerocitySpikedCollar.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petSpikedCollar : 0;
            CB_FerocityWildHunt.SelectedIndex = (tree == PetFamilyTree.Ferocity) ? CalcOpts.petWildHunt : 0;

            CB_TenacityAvoidance.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petAvoidance : 0;
            CB_TenacityBloodOfTheRhino.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petBloodOfTheRhino : 0;
            CB_TenacityBoarsSpeed.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petBoarsSpeed : 0;
            CB_TenacityCharge.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petChargeSwoop : 0;
            CB_TenacityCobraReflexes.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petCobraReflexes : 0;
            CB_TenacityGraceOfTheMantis.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petGraceOfTheMantis : 0;
            CB_TenacityGreatResistance.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petGreatResistance : 0;
            CB_TenacityGreatStamina.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petGreatStamina : 0;
            CB_TenacityGuardDog.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petGuardDog : 0;
            CB_TenacityIntervene.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petIntervene : 0;
            CB_TenacityLastStand.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petLastStand : 0;
            CB_TenacityLiohearted.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petLionhearted : 0;
            CB_TenacityNaturalArmor.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petNaturalArmor : 0;
            CB_TenacityPetBarding.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petPetBarding : 0;
            CB_TenacityRoarOfSacrifice.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petRoarOfSacrifice : 0;
            CB_TenacitySilverback.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petSilverback : 0;
            CB_TenacitySpikedCollar.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petSpikedCollar : 0;
            CB_TenacityTaunt.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petTaunt : 0;
            CB_TenacityThunderstomp.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petThunderstomp : 0;
            CB_TenacityWildHunt.SelectedIndex = (tree == PetFamilyTree.Tenacity) ? CalcOpts.petWildHunt : 0;
        }

        private void numericTime20_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.timeSpentSub20 = (int)NUD_Time20.Value;
            Character.OnCalculationsInvalidated();
        }

        private void numericTime35_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.timeSpent35To20 = (int)NUD_35.Value;
            Character.OnCalculationsInvalidated();
        }

        private void numericBossHP_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.bossHPPercentage = (float)(NUD_BossHP.Value / 100);
            Character.OnCalculationsInvalidated();
        }

        private void cmbManaPotion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (CB_ManaPotion.SelectedIndex == 0) CalcOpts.useManaPotion = ManaPotionType.None;
            if (CB_ManaPotion.SelectedIndex == 1) CalcOpts.useManaPotion = ManaPotionType.RunicManaPotion;
            if (CB_ManaPotion.SelectedIndex == 2) CalcOpts.useManaPotion = ManaPotionType.SuperManaPotion;
            Character.OnCalculationsInvalidated();
        }

        private void cmbAspect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (CB_Aspect.SelectedIndex == 0) CalcOpts.selectedAspect = Aspect.None;
            if (CB_Aspect.SelectedIndex == 1) CalcOpts.selectedAspect = Aspect.Beast;
            if (CB_Aspect.SelectedIndex == 2) CalcOpts.selectedAspect = Aspect.Hawk;
            if (CB_Aspect.SelectedIndex == 3) CalcOpts.selectedAspect = Aspect.Viper;
            if (CB_Aspect.SelectedIndex == 4) CalcOpts.selectedAspect = Aspect.Monkey;
            if (CB_Aspect.SelectedIndex == 5) CalcOpts.selectedAspect = Aspect.Dragonhawk;
            Character.OnCalculationsInvalidated();
        }

        private void cmbAspectUsage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (CB_AspectUsage.SelectedIndex == 0) CalcOpts.aspectUsage = AspectUsage.AlwaysOn;
            if (CB_AspectUsage.SelectedIndex == 1) CalcOpts.aspectUsage = AspectUsage.ViperToOOM;
            if (CB_AspectUsage.SelectedIndex == 2) CalcOpts.aspectUsage = AspectUsage.ViperRegen;
            Character.OnCalculationsInvalidated();
        }

        private void chkUseBeastDuringBW_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.useBeastDuringBeastialWrath = CK_UseBeastDuringBW.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chkEmulateBugs_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.emulateSpreadsheetBugs = CK_EmulateBugs.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chkSpreadsheetUptimes_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.calculateUptimesLikeSpreadsheet = CK_SpreadsheetUptimes.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void CalculationOptionsPanelHunter_Resize(object sender, EventArgs e)
        {
            Tabs.Height = Tabs.Parent.Height - 5;
        }

        private void chkUseRotation_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.useRotationTest = CK_UseRotation.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chkRandomProcs_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            CalcOpts.randomizeProcs = CK_RandomProcs.Checked;
            Character.OnCalculationsInvalidated();
        }

        // Hiding Based on Bad Stats
        private void CK_HideBadItems_Spl_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalculationOptionsHunterSE calcOpts = Character.CalculationOptions as CalculationOptionsHunterSE;
                calcOpts.HideBadItems_Spl = CK_HideSplGear.Checked;
                CalculationsHunterSE.HidingBadStuff_Spl = calcOpts.HideBadItems_Spl;
                ItemCache.OnItemsChanged();
                Character.OnCalculationsInvalidated();
            }
        }
        private void CK_HideBadItems_PvP_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                CalculationOptionsHunterSE calcOpts = Character.CalculationOptions as CalculationOptionsHunterSE;
                calcOpts.HideBadItems_PvP = CK_HidePvPGear.Checked;
                CalculationsHunterSE.HidingBadStuff_PvP = calcOpts.HideBadItems_PvP;
                ItemCache.OnItemsChanged();
                Character.OnCalculationsInvalidated();
            }
        }
        // Hiding Enchants based on Profession
        private void CK_HideProfEnchants_CheckedChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CalculationOptionsHunterSE calcOpts = Character.CalculationOptions as CalculationOptionsHunterSE;
                bool Checked = CK_HideProfEnchants.Checked;
                calcOpts.HideProfEnchants = Checked;
                CalculationsHunterSE.HidingBadStuff_Prof = calcOpts.HideProfEnchants;
                CB_Prof1.Enabled = Checked;
                CB_Prof2.Enabled = Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        private void CB_Prof1_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                Character.PrimaryProfession = StringToProfession(CB_Prof1.Text);
                Character.OnCalculationsInvalidated();
            }
        }
        private void CB_Prof2_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                Character.SecondaryProfession = StringToProfession(CB_Prof2.Text);
                Character.OnCalculationsInvalidated();
            }
        }
        public Profession StringToProfession(string s) {
            Profession                        p = Profession.None;
            if      (s == "Alchemy"       ) { p = Profession.Alchemy;
            }else if(s == "Blacksmithing" ) { p = Profession.Blacksmithing;
            }else if(s == "Enchanting"    ) { p = Profession.Enchanting;
            }else if(s == "Engineering"   ) { p = Profession.Engineering;
            }else if(s == "Herbalism"     ) { p = Profession.Herbalism;
            }else if(s == "Inscription"   ) { p = Profession.Inscription;
            }else if(s == "Jewelcrafting" ) { p = Profession.Jewelcrafting;
            }else if(s == "Leatherworking") { p = Profession.Leatherworking;
            }else if(s == "Mining"        ) { p = Profession.Mining;
            }else if(s == "Skinning"      ) { p = Profession.Skinning;
            }else if(s == "Tailoring"     ) { p = Profession.Tailoring; }
            return p;
        }
        public string ProfessionToString(Profession p) {
            string                                     s = "None";
            if      (p == Profession.Alchemy       ) { s = "Alchemy";
            }else if(p == Profession.Blacksmithing ) { s = "Blacksmithing";
            }else if(p == Profession.Enchanting    ) { s = "Enchanting";
            }else if(p == Profession.Engineering   ) { s = "Engineering";
            }else if(p == Profession.Herbalism     ) { s = "Herbalism";
            }else if(p == Profession.Inscription   ) { s = "Inscription";
            }else if(p == Profession.Jewelcrafting ) { s = "Jewelcrafting";
            }else if(p == Profession.Leatherworking) { s = "Leatherworking";
            }else if(p == Profession.Mining        ) { s = "Mining";
            }else if(p == Profession.Skinning      ) { s = "Skinning";
            }else if(p == Profession.Tailoring     ) { s = "Tailoring"; }
            return s;
        }
    }
}