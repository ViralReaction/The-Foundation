using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public class Hediff_610_Infection : Hediff_High
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            if (this.pawn.kindDef == PawnKindDefOf_SCP.SCP_610A || this.pawn.kindDef == PawnKindDefOf_SCP.SCP_610B)
            {
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf_SCP.SCP_610_Infection, this.pawn);
                this.pawn.health.RemoveHediff(hediff);
            }
        }
    }
}