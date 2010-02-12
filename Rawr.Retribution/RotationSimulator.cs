using System;
using System.Collections.Generic;
using System.Text;
#if RAWR3

#else
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
#endif

namespace Rawr.Retribution
{
    static class RotationSimulator
    {
        private static SerializableDictionary<RotationParameters, RotationSolution> savedSolutions = null;
        private static SerializableDictionary<RotationParameters, RotationSolution> SavedSolutions
        {
            get
            {
                if (savedSolutions == null) savedSolutions = LoadDictionary();
                return savedSolutions;
            }
        }

#if !RAWR3
        private static string SaveFilePath()
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                "Data" + System.IO.Path.DirectorySeparatorChar + "RetRotations.xml");
        }
#endif

        private static SerializableDictionary<RotationParameters, RotationSolution> LoadDictionary()
        {
            SerializableDictionary<RotationParameters, RotationSolution> sols = null;
#if RAWR3

#else
            string path = SaveFilePath();
            if (File.Exists(path))
            {
                try
                {
                    using (TextReader reader = new StreamReader(path, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<RotationParameters, RotationSolution>));
                        sols = (SerializableDictionary<RotationParameters, RotationSolution>)serializer.Deserialize(reader);
                    }

                }
                catch (Exception)
                {
#if DEBUG
                    MessageBox.Show(":(");
#endif
                }
            }
#endif
            if (sols == null) sols = new SerializableDictionary<RotationParameters, RotationSolution>();
            return sols;
        }

        private static void SaveDictionary(SerializableDictionary<RotationParameters, RotationSolution> sols)
        {
#if RAWR3
 
#else
            lock (sols)
            {
                try
                {
                    using (TextWriter writer = new StreamWriter(SaveFilePath(), false, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<RotationParameters, RotationSolution>));
                        serializer.Serialize(writer, sols);
                    }
                }
                catch (Exception) { }
            }
