using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    // Adds plastic poisoning for SCP2687 on adding Hediff with this class.
    public class Hediff_SCP2687 : Hediff_High
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            if (!this.pawn.health.hediffSet.HasHediff(HediffDefOf_SCP.SCP_2687_Poison))
            {
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf_SCP.SCP_2687_Poison, this.pawn);
                hediff.Severity = 0.05f;
                this.pawn.health.AddHediff(hediff);
            }
            else
                this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SCP.SCP_2687_Poison).Severity += 0.05f;
        }
    }
}