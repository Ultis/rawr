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
            if (SavedSolutions.ContainsKey(rot)) return SavedSolutions[rot];

            RotationSolution sol = new RotationSolution();
            float currentTime = 0;
            sol.FightLength = 2000000;
            SimulatorAbility.Delay = rot.Delay;
            SimulatorAbility.Wait = rot.Wait;

            SimulatorAbility[] abilities = new SimulatorAbility[6];

            abilities[(int)Ability.Judgement] = new SimulatorAbility(10 - rot.ImpJudgements - (rot.T7_4pc ? 1 : 0));
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(4);
            abilities[(int)Ability.DivineStorm] = new SimulatorAbility(10);
            abilities[(int)Ability.Consecration] = new SimulatorAbility(rot.GlyphConsecrate ? 10 : 8);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(15);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(6);

            abilities[(int)Ability.HammerOfWrath].NextUse = sol.FightLength * (1f - rot.TimeUnder20);

            bool gcdUsed;
            float minNext, tryUse, timeElapsed = 0;
            Random rand = new Random(6021987);

            while (currentTime < sol.FightLength)
            {
                gcdUsed = false;
                foreach (Ability ability in rot.Priorities)
                {
                    tryUse = abilities[(int)ability].UseAbility(currentTime);
                    if (tryUse > 0)
                    {
                        timeElapsed = tryUse - currentTime;
                        currentTime = tryUse;
                        gcdUsed = true;
                        break;
                    }
                }
                if (!gcdUsed)
                {
                    minNext = sol.FightLength;
                    foreach (Ability ab in rot.Priorities)
                    {
                        if (abilities[(int)ab].NextUse < minNext) minNext = abilities[(int)ab].NextUse;
                    }
                    timeElapsed = minNext - currentTime;
                    currentTime = minNext;
                }
                if (rot.T10_Speed > 0)
                {
                    if (rand.NextDouble() < (0.4 * timeElapsed / rot.T10_Speed)) abilities[(int)Ability.DivineStorm].ResetCD();
                }
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