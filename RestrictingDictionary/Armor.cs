using System;

namespace RestrictingDictionary
{
    public abstract class Armor
    {
        public Slot Slot { get; }

        protected Armor(Slot slot) => Slot = slot;
    }
    public sealed class Armor<TSlot> : Armor
        where TSlot : Slot, new()
    {
        public Armor() : base(new TSlot()) { }
    }
}