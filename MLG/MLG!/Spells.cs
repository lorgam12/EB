﻿using System.Collections.Generic;
using EloBuddy;

namespace MLG
{
    public class AkbarSpell
    {
        public AkbarSpell(Champion _hero, SpellSlot slot)
        {
            Slot = slot;
            Hero = _hero;
        }

        public SpellSlot Slot;
        public Champion Hero;
    }

    public class AkbarSpells
    {
        public static List<AkbarSpell> Spells = new List<AkbarSpell>
        {
            new AkbarSpell(Champion.Annie, SpellSlot.R),
            new AkbarSpell(Champion.Corki, SpellSlot.R),
            new AkbarSpell(Champion.Jinx, SpellSlot.R),
            new AkbarSpell(Champion.Ziggs, SpellSlot.R),
        };
    }
}
