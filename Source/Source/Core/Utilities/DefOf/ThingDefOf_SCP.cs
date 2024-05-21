﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    [DefOf]
    public static class ThingDefOf_SCP
    {
        public static ThingDef Foundation_Refuted_Egg;
        public static ThingDef Foundation_113_Stone;
        [MayRequireIdeology]
        public static ThingDef Foundation_475_Soap;
        public static ThingDef Foundation_578_Bloodstone_Maker;
        public static ThingDef Foundation_578_1_Bloodstone;
        public static ThingDef Foundation_500_Panacea;
        public static ThingDef Filth_OldMan;
        public static TraitDef Cannibal;
        public static ThingDef RawBerries;
        public static SoundDef Hive_Spawn;

        static ThingDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf_SCP));
    }
}
