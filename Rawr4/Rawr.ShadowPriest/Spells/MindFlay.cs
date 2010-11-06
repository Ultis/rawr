using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindFlay : Spell
    {
        /// <summary>
        /// Mind Flay is a dot that lasts 3 seconds.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Dark Evangelism (Maybe), Archangel, Darkness (to be handeled in Char stats), Shadowform, Pain And suffering, Sin and punishment, Harnessed Shadows
        /// Glyphs:
        /// Prime-Mind Flay
        //TODO: Archangel, Harnessed Shadows (Mastery), Sin and Punishment (cooldown of SF), Pain and suffering (refresh of SW:P)
        /// </summary>
        public MindFlay() : base()
        { 
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 221; //check
            baseMaxDamage = 221; //check
            spCoef = 1.5f / 3.5f / 2f; //Check
            dotSpCoef = .1f; //Check
            periodicTick = 556f / 4f; //Check
            periodicTicks = 6f; //Check
            periodicTickTime = 3f; //Check
            baseCastTime = 3f;
            manaCost = 0.22f * Constants.BaseMana;
            shortName = "MF";
        }

        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            totalCoef += .01f + args.Talents.TwinDisciplines;
            totalCoef += .01f + args.Talents.Evangelism;
            totalCoef += .01f + args.Talents.Shadowform;

            if (args.Talents.GlyphofMindFlay)
                periodicTick *= 1.1f; //Add 10% periodic damage

            ApplyDotHaste(args);
            base.Initialize(args);
        }
    }
}
