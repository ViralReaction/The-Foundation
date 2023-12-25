using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    // Tranq Dart. Checks to see if Pawn is an SCP and is !Tranqable, if so removes the hediff.
    public class Hediff_TranqDart : Hediff_High
    {
        private float severe = 0.15f;
        public override void PostAdd(DamageInfo? dinfo)
        {
            Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SCP.SCP_TranqHediff);
            if (SCP_Startup.IsSCP(this.pawn) && !this.pawn.def.GetModExtension<ContainmentExtension>().isTranqable || this.pawn.RaceProps.IsMechanoid)
            {
                this.pawn.health.RemoveHediff(hediff);
            }
            else
            {
                hediff.Severity += severe / pawn.BodySize;
            }

        }
    }
}