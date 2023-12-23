using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public class Hediff_SCP2845 : Hediff_High
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            if (this.pawn.kindDef == PawnKindDefOf_SCP.SCP_2845_Deer || this.pawn.kindDef == PawnKindDefOf_SCP.SCP_28451R)
            {
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf_SCP.SCP_2845_Transmute_Hediff, this.pawn);
                this.pawn.health.RemoveHediff(hediff);
            }
        }
    }
}