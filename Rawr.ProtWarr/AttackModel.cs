using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AttackModel
    {
        private Character Character;
        private CalculationOptionsProtWarr Options;
        private Stats Stats;
        private WarriorTalents Talents;
        private DefendTable DefendTable;
        private ParryModel ParryModel;

        public AbilityModelList Abilities = new AbilityModelList();

        private AttackModelMode _attackModelMode;
        public AttackModelMode AttackModelMode
        {
            get { return _attackModelMode; }
            set { _attackModelMode = value; Calculate(); }
        }

        private RageModelMode _rageModelMode;
        public RageModelMode RageModelMode
        {
            get { return _rageModelMode; }
            set { _rageModelMode = value; Calculate(); }
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float ThreatPerSecond { get; private set; }
        public float DamagePerSecond { get; private set; }
        public float WeaponAttacksPerSecond { get; private set; }
        public float HitsPerSecond { get; private set; }
        public float CritsPerSecond { get; private set; }
        public float AttackerSwingsPerSecond { get; private set; }
        public float AttackerHitsPerSecond { get; private set; }

        private void Calculate()
        {
            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;
            float modelHits = 0.0f;
            float modelWeaponAttacks = 0.0f;

            switch (AttackModelMode)
            {
                case AttackModelMode.Basic:
                    {
                        // Basic Rotation
                        // Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor
                        Name        = "Basic Cycle";
                        Description = "Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor";
                        modelLength = 6.0f;
                        modelThreat = 
                            Abilities[Ability.ShieldSlam].Threat + 
                            Abilities[Ability.Revenge].Threat + 
                            Abilities[Ability.SunderArmor].Threat * 2;
                        modelDamage = 
                            Abilities[Ability.ShieldSlam].Damage +
                            Abilities[Ability.Revenge].Damage;
                        modelHits =
                            Abilities[Ability.ShieldSlam].HitPercentage +
                            Abilities[Ability.Revenge].HitPercentage;
                        modelCrits  = 
                            Abilities[Ability.ShieldSlam].CritPercentage +
                            Abilities[Ability.Revenge].CritPercentage;
                        modelWeaponAttacks = 2.0f;
                        break;
                    }
                case AttackModelMode.Devastate:
                    {
                        // Devastate Rotation
                        // Shield Slam -> Revenge -> Devastate -> Devastate
                        if (Character.WarriorTalents.Devastate == 1)
                        {
                            Name        = "Devastate";
                            Description = "Shield Slam -> Revenge -> Devastate -> Devastate";
                            modelLength = 6.0f;
                            modelThreat =
                                Abilities[Ability.ShieldSlam].Threat +
                                Abilities[Ability.Revenge].Threat +
                                Abilities[Ability.Devastate].Threat * 2;
                            modelDamage =
                                Abilities[Ability.ShieldSlam].Damage +
                                Abilities[Ability.Revenge].Damage +
                                Abilities[Ability.Devastate].Damage * 2;
                            modelHits =
                               Abilities[Ability.ShieldSlam].HitPercentage +
                               Abilities[Ability.Revenge].HitPercentage +
                               Abilities[Ability.Devastate].HitPercentage * 2;
                            modelCrits =
                                Abilities[Ability.ShieldSlam].CritPercentage +
                                Abilities[Ability.Revenge].CritPercentage +
                                Abilities[Ability.Devastate].CritPercentage * 2;
                            modelWeaponAttacks = 4.0f;
                        }
                        else
                            goto case AttackModelMode.Basic;
                        break;
                    }
                case AttackModelMode.SwordAndBoard:
                    {
                        // Sword And Board Rotation
                        // Requires 3 points in Sword and Board
                        // Shield Slam > Revenge > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.4596 * Devastate
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3)
                        {
                            Name        = "Sword And Board";
                            Description = "Shield Slam > Revenge > Devastate";
                            modelLength = 4.7644f;
                            modelThreat =
                                (1.0f * Abilities[Ability.ShieldSlam].Threat) +
                                (0.73f * Abilities[Ability.Revenge].Threat) +
                                (1.4596f * Abilities[Ability.Devastate].Threat);
                            modelDamage = 
                                (1.0f * Abilities[Ability.ShieldSlam].Damage) +
                                (0.73f * Abilities[Ability.Revenge].Damage) +
                                (1.4596f * Abilities[Ability.Devastate].Damage);
                            modelHits =
                                (1.0f * Abilities[Ability.ShieldSlam].HitPercentage) +
                                (0.73f * Abilities[Ability.Revenge].HitPercentage) +
                                (1.4596f * Abilities[Ability.Devastate].HitPercentage);
                            modelCrits = 
                                (1.0f * Abilities[Ability.ShieldSlam].CritPercentage) +
                                (0.73f * Abilities[Ability.Revenge].CritPercentage) +
                                (1.4596f * Abilities[Ability.Devastate].CritPercentage);
                            modelWeaponAttacks = 1.0f + 0.73f + 1.4596f;
                        }
                        else
                            goto case AttackModelMode.Basic;
                        break;
                    }
                case AttackModelMode.FullProtection:
                    {
                        // Sword And Board + Shockwave/Concussion Blow Rotation
                        // Requires 3 points in Sword and Board, Shockwave, and Concussion Blow
                        // Shield Slam > Revenge > Devastate @ 3s Shield Slam Cooldown > Concussion Blow > Shockwave > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.133 * Devastate + 0.3266 * (Concussion Blow/Shockwave/Devastate)
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3 && Character.WarriorTalents.ConcussionBlow == 1 && Character.WarriorTalents.Shockwave == 1)
                        {
                            AbilityModel shieldSlam = Abilities[Ability.ShieldSlam];
                            AbilityModel revenge = Abilities[Ability.Revenge];
                            AbilityModel devastate = Abilities[Ability.Devastate];
                            AbilityModel concussionBlow = Abilities[Ability.ConcussionBlow];
                            AbilityModel shockwave = Abilities[Ability.Shockwave];
                            Name        = "Sword And Board + CB/SW";
                            Description = "Shield Slam > Revenge > Devastate\n@ 3s Shield Slam Cooldown: Concussion Blow > Shockwave > Devastate";
                            modelLength = 4.7644f;
                            modelThreat =
                                (1.0f * shieldSlam.Threat) +
                                (0.73f * revenge.Threat) +
                                (1.133f * devastate.Threat) +
                                (0.3266f * ((
                                    concussionBlow.Threat +
                                    shockwave.Threat +
                                    devastate.Threat
                                    ) / 3));
                            modelDamage =
                                (1.0f * shieldSlam.Damage) +
                                (0.73f * revenge.Damage) +
                                (1.133f * devastate.Damage) +
                                (0.3266f * ((
                                    concussionBlow.Damage +
                                    shockwave.Damage +
                                    devastate.Damage
                                    ) / 3));
                            modelHits =
                                (1.0f * shieldSlam.HitPercentage) +
                                (0.73f * revenge.HitPercentage) +
                                (1.133f * devastate.HitPercentage) +
                                (0.3266f * ((
                                    concussionBlow.HitPercentage +
                                    shockwave.HitPercentage +
                                    devastate.HitPercentage
                                    ) / 3));
                            modelCrits =
                                (1.0f * shieldSlam.CritPercentage) +
                                (0.73f * revenge.CritPercentage) +
                                (1.133f * devastate.CritPercentage) +
                                (0.3266f * ((
                                    concussionBlow.CritPercentage +
                                    shockwave.CritPercentage +
                                    devastate.CritPercentage
                                    ) / 3));
                            modelWeaponAttacks = 1.0f + 0.73f + 1.133f + 0.3266f;
                        }
                        else
                            goto case AttackModelMode.Basic;
                        break;
                    }
                case AttackModelMode.UnrelentingAssault:
                    {
                        // Unrelenting Assault 'Protection' Build
                        // Requires 2 points in Unrelenting Assault
                        // Shield Slam -> Revenge -> Revenge -> Revenge
                        if (Character.WarriorTalents.UnrelentingAssault == 2)
                        {
                            Name        = "Unrelenting Assault";
                            Description = "Revenge";
                            modelLength = 1.0f;
                            modelThreat = Abilities[Ability.Revenge].Threat;
                            modelDamage = Abilities[Ability.Revenge].Damage;
                            modelHits   = Abilities[Ability.Revenge].HitPercentage;
                            modelCrits  = Abilities[Ability.Revenge].CritPercentage;
                            modelWeaponAttacks = 1.0f;
                        }
                        else
                            goto case AttackModelMode.Basic;
                        break;
                    }
            }

            // Weapon Swings/Heroic Strike
            float attackerSwings = (modelLength / ParryModel.BossAttackSpeed);
            float attackerHits = DefendTable.AnyHit * attackerSwings;
            float weaponSwings = modelLength / ParryModel.WeaponSpeed;
            float heroicStrikePercentage = Math.Max(0.0f, Math.Min(1.0f, Options.HeroicStrikeFrequency));

            AbilityModel heroicStrike = Abilities[Ability.HeroicStrike];
            modelThreat += heroicStrike.Threat * weaponSwings * heroicStrikePercentage;
            modelDamage += heroicStrike.Damage * weaponSwings * heroicStrikePercentage;
            modelHits   += heroicStrike.HitPercentage * weaponSwings * heroicStrikePercentage;
            modelCrits  += heroicStrike.CritPercentage * weaponSwings * heroicStrikePercentage;

            AbilityModel whiteSwing = Abilities[Ability.None];
            modelThreat += whiteSwing.Threat * weaponSwings * (1.0f - heroicStrikePercentage);
            modelDamage += whiteSwing.Damage * weaponSwings * (1.0f - heroicStrikePercentage);
            modelCrits  += whiteSwing.CritPercentage * weaponSwings * (1.0f - heroicStrikePercentage);
            modelHits   += whiteSwing.HitPercentage * weaponSwings * (1.0f - heroicStrikePercentage);

            modelWeaponAttacks += weaponSwings;

            // Damage Shield
            AbilityModel damageShield = Abilities[Ability.DamageShield];
            modelThreat += damageShield.Threat * attackerHits;
            modelDamage += damageShield.Damage * attackerHits;

            // Deep Wounds
            AbilityModel deepWounds = Abilities[Ability.DeepWounds];
            modelThreat += deepWounds.Threat * modelCrits;
            modelDamage += deepWounds.Damage * modelCrits;

            // Misc. Power Gains
            modelThreat += DefendTable.DodgeParryBlock * (modelLength / ParryModel.BossAttackSpeed) * 25.0f * 
                (Talents.ShieldSpecialization * 0.2f);
            modelThreat += DefendTable.DodgeParryBlock * (modelLength / ParryModel.BossAttackSpeed) * 1.0f * 
                Lookup.StanceThreatMultipler(Character, Stats) * (Talents.ImprovedDefensiveStance * 0.5f);

            // Vigilance, is already calculated as TPS
            if (Options.UseVigilance)
                modelThreat += Abilities[Ability.Vigilance].Threat * modelLength;

            // Final Per-Second Calculations
            ThreatPerSecond             = modelThreat / modelLength;
            DamagePerSecond             = modelDamage / modelLength;
            WeaponAttacksPerSecond      = modelWeaponAttacks / modelLength;
            HitsPerSecond               = modelHits / modelLength;
            CritsPerSecond              = modelCrits / modelLength;
            AttackerSwingsPerSecond     = attackerSwings / modelLength;
            AttackerHitsPerSecond       = attackerHits / modelLength;
        }

        public AttackModel(Character character, Stats stats, CalculationOptionsProtWarr options, AttackModelMode attackModelMode)
            : this(character, stats, options, attackModelMode, RageModelMode.Infinite)
        {
        }

        public AttackModel(Character character, Stats stats, CalculationOptionsProtWarr options, AttackModelMode attackModelMode, RageModelMode rageModelMode)
        {
            Character        = character;
            Stats            = stats;
            Options          = options;
            Talents          = Character.WarriorTalents;
            DefendTable      = new DefendTable(character, stats, options);
            ParryModel       = new ParryModel(character, stats, options);
            _attackModelMode = attackModelMode;
            _rageModelMode   = rageModelMode;

            Abilities.Add(Ability.None, character, stats, options);
            Abilities.Add(Ability.Cleave, character, stats, options);
            Abilities.Add(Ability.ConcussionBlow, character, stats, options);
            Abilities.Add(Ability.DamageShield, character, stats, options);
            Abilities.Add(Ability.DeepWounds, character, stats, options);
            Abilities.Add(Ability.Devastate, character, stats, options);
            Abilities.Add(Ability.HeroicStrike, character, stats, options);
            Abilities.Add(Ability.HeroicThrow, character, stats, options);
            Abilities.Add(Ability.Rend, character, stats, options);
            Abilities.Add(Ability.Revenge, character, stats, options);
            Abilities.Add(Ability.ShieldSlam, character, stats, options);
            Abilities.Add(Ability.Shockwave, character, stats, options);
            Abilities.Add(Ability.Slam, character, stats, options);
            Abilities.Add(Ability.SunderArmor, character, stats, options);
            Abilities.Add(Ability.ThunderClap, character, stats, options);
            Abilities.Add(Ability.Vigilance, character, stats, options);

            Calculate();
        }
    }
}