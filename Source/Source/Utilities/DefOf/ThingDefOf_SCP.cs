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
        public static ThingDef SCP_848R;
        public static ThingDef SCP_113_Stone;
        [MayRequireIdeology]
        public static ThingDef SCP_475_Soap;
        public static ThingDef SCP_578R;
        public static ThingDef SCP_1577_Flare_Gun;
        public static ThingDef SCP_ES_019R;
        public static ThingDef SCP_3118R;
        public static ThingDef SCP_1762_Dragon_Box;
        public static ThingDef SCP_2228_Playset;
        public static ThingDef SCP_500_Panacea;
        public static ThingDef SCP_1797Consume;
        static ThingDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf_SCP));
    }
}
