using System;
using System.Diagnostics;

namespace Rawr.Warlock
{
    /// <summary>
    /// A generic class for modelling auras that proc during combat.
    /// </summary>
    public abstract class Aura
    {
        /// <summary>
        /// The name of the aura.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The current stack size.
        /// </summary>
        public int StackSize { get; set; }

        /// <summary>
        /// The maximum stack size.
        /// </summary>
        public int MaxStacks { get; set; }

        /// <summary>
        /// Indicates if the aura has been activated, or not.
        /// </summary>
        public bool Active
        {
            get { return (StackSize > 0); }
        }

        /// <summary>
        /// The amount of time that this Aura will be active.
        /// </summary>
        public float Duration { get; protected set; }

        /// <summary>
        /// This event is raised whenever the Aura is updated (i.e. its state changes).
        /// Subscribers must be attached or removed via the "+=" or "-=" operators.
        /// </summary>
        /// <remarks>
        /// <![CDATA[Note that by using the generic EventHandler<T> event type we do not need to declare a separate delegate type.]]>
        /// </remarks>
        protected internal event EventHandler AuraUpdate;

        /// <summary>
        /// This method ensures that subscribers are notified when the event has been raised.
        /// </summary>
        protected void OnAuraUpdate()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = AuraUpdate;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Constructor - initialise properties
        /// </summary>
        /// <param name="name">The name of the Aura.</param>
        /// <param name="duration">Time (in seconds) that the aura is active.</param>
        protected Aura(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }

        /// <summary>
        /// Applies the aura.
        /// </summary>
        protected virtual void ApplyAura()
        {
            Trace.WriteLine(String.Format("thread:[{0}] | {1} aura is active", System.Threading.Thread.CurrentThread.ManagedThreadId, Name));
            OnAuraUpdate();
        }

        /// <summary>
        /// Removes the aura.
        /// </summary>
        protected virtual void RemoveAura()
        {
            Trace.WriteLine(String.Format("thread:[{0}] | {1} aura has ended", System.Threading.Thread.CurrentThread.ManagedThreadId, Name));
            OnAuraUpdate();
        }

        /// <summary>
        /// A generic method to handle spellcast notifications and update the aura accordingly.
        /// It must be implemented in the derived Aura type because the logic is different for each aura.
        /// </summary>
        protected internal virtual void SpellCastHandler(Object sender, EventArgs e)
        {
        }
    }


    /// <summary>
    /// When you cast Conflagrate, the cast time and global cooldown of your next three Destruction spells is reduced by 30%. Lasts 15 sec.
    /// </summary>
    public class Backdraft : Aura
    {
        public Backdraft()
            : base("Backdraft", 15f)
        {
        }

        protected override void ApplyAura()
        {
            StackSize = 3;
            base.ApplyAura();
        }

        protected override void RemoveAura()
        {
            StackSize = 0;
            base.RemoveAura();
        }

        protected internal override void SpellCastHandler(Object sender, EventArgs e)
        {
            Conflagrate conflagrate = sender as Conflagrate;

            if (conflagrate != null)
            {
                //conflagrate has been cast, so trigger the aura
                ApplyAura();
            }
            else
            {
                //some other destruction spell has been cast
                if (StackSize > 0)   //Backdraft is currently active
                {
                    StackSize -= 1;

                    if (StackSize == 0)
                    {
                        RemoveAura();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Your Shadow Bolt and Haunt spells apply the Shadow Embrace effect, increasing all shadow periodic damage dealt 
    /// to the target by you by 5%, and reduces all periodic healing done to the target by 15%. 
    /// Lasts for 12 sec. Stacks up to 2 times.
    /// </summary>
    public class ShadowEmbrace : Aura
    {
        public ShadowEmbrace()
            : base("ShadowEmbrace", 12f)
        {
            MaxStacks = 2;
        }

        protected internal override void SpellCastHandler(object sender, EventArgs e)
        {
            base.SpellCastHandler(sender, e);

            if (StackSize < MaxStacks)
            {
                StackSize += 1;
            }

            Trace.WriteLine(String.Format("{0} aura updated - current stacksize = {1}", Name, StackSize));
        }
    }

    /// <summary>
    /// Damage taken from Shadow damage-over-time effects increased by 20%.
    /// </summary>
    public class Haunted : Aura
    {
        public Haunted() : base("Haunted", 12f)
        {
            StackSize = 0;
        }
    }

    /// <summary>
    /// When you deal damage with Corruption, you have 6% chance to increase your spell casting speed by 6/12/20% for 10 sec.
    /// </summary>
    public class Eradication : Aura
    {
        public Eradication()
            : base("Eradication", 10f)
        {

        }
    }

    /// <summary>
    /// When you Shadowbolt or Incinerate a target that is at or below 35% health, the cast time of your next Soulfire is reduced by 60%. Soulfires cast under the effect of Decimation cost no shard. Lasts 10 sec.
    /// </summary>
    public class Decimation : Aura
    {
        public Decimation()
            : base("Decimation", 10f)
        {
            //
        }
    }

    /// <summary>
    /// When you critically strike with Searing Pain or Conflagrate, your Fire and Shadow spell damage is increased by 2/4/6% for 10 sec.
    /// </summary>
    public class Pyroclasm : Aura
    {
        public Pyroclasm()
            : base("Pyroclasm", 10f)
        {
        }
    }

}