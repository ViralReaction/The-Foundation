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
    public static class PawnKindDefOf_SCP
    {
        public static PawnKindDef SCP_131_Eye_Pod;
        public static PawnKindDef SCP_610A;
        public static PawnKindDef SCP_610B;
        public static PawnKindDef SCP_939_Normal;
        public static PawnKindDef SCP_939_Incident;
        public static PawnKindDef SCP_2845_Deer;
        public static PawnKindDef SCP_28451R;
        public static PawnKindDef SCP_1797A_Kitten;
        public static PawnKindDef SCP_3199R;

        static PawnKindDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf_SCP));
    }
}