using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    // Adds plastic poisoning for SCP2687 on adding Hediff with this class.
    public class Hediff_KittenFlu : Hediff_High
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            if (this.pawn.kindDef == PawnKindDefOf_SCP.SCP_1797A_Kitten)
            {
                Hediff hediff = HediffMaker.MakeHediff(SCPDefOf.SCP_1797_Flu, this.pawn);
                this.pawn.health.RemoveHediff(hediff);
            }
        }
    }
}