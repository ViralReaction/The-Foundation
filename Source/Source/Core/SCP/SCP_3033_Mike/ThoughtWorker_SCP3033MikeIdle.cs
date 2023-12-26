using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class ThoughtWorker_SCP3033MikeIdle : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SCP.SCP_30331R) == null)
                return (ThoughtState)false;
            var allPawn = p.Map.mapPawns.AllPawns;
            for (int index = 0; index < allPawn.Count; ++index)
            {
                Pawn pawnCheck = allPawn[index];
                if (pawnCheck.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SCP.SCP_30332R) != null && pawnCheck.Faction.HostileTo(Faction.OfPlayer))
                    return (ThoughtState)false;
            }
            Hediff firstHediffOfDef = p.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf_SCP.SCP_TranqHediff);
            if (firstHediffOfDef != null)
                firstHediffOfDef.Severity = 0.0f;
            return (ThoughtState)true;
        }
    }
}
