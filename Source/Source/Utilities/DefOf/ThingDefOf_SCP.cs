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
        static ThingDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf_SCP));
    }
}
