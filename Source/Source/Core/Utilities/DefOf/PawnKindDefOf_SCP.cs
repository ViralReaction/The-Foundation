using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    [DefOf]
    public static class PawnKindDefOf_SCP
    {
        public static PawnKindDef Foundation_ShyGuy;
        public static PawnKindDef Foundation_Reptile;
        public static PawnKindDef Foundation_ManyVoices_Normal;
        public static PawnKindDef Foundation_ManyVoices_Incident;
        public static PawnKindDef Foundation_Deer;
        public static PawnKindDef Foundation_28451R;
        public static PawnKindDef Foundation_Refuted;
        public static PawnKindDef Foundation_5893R;
        //public static PawnKindDef Foundation_Josie;

        static PawnKindDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf_SCP));
    }
}