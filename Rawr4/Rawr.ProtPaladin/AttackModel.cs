using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class AttackModel
    {
        private Character Character;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;
        private Stats Stats;
        private DefendTable DefendTable;
        private ParryModel ParryModel;

        public AbilityModelList Abilities = new AbilityModelList();

        private AttackModelMode _attackModelMode;
        public AttackModelMode AttackModelMode {
            get { return _attackModelMode; }
            set { _attackModelMode = value; Calculate(); }
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float ThreatPerSecond { get; private set; }
        public float DamagePerSecond { get; private set; }
        public float AttackerHitsPerSecond { get; private set; }

        private void Calculate() {
            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;

            switch (AttackModelMode) {
                case AttackModelMode.BasicSoV: {
                    // Basic Rotation (Assumes Judgement of Vengeance)
                    Name        = "Basic + Seal of Vengeance";
                    Description = "9-6-9-6 Rotation";
                    modelLength = 18.0f;
                    modelThreat = 
                        Abilities[Ability.ShieldOfTheRighteous].Threat * 3 + 
                        Abilities[Ability.HammerOfTheRighteous].Threat * 3 + 
                        Abilities[Ability.JudgementOfTruth].Threat * 2 +
                        Abilities[Ability.Consecration].Threat * 2;
                    modelDamage =
                        Abilities[Ability.ShieldOfTheRighteous].Damage * 3 +
                        Abilities[Ability.HammerOfTheRighteous].Damage * 3 +
                        Abilities[Ability.JudgementOfTruth].Damage * 2 +
                        Abilities[Ability.Consecration].Damage * 2;
                    modelCrits  =
                        Abilities[Ability.ShieldOfTheRighteous].CritPercentage * 3 +
                        Abilities[Ability.HammerOfTheRighteous].CritPercentage * 3 +
                        Abilities[Ability.JudgementOfTruth].CritPercentage * 2 +
                        Abilities[Ability.Consecration].CritPercentage * 2;
                    break;
                }
                case AttackModelMode.BasicSoR: {                        
                    // Basic Rotation (Assumes Judgement of Righteousness)
                    Name = "Basic + Seal of Righteousness";
                    Description = "9-6-9-6 Rotation";
                    modelLength = 18.0f;
                    modelThreat =
                        Abilities[Ability.ShieldOfTheRighteous].Threat * 3 +
                        Abilities[Ability.HammerOfTheRighteous].Threat * 3 +
                        Abilities[Ability.JudgementOfRighteousness].Threat * 2 +
                        Abilities[Ability.Consecration].Threat * 2;
                    modelDamage =
                        Abilities[Ability.ShieldOfTheRighteous].Damage * 3 +
                        Abilities[Ability.HammerOfTheRighteous].Damage * 3 +
                        Abilities[Ability.JudgementOfRighteousness].Damage * 2 +
                        Abilities[Ability.Consecration].Damage * 2;
                    modelCrits =
                        Abilities[Ability.ShieldOfTheRighteous].CritPercentage * 3 +
                        Abilities[Ability.HammerOfTheRighteous].CritPercentage * 3 +
                        Abilities[Ability.JudgementOfRighteousness].CritPercentage * 2 +
                        Abilities[Ability.Consecration].CritPercentage * 2;
                    break;
                }
            }

            // White Damage
            float reckoningUptime = 1f - (float)Math.Pow((1f - 0.02f * Character.PaladinTalents.Reckoning * DefendTable.AnyHit), (Math.Min(8f, 4f * ParryModel.WeaponSpeed) / ParryModel.BossAttackSpeed));
            float weaponHits = modelLength / ParryModel.WeaponSpeed / (1 - reckoningUptime); //Lookup.WeaponSpeed(Character, Stats);
            modelThreat += Abilities[Ability.MeleeSwing].Threat * weaponHits;
            modelDamage += Abilities[Ability.MeleeSwing].Damage * weaponHits;
            modelCrits  += Abilities[Ability.MeleeSwing].CritPercentage * weaponHits;
            
            // Seals
            weaponHits += modelLength / 6.0f; // Add Seal Damage from Hammer of the Righteous
            switch (CalcOpts.SealChoice) {
                // Seal of Righteousness
                case "Seal of Righteousness":				
                    modelThreat += Abilities[Ability.SealOfRighteousness].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfRighteousness].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfRighteousness].CritPercentage * weaponHits;
                    break;
                //Seal of Truth Mode
                case "Seal of Truth":
                    modelThreat += Abilities[Ability.SealOfTruth].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfTruth].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfTruth].CritPercentage * weaponHits;

                    //Censure (Seal of Truth DOT)
                    modelThreat += Abilities[Ability.Censure].Threat;
                    modelDamage += Abilities[Ability.Censure].Damage;
                    break;
            }

            float attackerHits = DefendTable.AnyHit * (modelLength / ParryModel.BossAttackSpeed); //Options.BossAttackSpeed;

            ThreatPerSecond = modelThreat / modelLength;
            DamagePerSecond = modelDamage / modelLength;
            AttackerHitsPerSecond = attackerHits / modelLength;
        }

        public AttackModel(Character character, Stats stats, AttackModelMode attackModelMode, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Character        = character;
            CalcOpts         = calcOpts;
            BossOpts         = bossOpts;
            Stats            = stats;
            DefendTable      = new DefendTable(character, stats, calcOpts, bossOpts);
            ParryModel       = new ParryModel(character, stats, calcOpts, bossOpts);
            _attackModelMode = attackModelMode;

            Abilities.Add(Ability.MeleeSwing, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.ShieldOfTheRighteous, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HammerOfTheRighteous, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.SealOfTruth, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.Censure, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.JudgementOfTruth, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.SealOfRighteousness, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.JudgementOfRighteousness, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HammerOfWrath, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.AvengersShield, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.RetributionAura, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HolyWrath, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.Consecration, character, stats, calcOpts, bossOpts);

            Calculate();
        }
    }
}
