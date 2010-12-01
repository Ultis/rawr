
namespace Rawr.RestoSham
{
    public class Spell
    {
        public const float HealingPowerMultiplier = 1.88f;

        public string SpellName { get; set; }
        public int BaseManaCost { get; set; }
        public float BaseCoefficient { get; set; }
        public float Cooldown { get; set; }
    }
    public class Hot : Spell
    {
        public float HotDuration { get; set; }
        public float BaseHotTickFrequency { get; set; }
        public float BaseHotCoefficient { get; set; }
    }
    public sealed class HealingRain : Hot
    {
        // Healing Rain! AoE Hot... yeah... good times modeling that I'm sure
        public float BaseCastTime { get; set; }

        public HealingRain()
        {
            SpellName = "Healing Rain";
            Cooldown = 10f;
            BaseCastTime = 2f;
            BaseHotCoefficient = 0.5f;
        }
    }
    public class HealingSpell : Spell
    {
        // Healing Surge, Healing Wave, Lightning Bolt, etc
        public float BaseCastTime { get; set; }
    }
    public sealed class Riptide : Hot
    {
        public Riptide()
        {
            SpellName = "Riptide";
            Cooldown = 6f;
            BaseHotTickFrequency = 3f;
            BaseCoefficient = 1.5f / 3.5f;
            BaseHotCoefficient = 0.5f;
        }
    }
    public sealed class ChainHeal : HealingSpell
    {
        public bool ChainedHeal { get; set; }

        public ChainHeal()
        {
            SpellName = "ChainHeal";
            Cooldown = 0f;
            BaseCoefficient = 2.5f / 3.5f;
            BaseCastTime = 2.5f;
        }
    }
}
