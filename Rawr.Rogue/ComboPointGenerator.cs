namespace Rawr.Rogue
{
    public interface IComboPointGenerator
    {
        string Name { get; }
        float EnergyCost { get; }
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
    }

    public class Backstab : IComboPointGenerator
    {
        public string Name { get { return "backstab"; } }
        public float EnergyCost { get { return 60f; } }
    }

    public class Hemo : IComboPointGenerator
    {
        public string Name { get { return "hemo"; } }
        public float EnergyCost { get { return 35; } }
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
    }
}
