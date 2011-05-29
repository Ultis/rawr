using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{

    public class CharacterCalculationsHealadin : CharacterCalculationsBase
    {

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f , 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float FightPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float BurstPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float AvgHPS { get; set; }
        public float AvgHPM { get; set; }
        public float TotalHealed { get; set; }
        public float TotalMana { get; set; }

        public float FightLength { get; set; }
        public float ActiveTime { get; set; }

        public FlashOfLight FoL { get; set; }
        public HolyLight HL { get; set; }
        public HolyShock HS { get; set; }
        public BeaconOfLight BoL { get; set; }
        public JudgementsOfThePure JotP { get; set; }
        
        public DivineLight DL { get; set; }    
        public WordofGlory WoG { get; set; }    
        public LightofDawn LoD { get; set; }
        public HolyRadiance HR { get; set; }
        public LayonHands LoH { get; set; }
        public ProtectoroftheInnocent PotI { get; set; }
        public EnlightenedJudgements EJ { get; set; }

        public float SpellPowerTotal { get; set; }  // add spellpower from items + int to get this
        public float HasteJotP { get; set; } // Haste from Judgement of the Pure talent (just used to print on stats screen)
        public float HasteSoL { get; set; }  // Haste from Speed of Light talent (just used to print on stats screen)

        // These track total cast time for each spell
        public float RotationFoL { get; set; }  
        public float RotationHL { get; set; }
        public float RotationDL { get; set; }
        public float RotationHS { get; set; }
        // public float RotationJotP { get; set; }   I want to remove this and replace with RotationJudge
        public float RotationJudge { get; set; }
        public float RotationBoL { get; set; }
        public float RotationWoG { get; set; }
        public float RotationLoD { get; set; }
        public float RotationHR { get; set; }
        public float RotationLoH { get; set; }
        public float RotationCleanse { get; set; }
        public float RotationMelee { get; set; }  // Time spend melee, NOT during instant casts. Just doing melee for mana.
        public float RotationTotal { get; set; }
        public float HolyPowerCasts { get; set; }

        // total healed for each spell
        public float HealedFoL { get; set; }
        public float HealedHL { get; set; }
        public float HealedDL { get; set; }
        public float HealedHS { get; set; }
        public float HealedWoG { get; set; }
        public float HealedGHL { get; set; }
        public float HealedBoL { get; set; }
        public float HealedJudge { get; set; }
        public float HealedPotI { get; set; }
        public float HealedLoD { get; set; }
        public float HealedHR { get; set; }
        public float HealedLoH { get; set; }
        public float HealedIH { get; set; }
        public float HealedOther { get; set; }

        // These track total mana used for each spell
        public float UsageFoL { get; set; }  
        public float UsageHL { get; set; }
        public float UsageDL { get; set; }
        public float UsageHS { get; set; }
        public float UsageWoG { get; set; }
        public float UsageBoL { get; set; }
        public float UsageJudge { get; set; }
        public float UsageLoD { get; set; }
        public float UsageHR { get; set; }
        public float UsageLoH { get; set; }
        public float UsageCleanse { get; set; }
        public float UsageTotal { get; set; }

        // number of casts in fight
        public float FoLCasts { get; set; }
        public float WoGCasts { get; set; }
        public float LoDCasts { get; set; }
        public float HLCasts { get; set; }
        public float DLCasts { get; set; }
        public float HRCasts { get; set; }
        public float HSCasts { get; set; }
        public float BoLCasts { get; set; }
        public float JudgeCasts { get; set; }
        public float PotICasts { get; set; }
        public float LoHCasts { get; set; }
        public float CleanseCasts { get; set; }
        public float MeleeSwings { get; set; }
        public float MeleeProcs { get; set; }

        public float ManaBase { get; set; }
        public float ManaMp5 { get; set; }
        public float ManaReplenishment { get; set; }
        public float ManaArcaneTorrent { get; set; }
        public float ManaDivinePlea { get; set; }
        public float ManaLayOnHands { get; set; }
        public float ManaOther { get; set; }
        public float ManaJudgements { get; set; }   // mana generated from casting Judgements (15% base mana per cast)
        public float ManaRegenRate { get; set; }    // out of combat regen rate
        public float CombatRegenRate { get; set; }  // in combat total regen rate
        public float ManaMelee { get; set; }
        public float CombatRegenTotal { get; set; }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Status", string.Format("Overall: {0,-10} Fight: {1,-10} Burst: {2,-10}",
                OverallPoints.ToString("N0"),
                FightPoints.ToString("N0"),
                BurstPoints.ToString("N0")));



            //Basic Stats
            dictValues["Health"] = BasicStats.Health.ToString("N00");
            dictValues["Stamina"] = BasicStats.Stamina.ToString("N00");
            dictValues["Mana"] = BasicStats.Mana.ToString("N00");
            dictValues["Intellect"] = BasicStats.Intellect.ToString("N00");
            dictValues["Spirit"] = BasicStats.Spirit.ToString("N00");
            dictValues["Mastery Rating"] = string.Format("{0}*{1} Mastery Rating", 
                                   (8 + BasicStats.MasteryRating / 179.28f).ToString("N02"), BasicStats.MasteryRating);
            dictValues["Spell Power"] = string.Format("{0}", SpellPowerTotal.ToString("N00"));
            dictValues["Mana Regen"] = string.Format("{0}", ManaRegenRate.ToString("N00"));
            dictValues["Combat Regen"] = string.Format("{0}", CombatRegenRate.ToString("N00"));  
            // dictValues["Mp5"] = BasicStats.Mp5.ToString("N00");
            dictValues["Spell Crit"] = string.Format("{0}%*{1} Crit Rating", (BasicStats.SpellCrit * 100).ToString("N02"), BasicStats.CritRating);
            dictValues["Spell Haste"] = string.Format("{0}%*Haste breakdown:\nHaste Rating {1} gives {2} haste.\nTalent haste multipliers:\nSpeed of Light: {3}\nJudgements of the Pure: {4}\nOther Haste: {5} (probably from buffs)",
                                      (BasicStats.SpellHaste * 100).ToString("N02"), BasicStats.HasteRating,
                                      (BasicStats.RangedHaste * 100).ToString("N02"), HasteSoL.ToString("N02"), HasteJotP.ToString("N02"), 
                                      (((1f + BasicStats.SpellHaste) / (1f + BasicStats.PhysicalHaste) -1f) * 100f).ToString("N02"));

            // Cycle Stats
            dictValues["Total Healed"] = string.Format("{0} healing", TotalHealed.ToString("N00"));
            dictValues["Total Mana"] = string.Format("{0} mana *Mana Sources:\nBase Mana: {1} \nCombat Regen: {2}\nJudgements: {3}\nReplenishment: {4}\nDivine Plea: {5}\nArcane Torrent: {6}\nLay on Hands: {7}\nMP5: {8}\nMelee: {9}\nOther: {10}", 
                                   TotalMana.ToString("N00"), ManaBase.ToString("N00"), CombatRegenTotal.ToString("N00"), ManaJudgements.ToString("N00"),
                                   ManaReplenishment.ToString("N00"), ManaDivinePlea.ToString("N00"), ManaArcaneTorrent.ToString("N00"), 
                                   ManaLayOnHands.ToString("N00"), ManaMp5.ToString("N00"), ManaMelee.ToString("N00"), ManaOther.ToString("N00"));

            dictValues["Average Healing per sec"] = string.Format("{0} hps", AvgHPS.ToString("N00"));
            dictValues["Average Healing per mana"] = string.Format("{0} hpm", AvgHPM.ToString("N02"));
            dictValues["Fight Length"] = string.Format("{0} sec *{1} Active Time \n{2} Total Cast Time", 
                        FightLength.ToString("N00"), ActiveTime.ToString("N00"), RotationTotal.ToString("N00"));

         
            // Healing Breakdown
            dictValues["Holy Light Healed"] = string.Format("{0} *Holy Light Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedHL.ToString("N00"), HLCasts.ToString("N01"), RotationHL.ToString("N02"), UsageHL.ToString("N00"));
            dictValues["Divine Light Healed"] = string.Format("{0} *Divine Light Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedDL.ToString("N00"), DLCasts.ToString("N01"), RotationDL.ToString("N02"), UsageDL.ToString("N00"));
            dictValues["Flash of Light Healed"] = string.Format("{0} *Flash of Light Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedFoL.ToString("N00"), FoLCasts.ToString("N01"), RotationFoL.ToString("N02"), UsageFoL.ToString("N00"));
            HSCasts = HS.Casts(); // I wasn't sure how to print this directly, so I made this variable to get it to work for now
            dictValues["Holy Shock Healed"] = string.Format("{0} *Holy Shock Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedHS.ToString("N00"), HSCasts.ToString("N00"), RotationHS.ToString("N02"), UsageHS.ToString("N00"));
            dictValues["Word of Glory Healed"] = string.Format("{0} *Word of Glory Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedWoG.ToString("N00"), WoGCasts.ToString("N00"), RotationWoG.ToString("N02"), UsageWoG.ToString("N00"));
            dictValues["Light of Dawn Healed"] = string.Format("{0} *Light of Dawn Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedLoD.ToString("N00"), LoDCasts.ToString("N00"), RotationLoD.ToString("N02"), UsageLoD.ToString("N00"));
            dictValues["Holy Radiance Healed"] = string.Format("{0} *Holy Radiance Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedHR.ToString("N00"), HRCasts.ToString("N00"), RotationHR.ToString("N02"), UsageHR.ToString("N00"));
            BoLCasts = BoL.Casts();  // I wasn't sure how to print this directly, so I made this variable to get it to work for now
            dictValues["Beacon of Light Healed"] = string.Format("{0} *Beacon of Light Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedBoL.ToString("N00"), BoLCasts.ToString("N00"), RotationBoL.ToString("N02"), UsageBoL.ToString("N00"));
            LoHCasts = LoH.Casts();
            dictValues["Lay on Hands Healed"] = string.Format("{0} *Lay on Hands Information: \nNumber of Casts: {1}\nTotal cast time: {2} sec\nTotal mana used: {3}",
                                                            HealedLoH.ToString("N00"), LoHCasts.ToString("N00"), RotationLoH.ToString("N02"), UsageLoH.ToString("N00"));
            //dictValues["Glyph of HL Healed"] = string.Format("{0} ", HealedGHL.ToString("N00")); take this out, seems like this glyph no longer exists
            dictValues["Protector of the Innocent "] = string.Format("{0} *Protector of the Innocent Info:\nNumber of Casts: {1}",
                                                            HealedPotI.ToString("N00"), PotICasts.ToString("N00"));
            dictValues["Enlightened Judgements "] = string.Format("{0} *Enlightened Judgements Info:\nNumber of Casts: {1}", 
                                                            HealedJudge.ToString("N00"), JudgeCasts.ToString("N00"));
            dictValues["Illuminated Healing"] = string.Format("{0} *These are from the absorb shields, and increase with Mastery stat.\nThe shields often expire or are overwritten.\nSee options tab to adjust the effective %.",
                                                            HealedIH.ToString("N00"));
            dictValues["Cleanse casting"] = string.Format("*Based on the settings from the options tab, you cast Cleanse {0} times, using up {1} mana.",
                                                            CleanseCasts.ToString("N00"), UsageCleanse.ToString("N00"));
            dictValues["Other Healed"] = string.Format("{0} ", HealedOther.ToString("N00"));

            //Spell Details
            dictValues["Holy Light"] = "*" + HL.ToString();
            dictValues["Flash of Light"] = "*" + FoL.ToString();
            dictValues["Holy Shock"] = "*" + HS.ToString();
            dictValues["Divine Light"] = "*" + DL.ToString();  
            dictValues["Word of Glory(3 holy power)"] = "*" + WoG.ToString();
            dictValues["LoD(3 hp, max targets)"] = "*" + LoD.ToString();
            dictValues["Holy Radiance (max)"] = "*" + HR.ToString();
            dictValues["Lay on Hands"] = "*" + LoH.ToString();
            dictValues["Protector of the Innocent"] = "*" + PotI.ToString();
            dictValues["Enlightened Judgements"] = "*" + EJ.ToString();  

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Holy Light Cast Time": return HL.CastTime();
                case "Holy Light HPS": return HL.HPS();
                case "Holy Light Time": return RotationHL;
                case "Flash of Light Cast Time": return FoL.CastTime();
                case "Flash of Light HPS": return FoL.HPS();
                case "Flash of Light Time": return RotationFoL;
            }
            return 0f;
        }
    }
}
