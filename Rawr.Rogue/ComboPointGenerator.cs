namespace Rawr.Rogue
{
    public interface IComboPointGenerator 
    {
        string Name { get; }
        float EnergyCost { get; }
        CpgAttackValues CalcAttackValues(Stats stats, CombatFactors combatFactors);
    }

    public class CpgAttackValues
    {
        public float AttackDamage;
        public float Crit;
        public float BonusDamageMultiplier = 1f;
        public float BonusCritDamageMultiplier = 2f;
    }

    public static class ComboPointGenerator
    {
        public static IComboPointGenerator Get(RogueTalents talents, CombatFactors combatFactors)
        {
            // if we have mutilate and we're using two daggers, assume we use it to generate CPs
            if (talents.Mutilate > 0 &&
                combatFactors.MainHand.Type == Item.ItemType.Dagger &&
                combatFactors.OffHand.Type == Item.ItemType.Dagger)
            {
                return new Mutilate(talents);
            }

            // if we're main handing a dagger, assume we're using backstab it to generate CPs
            if (combatFactors.MainHand.Type == Item.ItemType.Dagger)
            {
                return new Backstab(talents);
            }

            // if we have hemo, assume we use it to generate CPs
            if (talents.Hemorrhage > 0)
            {
                return new Hemo(talents);
            }

            // otherwise use sinister strike
            return new SinisterStrike(talents);
        }
    }

    public class Mutilate : IComboPointGenerator
    {
        public Mutilate(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;

        public string Name { get { return "mutilate"; } }
        public float EnergyCost{ get { return 60f; } }
        public CpgAttackValues CalcAttackValues(Stats stats, CombatFactors combatFactors)
        {
            //TODO:  Figure out why stats.WeaponDamage was not included in the Mutilate Calculation

            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = (combatFactors.MainHand.MinDamage + combatFactors.MainHand.MaxDamage) / 2f + 121.5f;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage += (combatFactors.OffHand.MinDamage + combatFactors.OffHand.MaxDamage) / 2f + 121.5f;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage *= 1.5f;

            attackValues.Crit += combatFactors.ProbMhCrit + 0.05f * _talents.PuncturingWounds;
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * _talents.Lethality);
            attackValues.BonusDamageMultiplier *= (1f + 0.04f * _talents.Opportunity);
            return attackValues;
        }
    }

    public class Backstab : IComboPointGenerator
    {
        public Backstab(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;

        public string Name { get { return "backstab"; } }
        public float EnergyCost { get { return 60f; } }
        public CpgAttackValues CalcAttackValues(Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage *= 1.5f;
            attackValues.AttackDamage += 255f;

            attackValues.BonusDamageMultiplier *= (1f + .02f * _talents.Aggression);
            attackValues.BonusDamageMultiplier *= (1f + .1f * _talents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + 0.04f * _talents.Opportunity);
            attackValues.Crit += combatFactors.ProbMhCrit + .1f * _talents.PuncturingWounds;
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * _talents.Lethality);

            return attackValues;
        }
    }

    public class Hemo : IComboPointGenerator
    {
        public Hemo(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;

        public string Name { get { return "hemo"; } }
        public float EnergyCost { get { return 35; } }
        public CpgAttackValues CalcAttackValues(Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 2.4f;
            attackValues.AttackDamage *= 1.1f;

            attackValues.Crit += combatFactors.ProbMhCrit;
            attackValues.BonusDamageMultiplier *= (1f + .1f * _talents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + stats.BonusCPGDamage);
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * _talents.Lethality);
            return attackValues;
        }
    }

    public class SinisterStrike : IComboPointGenerator
    {
        public SinisterStrike(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;
        
        public string Name { get { return "ss"; } }
        public float EnergyCost
        {
            get
            {
                switch (_talents.ImprovedSinisterStrike)
                {
                    case 2: return 40f;
                    case 1: return 42f;
                    default: return 45f;
                }
            }
        }

        public CpgAttackValues CalcAttackValues(Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 2.4f;
            attackValues.AttackDamage += 98f;

            attackValues.Crit = combatFactors.ProbMhCrit;
            attackValues.BonusDamageMultiplier *= (1f + .02f * _talents.Aggression);
            attackValues.BonusDamageMultiplier *= (1f + .1f * _talents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + stats.BonusCPGDamage);
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * _talents.Lethality);
            return attackValues;
        }
    }
}
