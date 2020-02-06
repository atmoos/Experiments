using System;

namespace RestrictingDictionary
{
    public abstract class Slot
    {
    }
    public sealed class Head : Slot { }
    public sealed class Torso : Slot { }
    public sealed class Legs : Slot { }
}
