using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class ThreatModel
    {
        private Character   Character;
        private Stats       Stats;

        private ThreatModelMode _threatModelMode;
        public ThreatModelMode ThreatModelMode
        {
            get { return _threatModelMode; }
            set { _threatModelMode = value; Calculate(); }
        }

        private RageModelMode _rageModelMode;
        public RageModelMode RageModelMode
        {
            get { return _rageModelMode; }
            set { _rageModelMode = value; Calculate(); }
        }

        private float _threatPerSecond = 0.0f;
        public float ThreatPerSecond
        {
            get { return _threatPerSecond; }
            private set { _threatPerSecond = value; }
        }

        private float _damagePerSecond = 0.0f;
        public float DamagePerSecond
        {
            get { return _damagePerSecond; }
            private set { _damagePerSecond = value; }
        }

        private float AbilityThreat(Ability ability)
        {
            // This is just a dummy overload to save some time as Character and Stats are always known in this context
            return Lookup.AbilityThreat(Character, Stats, ability);
        }

        private float AbilityDamage(Ability ability)
        {
            // This is just a dummy overload to save some time as Character and Stats are always known in this context
            return Lookup.AbilityDamage(Character, Stats, ability);
        }

        private float AttackTable(HitResult resultType, Ability ability)
        {
            // This is just a dummy overload to save some time as Character and Stats are always known in this context
            return Lookup.AttackTable(Character, Stats, resultType, ability);
        }

        public void Calculate()
        {
            // Almost everything here requires a weapon+shield, so no reason to bother if none is equipped
            // Can add some fallbacks here later...
            if (Character.MainHand == null || Character.OffHand == null)
                return;

            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;

            switch (ThreatModelMode)
            {
                case ThreatModelMode.Basic:
                    {
                        // Basic Rotation
                        // Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor
                        modelLength = 6.0f;
                        modelThreat =   AbilityThreat(Ability.ShieldSlam) + 
                                        AbilityThreat(Ability.Revenge) + 
                                        AbilityThreat(Ability.SunderArmor) * 2;

                        modelDamage =   AbilityDamage(Ability.ShieldSlam) + 
                                        AbilityDamage(Ability.Revenge) + 
                                        AbilityDamage(Ability.SunderArmor) * 2;

                        modelCrits  =   AttackTable(HitResult.Crit, Ability.ShieldSlam) + 
                                        AttackTable(HitResult.Crit, Ability.Revenge);
                        break;
                    }
                case ThreatModelMode.Devastate:
                    {
                        // Devastate Rotation
                        // Shield Slam -> Revenge -> Devastate -> Devastate
                        if (Character.WarriorTalents.Devastate == 1)
                        {
                            modelLength =   6.0f;
                            modelThreat =   AbilityThreat(Ability.ShieldSlam) + 
                                            AbilityThreat(Ability.Revenge) + 
                                            AbilityThreat(Ability.Devastate) * 2;

                            modelDamage =   AbilityDamage(Ability.ShieldSlam) +
                                            AbilityDamage(Ability.Revenge) +
                                            AbilityDamage(Ability.Devastate) * 2;

                            modelCrits  =   AttackTable(HitResult.Crit, Ability.ShieldSlam) + 
                                            AttackTable(HitResult.Crit, Ability.Revenge) +
                                            AttackTable(HitResult.Crit, Ability.Devastate) * 2;
                        }
                        break;
                    }
                case ThreatModelMode.SwordAndBoard:
                    {
                        // Sword And Board Rotation
                        // Requires 3 points in Sword and Board
                        // Shield Slam > Revenge > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.4596 * Devastate
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3)
                        {
                            modelLength =   4.7644f;
                            modelThreat =   (1.0f * AbilityThreat(Ability.ShieldSlam)) +
                                            (0.73f * AbilityThreat(Ability.Revenge)) +
                                            (1.4596f * AbilityThreat(Ability.Devastate));
                            
                            modelDamage =   (1.0f * AbilityDamage(Ability.ShieldSlam)) +
                                            (0.73f * AbilityDamage(Ability.Revenge)) +
                                            (1.4596f * AbilityDamage(Ability.Devastate));

                            modelCrits =    (1.0f * AttackTable(HitResult.Crit, Ability.ShieldSlam)) +
                                            (0.73f * AttackTable(HitResult.Crit, Ability.Revenge)) +
                                            (1.4596f * AttackTable(HitResult.Crit, Ability.Devastate));
                        }
                        break;
                    }
                case ThreatModelMode.Full:
                    {
                        // Sword And Board + Shockwave/Concussion Blow Rotation
                        // Requires 3 points in Sword and Board, Shockwave, and Concussion Blow
                        // Shield Slam > Revenge > Devastate @ 3s Shield Slam Cooldown > Concussion Blow > Shockwave > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.133 * Devastate + 0.3266 * (Concussion Blow/Shockwave/Devastate)
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3 && Character.WarriorTalents.ConcussionBlow == 1 && Character.WarriorTalents.Shockwave == 1)
                        {
                            // Concussion Blow, Shockwave, and Devastate end up being about equal in the final spot
                            float fillThreat = (AbilityThreat(Ability.ConcussionBlow) + 
                                                AbilityThreat(Ability.Shockwave) + AbilityThreat(Ability.Devastate)) / 3;
                            float fillDamage = (AbilityDamage(Ability.ConcussionBlow) +
                                                AbilityDamage(Ability.Shockwave) + AbilityDamage(Ability.Devastate)) / 3;
                            float fillCrits  = (AttackTable(HitResult.Crit, Ability.ConcussionBlow) +
                                                AttackTable(HitResult.Crit, Ability.Shockwave) + AttackTable(HitResult.Crit, Ability.Devastate)) / 3;

                            modelLength =   4.7644f;
                            modelThreat =   (1.0f * AbilityThreat(Ability.ShieldSlam)) +
                                            (0.73f * AbilityThreat(Ability.Revenge)) +
                                            (1.4596f * AbilityThreat(Ability.Devastate)) +
                                            (0.3266f * fillThreat);

                            modelDamage =   (1.0f * AbilityDamage(Ability.ShieldSlam)) +
                                            (0.73f * AbilityDamage(Ability.Revenge)) +
                                            (1.4596f * AbilityDamage(Ability.Devastate)) +
                                            (0.3266f * fillDamage);

                            modelCrits =    (1.0f * AttackTable(HitResult.Crit, Ability.ShieldSlam)) +
                                            (0.73f * AttackTable(HitResult.Crit, Ability.Revenge)) +
                                            (1.4596f * AttackTable(HitResult.Crit, Ability.Devastate)) +
                                            (0.3266f * fillCrits);
                        }
                        break;
                    }
            }

            // White Damage
            float weaponHits = modelLength / (Math.Max(1.0f, Character.MainHand.Speed / (1.0f + (Stats.HasteRating * ProtWarr.HasteRatingToHaste / 100.0f))));
            if (RageModelMode == RageModelMode.Infinite)
            {
                // Convert all white hits to heroic strikes
                modelThreat += AbilityThreat(Ability.HeroicStrike) * weaponHits;
                modelDamage += AbilityDamage(Ability.HeroicStrike) * weaponHits;
                modelCrits  += AttackTable(HitResult.Crit, Ability.HeroicStrike) * weaponHits;
            }
            else
            {
                // Normal white hits if we aren't using infinite rage, add some logic for a hybrid system later...
                modelThreat += (Lookup.WeaponDPS(Character, Stats) * Lookup.StanceThreatMultipler(Character, Stats) * modelLength);
                modelDamage += Lookup.WeaponDPS(Character, Stats) * modelLength;
                modelCrits  += Lookup.AttackTable(Character, Stats, HitResult.Crit) * weaponHits;
            }

            // Damage Shield, hardcoded at 2.0s attack speed... need to add attack speed as a character option
            float attackerHits = modelLength / 2.0f;
            modelThreat += AbilityThreat(Ability.DamageShield) * attackerHits;
            modelDamage += AbilityDamage(Ability.DamageShield) * attackerHits;
            modelCrits  += AttackTable(HitResult.Crit, Ability.DamageShield) * attackerHits;

            // Deep Wounds
            modelThreat += AbilityThreat(Ability.DeepWounds) * modelCrits;
            modelDamage += (AbilityDamage(Ability.DeepWounds) * modelCrits);

            ThreatPerSecond = modelThreat / modelLength;
            DamagePerSecond = modelDamage / modelLength;
        }

        public ThreatModel(Character character, Stats stats, ThreatModelMode threatModelMode, RageModelMode rageModelMode)
        {
            Character       = character;
            Stats           = stats;
            ThreatModelMode = threatModelMode;
            RageModelMode   = rageModelMode;
            Calculate();
        }
    }
}