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
    public static class HediffDefOf_SCP
    {
        public static HediffDef SCP_2687_Poison; // Plastic Organ Poisoning
        public static HediffDef SCP_939_Breath_Hediff; // With Many Voices Amneisa Breath
        public static HediffDef SCP_2845_Transmute_Hediff; // The Deer Transmute Aura
        public static HediffDef SCP_1797_Flu; // Kitten Flu
        public static HediffDef GutWorms; // Pope Soap Cleanining
        public static HediffDef SCP_TranqHediff;
        public static HediffDef SCP_30331R;
        public static HediffDef SCP_30332R;


        static HediffDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf_SCP));
    }
}
