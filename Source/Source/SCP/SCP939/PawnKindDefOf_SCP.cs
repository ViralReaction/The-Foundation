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
        public static PawnKindDef SCP_939_Normal;
        public static PawnKindDef SCP_939_Incident;

        static PawnKindDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf_SCP));
    }
}