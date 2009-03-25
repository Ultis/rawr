using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class AttackModel
    {
        private Character Character;
        private CalculationOptionsProtPaladin Options;
        private Stats Stats;
        private DefendTable DefendTable;
        private ParryModel ParryModel;

        public AbilityModelList Abilities = new AbilityModelList();

        private AttackModelMode _attackModelMode;
        public AttackModelMode AttackModelMode
        {
            get { return _attackModelMode; }
            set { _attackModelMode = value; Calculate(); }
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float ThreatPerSecond { get; private set; }
        public float DamagePerSecond { get; private set; }

        private void Calculate()
        {
            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;

            switch (AttackModelMode)
            {
                case AttackModelMode.BasicSoV:
                    {
                        // Basic Rotation (Assumes Judgement of Vengeance)
                        Name        = "Basic + Seal of Vengeance";
                        Description = "9-6-9-6 Rotation";
                        modelLength = 18.0f;
                        modelThreat = 
                            Abilities[Ability.ShieldOfRighteousness].Threat * 3 + 
                            Abilities[Ability.HammerOfTheRighteous].Threat * 3 + 
                            Abilities[Ability.JudgementOfVengeance].Threat * 2 +
                            Abilities[Ability.Consecration].Threat * 2;
                        modelDamage =
                            Abilities[Ability.ShieldOfRighteousness].Damage * 3 +
                            Abilities[Ability.HammerOfTheRighteous].Damage * 3 +
                            Abilities[Ability.JudgementOfVengeance].Damage * 2 +
                            Abilities[Ability.Consecration].Damage * 2;
                        modelCrits  =
                            Abilities[Ability.ShieldOfRighteousness].CritPercentage * 3 +
                            Abilities[Ability.HammerOfTheRighteous].CritPercentage * 3 +
                            Abilities[Ability.JudgementOfVengeance].CritPercentage * 2 +
                            Abilities[Ability.Consecration].CritPercentage * 2;
                        break;
                    }
                case AttackModelMode.BasicSoR:
                    {                        
                        // Basic Rotation (Assumes Judgement of Righteousness)
                        Name = "Basic + Seal of Vengeance";
                        Description = "9-6-9-6 Rotation";
                        modelLength = 18.0f;
                        modelThreat =
                            Abilities[Ability.ShieldOfRighteousness].Threat * 3 +
                            Abilities[Ability.HammerOfTheRighteous].Threat * 3 +
                            Abilities[Ability.JudgementOfRighteousness].Threat * 2 +
                            Abilities[Ability.Consecration].Threat * 2;
                        modelDamage =
                            Abilities[Ability.ShieldOfRighteousness].Damage * 3 +
                            Abilities[Ability.HammerOfTheRighteous].Damage * 3 +
                            Abilities[Ability.JudgementOfRighteousness].Damage * 2 +
                            Abilities[Ability.Consecration].Damage * 2;
                        modelCrits =
                            Abilities[Ability.ShieldOfRighteousness].CritPercentage * 3 +
                            Abilities[Ability.HammerOfTheRighteous].CritPercentage * 3 +
                            Abilities[Ability.JudgementOfRighteousness].CritPercentage * 2 +
                            Abilities[Ability.Consecration].CritPercentage * 2;
                        break;
                    }
            }

            // White Damage
            float weaponHits = modelLength / ParryModel.WeaponSpeed; //Lookup.WeaponSpeed(Character, Stats);
            modelThreat += Abilities[Ability.None].Threat * weaponHits;
            modelDamage += Abilities[Ability.None].Damage * weaponHits;
            modelCrits  += Abilities[Ability.None].CritPercentage * weaponHits;
			
			// Seals
			weaponHits += modelLength / 6.0f; // Add Seal Damage from Hammer of the Righteous
            switch (Options.SealChoice)
            {
                // Seal of Righteousness
                case "Seal of Righteousness":				
                    modelThreat += Abilities[Ability.SealOfRighteousness].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfRighteousness].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfRighteousness].CritPercentage * weaponHits;
                    break;
				//Seal of Vengeance Mode
				case "Seal of Vengeance":
					modelThreat += Abilities[Ability.SealOfVengeance].Threat * weaponHits;
					modelDamage += Abilities[Ability.SealOfVengeance].Damage * weaponHits;
					modelCrits  += Abilities[Ability.SealOfVengeance].CritPercentage * weaponHits;
                    
                    //Holy Vengeance (Seal of Vengeance DOT)
                    modelThreat += Abilities[Ability.HolyVengeance].Threat;
                    modelDamage += Abilities[Ability.HolyVengeance].Damage;
					break;
			}

            if (Options.UseHolyShield)
            {
                // Holy Shield
                // TODO: Model Holy Shield Charges
                float attackerBlocks = DefendTable.Block * (modelLength / ParryModel.BossAttackSpeed); //Options.BossAttackSpeed;
                modelThreat += Abilities[Ability.HolyShield].Threat * attackerBlocks;
                modelDamage += Abilities[Ability.HolyShield].Damage * attackerBlocks;
                modelCrits += Abilities[Ability.HolyShield].CritPercentage * attackerBlocks;
            }

            ThreatPerSecond = modelThreat / modelLength;
            DamagePerSecond = modelDamage / modelLength;
        }

        public AttackModel(Character character, Stats stats, AttackModelMode attackModelMode) //, RageModelMode rageModelMode)
        {
            Character        = character;
            Options          = Character.CalculationOptions as CalculationOptionsProtPaladin;
            Stats            = stats;
            DefendTable      = new DefendTable(character, stats);
            ParryModel       = new ParryModel(character, stats);
            _attackModelMode = attackModelMode;

            Abilities.Add(Ability.None, character, stats);
            Abilities.Add(Ability.ShieldOfRighteousness, character, stats);
            Abilities.Add(Ability.HammerOfTheRighteous, character, stats);
            Abilities.Add(Ability.SealOfVengeance, character, stats);
            Abilities.Add(Ability.HolyVengeance, character, stats);
            Abilities.Add(Ability.JudgementOfVengeance, character, stats);
            Abilities.Add(Ability.SealOfRighteousness, character, stats);
            Abilities.Add(Ability.JudgementOfRighteousness, character, stats);
            Abilities.Add(Ability.Exorcism, character, stats);
            Abilities.Add(Ability.HammerOfWrath, character, stats);
            Abilities.Add(Ability.AvengersShield, character, stats);
            Abilities.Add(Ability.HolyShield, character, stats);
            Abilities.Add(Ability.RetributionAura, character, stats);
            Abilities.Add(Ability.Consecration, character, stats);

            Calculate();
        }
    }
}