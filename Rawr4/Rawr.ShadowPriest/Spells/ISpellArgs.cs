using System;

namespace Rawr.ShadowPriest.Spells
{
    public interface ISpellArgs
    {
        PriestTalents Talents { get; }
        Stats Stats { get; }
        float LatencyCast { get; }
        float LatencyGCD { get; }
        int AdditionalTargets { get; }
    }
}
