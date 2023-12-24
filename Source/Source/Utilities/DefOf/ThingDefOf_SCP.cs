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
    public static class ThingDefOf_SCP
    {
        public static ThingDef Containment_Zone;
        public static ThingDef SCP_3199_Egg;
        public static ThingDef SCP_3199_Egg_Ruined;
        public static ThingDef SCP_610A;
        public static ThingDef SCP_19051R;
        public static ThingDef SCP_127R;
        public static ThingDef SCP_572R;
        public static ThingDef Apparel_Jacket;
        public static ThingDef Gun_BoltActionRifle;
        public static ThingDef Synthread;
        static ThingDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf_SCP));
    }
}
