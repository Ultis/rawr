using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ShadowPriest.Spells;
using System.Windows.Media;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase
    {
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            /*
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    //Meta
                    int chaotic = 0;

                    // [0] uncommon
                    // [1] perfect uncommon
                    // [2] rare
                    // [3] epic
                    // [4] jewelcrafting

                    //Red
                    //Blue
                    //Yellow
                    //Purple
                    //Green
                    //Orange
                }
            }
             */
        }
    public static class Constants
        {
            // Source: http://bobturkey.wordpress.com/2010/09/28/priest-base-mana-pool-and-mana-regen-coefficient-at-85/
            public static float BaseMana = 20590;
        }
    
}
