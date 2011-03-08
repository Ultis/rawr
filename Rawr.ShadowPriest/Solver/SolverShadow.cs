using System;
using System.Collections.Generic;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
}
        /*
        Stats stats1;
        PriestTalents talents;

        CombatFactors combatFactors = new CombatFactors( talents, stats, 0, .1f, .1f);

        SpellBox spellbox = new SpellBox(combatFactors);
        private void spellPriority()
        {
            foreach (Spell s in spellbox.Spells)
            {
                switch (s.type)
                {
                    case "dot":
                        Dot dot = s as Dot;
                        if (dot.Duration < dot.Duration - dot.TickPeriod - dot.CastTime)
                        {
                            s.priority++;
                        }
                        break;
                    case "MF":
                        s.priority = 1;
                        break;

                }
            }
        }

        Spell nextSpell;
        private void spellToCast()
        {
            nextSpell = spellbox.Wait;
            foreach (Spell s in spellbox.Spells)
            {
                if (s.priority > nextSpell.priority) nextSpell = s;
            }
        }

        List<Spell> rotation;
        private void buildRotation()
        {
            rotation.Add(nextSpell);
            switch (nextSpell.type)
            {
                case "dot":
                    Dot dot = nextSpell as Dot;
                    break;
                case "MF":
                    break;

            }

        } 
         */