#endif
        }

        public static void ClearCache()
        {
            savedSolutions.Clear();
        }

        public static RotationSolution SimulateRotation(RotationParameters rot)
        {
            const float t10ProcChance = 0.4f;
            const int timeUnitsPerSecond = 100;
            const int fightLength = 2000000 * timeUnitsPerSecond;
            const int meleeAbilityGcd = (int)(1.5m * timeUnitsPerSecond);

            if (SavedSolutions.ContainsKey(rot)) return SavedSolutions[rot];

            int bloodlustSpellGcd = (int)(rot.BloodlustSpellGCD * timeUnitsPerSecond);
            int spellGcd = (int)(rot.SpellGCD * timeUnitsPerSecond);
            int bloodlustT10Speed = (int)(rot.BloodlustT10Speed * timeUnitsPerSecond);
            int t10Speed = (int)(rot.T10_Speed * timeUnitsPerSecond);
            SimulatorAbility.Delay = (int)(rot.Delay * timeUnitsPerSecond);
            SimulatorAbility.Wait = (int)(rot.Wait * timeUnitsPerSecond);

            SimulatorAbility[] abilities = new SimulatorAbility[6];

            abilities[(int)Ability.Judgement] = new SimulatorAbility(
                (10 - rot.ImpJudgements - (rot.T7_4pc ? 1 : 0)) * timeUnitsPerSecond, 
                meleeAbilityGcd);
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(
                4 * timeUnitsPerSecond, 
                meleeAbilityGcd);
            abilities[(int)Ability.DivineStorm] = new SimulatorAbility(
                10 * timeUnitsPerSecond, 
                meleeAbilityGcd);
            abilities[(int)Ability.Consecration] = new SimulatorAbility(
                (rot.GlyphConsecrate ? 10 : 8) * timeUnitsPerSecond, 
                bloodlustSpellGcd);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(
                15 * timeUnitsPerSecond, 
                bloodlustSpellGcd);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(
                6 * timeUnitsPerSecond, 
                meleeAbilityGcd);

            abilities[(int)Ability.HammerOfWrath].NextUse = 
                (int)Math.Round((double)fightLength * (1d - rot.TimeUnder20));

            int gcdFinishTime = 0;
            Random rand = new Random(6021987);
            bool isBloodlustActive = true;
            int bloodlustFinishTime = (int)Math.Round((double)fightLength * rot.BloodlustUptime);
            int nextSwingTime = bloodlustFinishTime > 0 ? bloodlustT10Speed : t10Speed;

            int currentTime = 0;
            while (currentTime < fightLength)
            {
                if (isBloodlustActive && (currentTime >= bloodlustFinishTime))
                {
                    isBloodlustActive = false;
                    abilities[(int)Ability.Consecration].GlobalCooldown = spellGcd;
                    abilities[(int)Ability.Exorcism].GlobalCooldown = spellGcd;
                }

                if (currentTime >= gcdFinishTime)
                {
                    foreach (Ability ability in rot.Priorities)
                    {
                        if (abilities[(int)ability].ShouldAbilityBeUsedNext(currentTime))
                        {
                            if (abilities[(int)ability].CanAbilityBeUsedNow(currentTime))
                                gcdFinishTime = abilities[(int)ability].UseAbility(currentTime);

                            break;
                        }
                    }
                }

                int nextTime = fightLength;
                if (currentTime >= gcdFinishTime)
                {
                    foreach (SimulatorAbility ability in abilities)
                    {
                        int nextUseTime = ability.GetNextUseTime(currentTime);
                        if (nextUseTime > currentTime)
                            nextTime = Math.Min(nextTime, nextUseTime);
                    }
                }
                else
                {
                    nextTime = Math.Min(nextTime, gcdFinishTime);
                }

                if (t10Speed > 0)
                {
                    if (isBloodlustActive)
                        nextTime = Math.Min(nextTime, bloodlustFinishTime);

                    while (nextTime > nextSwingTime)
                    {
                        if (rand.NextDouble() < t10ProcChance)
                        {
                            abilities[(int)Ability.DivineStorm].ResetCooldown(nextSwingTime);
                            nextTime = nextSwingTime;
                        }

                        nextSwingTime += isBloodlustActive ? bloodlustT10Speed : t10Speed;
                    }
                }

                currentTime = nextTime;
            }

            RotationSolution sol = new RotationSolution();
            sol.FightLength = ((float)fightLength) / timeUnitsPerSecond;
            sol.Judgement = abilities[(int)Ability.Judgement].Uses;
            sol.JudgementCD = abilities[(int)Ability.Judgement].EffectiveCooldown() / timeUnitsPerSecond;

            sol.CrusaderStrike = abilities[(int)Ability.CrusaderStrike].Uses;
            sol.CrusaderStrikeCD = 
                abilities[(int)Ability.CrusaderStrike].EffectiveCooldown() / timeUnitsPerSecond;

            sol.DivineStorm = abilities[(int)Ability.DivineStorm].Uses;
            sol.DivineStormCD = abilities[(int)Ability.DivineStorm].EffectiveCooldown() / timeUnitsPerSecond;

            sol.Consecration = abilities[(int)Ability.Consecration].Uses;
            sol.ConsecrationCD = abilities[(int)Ability.Consecration].EffectiveCooldown() / timeUnitsPerSecond;

            sol.Exorcism = abilities[(int)Ability.Exorcism].Uses;
            sol.ExorcismCD = abilities[(int)Ability.Exorcism].EffectiveCooldown() / timeUnitsPerSecond;

            sol.HammerOfWrath = abilities[(int)Ability.HammerOfWrath].Uses;
            sol.HammerOfWrathCD = 
                abilities[(int)Ability.HammerOfWrath].EffectiveCooldown() / timeUnitsPerSecond;

            SavedSolutions[rot] = sol;
            SaveDictionary(SavedSolutions);

            return sol;
        }
    }
}