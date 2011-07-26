namespace Rawr.Retribution
{
    public class StatsRetri : Stats
    {
        /// <summary>
        /// Increases the damage done by your Templar's Verdict ability by 10%.
        /// </summary>
        public bool T11_2P { get; set; }
        /// <summary>
        /// Your Inquisition ability's duration is calculated as if you had one additional Holy Power.
        /// </summary>
        public bool T11_4P { get; set; }
        /// <summary>
        ///  Your Crusader Strike deals 15% additional damage as Fire damage over 4 sec.
        /// </summary>
        public bool T12_2P { get; set; }
        /// <summary>
        /// Increases the duration of your Zealotry ability by 15 sec.
        /// </summary>
        public bool T12_4P { get; set; }
        /// <summary>
        /// +400 resilience rating
        /// Increases Strength by 70
        /// </summary>
        public bool PVP_2P { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Judgements by 1 sec.
        /// Increases Strength by 90.
        /// </summary>
        public bool PVP_4P { get; set; }

        public void SetSets(Character character)
        {
            int TCount;
            // Gladiator
            character.SetBonusCount.TryGetValue("Gladiator's Vindication", out TCount);
            if (TCount >= 2) { PVP_2P = true; Resilience += 400; Strength += 70; }
            if (TCount >= 4) { PVP_4P = true; JudgementCDReduction = 1; Strength += 90; }

            //T11
            character.SetBonusCount.TryGetValue("Reinforced Sapphirium Battleplate", out TCount);
            if (TCount >= 2) { T11_2P = true; }
            if (TCount >= 4) { T11_4P = true; }

            //T12
            character.SetBonusCount.TryGetValue("Battleplate of Immolation", out TCount);
            if (TCount >= 2) { T12_2P = true; }
            if (TCount >= 4) { T12_4P = true; }
        }

        public new StatsRetri Clone()
        {
            StatsRetri clone = new StatsRetri();
            clone.Accumulate(base.Clone());
            clone.T11_2P = T11_2P;
            clone.T11_4P = T11_4P;
            clone.T12_2P = T12_2P;
            clone.T12_4P = T12_4P;
            return clone;
        }

        public void Accumulate(StatsRetri data)
        {
            base.Accumulate(data);
            T11_2P = data.T11_2P || T11_2P;
            T11_4P = data.T11_4P || T11_4P;
            T12_2P = data.T12_2P || T12_2P;
            T12_4P = data.T12_4P || T12_4P;
        }
    }
}
