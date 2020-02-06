using System;
using System.Collections.Generic;

namespace RestrictingDictionary
{
    public sealed class SafeDictionary
    {
        private readonly Dictionary<Slot, Armor> _dictionary = new Dictionary<Slot, Armor>();
        public Armor this[Slot slot] => _dictionary[slot];
        public void Add<TSlot>(TSlot slot, Armor<TSlot> armor)
            where TSlot : Slot, new()
        {
            _dictionary.Add(slot, armor);
        }

        static void Main()
        {
            var head = new Armor<Head>();
            var torso = new Armor<Torso>();
            var torsoKey = new Torso();
            var dictionary = new SafeDictionary();
            dictionary.Add(head.Slot, head);
            dictionary.Add(torsoKey, torso);
            Console.WriteLine(torso == dictionary[torsoKey]);
            // Do not compile:
            // dictionary.Add(torsoKey, head);
            // dictionary.Add<Torso>(torsoKey, head);
        }
    }
}