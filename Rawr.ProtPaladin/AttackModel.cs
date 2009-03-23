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
                case AttackModelMode.Basic:
                    {
                        // Basic Rotation (Assumes Judgement of Vengeance)
						// TODO Judgement + Seal Support
                        Name        = "Basic*9-6-9-6";
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
            }

            // White Damage
            float weaponHits = modelLength / ParryModel.WeaponSpeed; //Lookup.WeaponSpeed(Character, Stats);
            modelThreat += Abilities[Ability.None].Threat * weaponHits;
            modelDamage += Abilities[Ability.None].Damage * weaponHits;
            modelCrits  += Abilities[Ability.None].CritPercentage * weaponHits;
			
			// Seals
			weaponHits += modelLength / 6.0f; // Add Seal Damage from Hammer of the Righteous
            //switch (SealMode)
            //{
                //// Seal of Righteousness
                //case SealMode.Righteousness:				
                //    modelThreat += Abilities[Ability.SealOfRighteousness].Threat * weaponHits;
                //    modelDamage += Abilities[Ability.SealOfRighteousness].Damage * weaponHits;
                //    modelCrits  += Abilities[Ability.SealOfRighteousness].CritPercentage * weaponHits;
                //    break;
				// Seal of Vengeance
				//case SealMode.Vengeance:
					modelThreat += Abilities[Ability.SealOfVengeance].Threat * weaponHits;
					modelDamage += Abilities[Ability.SealOfVengeance].Damage * weaponHits;
					modelCrits  += Abilities[Ability.SealOfVengeance].CritPercentage * weaponHits;
					//break;
			//}


            // Holy Shield
            // TODO: Model Holy Shield Charges
            float attackerHits = DefendTable.AnyHit * (modelLength / ParryModel.BossAttackSpeed); //Options.BossAttackSpeed;
            modelThreat += Abilities[Ability.HolyShield].Threat * attackerHits;
            modelDamage += Abilities[Ability.HolyShield].Damage * attackerHits;
            modelCrits  += Abilities[Ability.HolyShield].CritPercentage * attackerHits;

            // Holy Vengeance (Seal of Vengeance DOT)
            modelThreat += Abilities[Ability.HolyVengeance].Threat * modelCrits;
            modelDamage += Abilities[Ability.HolyVengeance].Damage * modelCrits;

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