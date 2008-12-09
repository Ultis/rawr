namespace Rawr.Rogue
{
    public interface IFinisher
    {
        float EnergyCost { get; }
    }
    public static class Finishers
    {
        public static IFinisher Get(CalculationOptionsRogue calcOpts)
        {
            if (calcOpts.DPSCycle['r'] > 0)
            {
                return new Rupture();
            }
            
            if (calcOpts.DPSCycle['e'] > 0)
            {
                new Evis();
            }

            return new NoFinisher();
        }
    }

    public class Rupture : IFinisher
    {
        public float EnergyCost { get { return 25f; } }
    }

    public class Evis : IFinisher
    {
        public float EnergyCost { get { return 35f; } }
    }

    public class NoFinisher : IFinisher
    {
        public float EnergyCost { get { return 0f; } }
    }
}
