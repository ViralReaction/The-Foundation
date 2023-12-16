using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    [DefOf]
    public static class HediffDefOf
    {
        public static HediffDef SCP_2687Poison;
        public static HediffDef GutWorms;
        public static HediffDef MuscleParasites;

        static HediffDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
    }
}
