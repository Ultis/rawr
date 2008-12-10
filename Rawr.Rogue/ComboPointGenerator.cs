namespace Rawr.Rogue
{
    public interface IComboPointGenerator 
    {
        string Name { get; }
        float EnergyCost { get; }
        CpgAttackValues CalcAttackValues(Character character, Stats stats, CombatFactors combatFactors);
    }

    public class CpgAttackValues
    {
        public float AttackDamage;
        public float BonusCrit;
        public float BonusDamageMultiplier = 1f;
        public float BonusCritDamageMultiplier = 2f;
    }

    public static class ComboPointGenerator
    {
        public static IComboPointGenerator Get(Character character)
        {
            // if we have mutilate and we're using two daggers, assume we use it to generate CPs
            if (character.RogueTalents.Mutilate > 0 &&
                character.MainHand != null && character.MainHand.Type == Item.ItemType.Dagger &&
                character.OffHand != null && character.OffHand.Type == Item.ItemType.Dagger)
            {
                return new Mutilate();
            }
            // if we're main handing a dagger, assume we're using backstab it to generate CPs
            if (character.MainHand != null && character.MainHand.Type == Item.ItemType.Dagger)
            {
                return new Backstab();
            }
            // if we have hemo, assume we use it to generate CPs
            if (character.RogueTalents.Hemorrhage > 0)
            {
                return new Hemo();
            }

            // otherwise use sinister strike
            return new SinisterStrike(character);
        }
    }

    public class Mutilate : IComboPointGenerator
    {
        public string Name { get { return "mutilate"; } }
        public float EnergyCost{ get { return 60f; } }
        public CpgAttackValues CalcAttackValues(Character character, Stats stats, CombatFactors combatFactors)
        {
            //TODO:  Figure out why stats.WeaponDamage was not included in the Mutilate Calculation

            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f + 121.5f;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage += (character.OffHand.MinDamage + character.OffHand.MaxDamage) / 2f + 121.5f;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage *= 1.5f;

            attackValues.BonusCrit += 5f * character.RogueTalents.PuncturingWounds;
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * character.RogueTalents.Lethality);
            attackValues.BonusDamageMultiplier *= (1f + 0.04f * character.RogueTalents.Opportunity);
            return attackValues;
        }
    }

    public class Backstab : IComboPointGenerator
    {
        public string Name { get { return "backstab"; } }
        public float EnergyCost { get { return 60f; } }
        public CpgAttackValues CalcAttackValues(Character character, Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 1.7f;
            attackValues.AttackDamage *= 1.5f;
            attackValues.AttackDamage += 255f;

            attackValues.BonusDamageMultiplier *= (1f + .02f * character.RogueTalents.Aggression);
            attackValues.BonusDamageMultiplier *= (1f + .1f * character.RogueTalents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + 0.04f * character.RogueTalents.Opportunity);
            attackValues.BonusCrit += 10f * character.RogueTalents.PuncturingWounds;
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * character.RogueTalents.Lethality);

            return attackValues;
        }
    }

    public class Hemo : IComboPointGenerator
    {
        public string Name { get { return "hemo"; } }
        public float EnergyCost { get { return 35; } }
        public CpgAttackValues CalcAttackValues(Character character, Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 2.4f;
            attackValues.AttackDamage *= 1.1f;

            attackValues.BonusDamageMultiplier *= (1f + .1f * character.RogueTalents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + stats.BonusCPGDamage);
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * character.RogueTalents.Lethality);
            return attackValues;
        }
    }

    public class SinisterStrike : IComboPointGenerator
    {
        public SinisterStrike(Character character)
        {
            _character = character;
        }

        private readonly Character _character;
        
        public string Name { get { return "ss"; } }
        public float EnergyCost
        {
            get
            {
                switch (_character.RogueTalents.ImprovedSinisterStrike)
                {
                    case 2: return 40f;
                    case 1: return 42f;
                    default: return 45f;
                }
            }
        }

        public CpgAttackValues CalcAttackValues(Character character, Stats stats, CombatFactors combatFactors)
        {
            var attackValues = new CpgAttackValues();
            attackValues.AttackDamage = combatFactors.AvgMhWeaponDmg;
            attackValues.AttackDamage += stats.AttackPower / 14f * 2.4f;
            attackValues.AttackDamage += 98f;

            attackValues.BonusDamageMultiplier *= (1f + .02f * character.RogueTalents.Aggression);
            attackValues.BonusDamageMultiplier *= (1f + .1f * character.RogueTalents.SurpriseAttacks);
            attackValues.BonusDamageMultiplier *= (1f + stats.BonusCPGDamage);
            attackValues.BonusCritDamageMultiplier *= (1f + .06f * character.RogueTalents.Lethality);
            return attackValues;
        }
    }
}
