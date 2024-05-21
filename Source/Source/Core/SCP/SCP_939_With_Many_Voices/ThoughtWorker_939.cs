using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation;

namespace Foundation
{
    internal class ThoughtWorker_SCP939 : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(SCPDefOf.Foundation_939_Breath_Hediff);
            if (firstHediffOfDef == null || firstHediffOfDef.FullyImmune())
                return (ThoughtState)false;
            if ((double)firstHediffOfDef.Severity >= 0.35)
                p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Psychotic, (string)("MBReason".Translate() + "SCP-939"));
            return (ThoughtState)true;
        }
    }
}