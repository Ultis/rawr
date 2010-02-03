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

            if (SavedSolutions.ContainsKey(rot)) return SavedSolutions[rot];

            RotationSolution sol = new RotationSolution();
            sol.FightLength = 2000000;
            SimulatorAbility.Delay = rot.Delay;
            SimulatorAbility.Wait = rot.Wait;

            SimulatorAbility[] abilities = new SimulatorAbility[6];

            abilities[(int)Ability.Judgement] = 
                new SimulatorAbility(10 - rot.ImpJudgements - (rot.T7_4pc ? 1 : 0), 1.5f);
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(4, 1.5f);
            abilities[(int)Ability.DivineStorm] = new SimulatorAbility(10, 1.5f);
            abilities[(int)Ability.Consecration] = 
                new SimulatorAbility(rot.GlyphConsecrate ? 10 : 8, rot.SpellGCD);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(15, rot.SpellGCD);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(6, 1.5f);

            abilities[(int)Ability.HammerOfWrath].NextUse = sol.FightLength * (1f - rot.TimeUnder20);

            float gcdFinishTime = 0;
            Random rand = new Random(6021987);
            float nextSwingTime = rot.T10_Speed;

            float currentTime = 0;
            while (currentTime < sol.FightLength)
            {
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

                float nextTime = sol.FightLength;
                if (currentTime >= gcdFinishTime)
                {
                    foreach (SimulatorAbility ability in abilities)
                    {
                        float nextUseTime = ability.GetNextUseTime(currentTime);
                        if (nextUseTime > currentTime)
                            nextTime = Math.Min(nextTime, nextUseTime);
                    }
                }
                else
                {
                    nextTime = Math.Min(nextTime, gcdFinishTime);
                }

                if (rot.T10_Speed > 0)
                {
                    while (nextTime > nextSwingTime)
                    {
                        if (rand.NextDouble() < t10ProcChance)
                        {
                            abilities[(int)Ability.DivineStorm].ResetCooldown(nextSwingTime);
                            nextTime = nextSwingTime;
                        }

                        nextSwingTime += rot.T10_Speed;
                    }
                }

                currentTime = nextTime;
            }

            sol.Judgement = abilities[(int)Ability.Judgement].Uses;
            sol.JudgementCD = abilities[(int)Ability.Judgement].EffectiveCooldown();

            sol.CrusaderStrike = abilities[(int)Ability.CrusaderStrike].Uses;
            sol.CrusaderStrikeCD = abilities[(int)Ability.CrusaderStrike].EffectiveCooldown();

            sol.DivineStorm = abilities[(int)Ability.DivineStorm].Uses;
            sol.DivineStormCD = abilities[(int)Ability.DivineStorm].EffectiveCooldown();

            sol.Consecration = abilities[(int)Ability.Consecration].Uses;
            sol.ConsecrationCD = abilities[(int)Ability.Consecration].EffectiveCooldown();

            sol.Exorcism = abilities[(int)Ability.Exorcism].Uses;
            sol.ExorcismCD = abilities[(int)Ability.Exorcism].EffectiveCooldown();

            sol.HammerOfWrath = abilities[(int)Ability.HammerOfWrath].Uses;
            sol.HammerOfWrathCD = abilities[(int)Ability.HammerOfWrath].EffectiveCooldown();

            SavedSolutions[rot] = sol;
            SaveDictionary(SavedSolutions);

            return sol;
        }
    }
}