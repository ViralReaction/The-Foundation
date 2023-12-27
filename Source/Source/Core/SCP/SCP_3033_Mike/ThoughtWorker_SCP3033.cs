using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class ThoughtWorker_SCP3033Mike : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.health.hediffSet.GetFirstHediffOfDef(SCPDefOF.SCP_30331R) != null)
            {
                List<Pawn> allPawn = p.MapHeld.mapPawns.AllPawnsSpawned;
                for (int index = 0; index < allPawn.Count; index++)
                {
                    Pawn pawnCheck = allPawn[index];
                    if (pawnCheck.health.hediffSet.GetFirstHediffOfDef(SCPDefOF.SCP_30332R) != null && pawnCheck.Faction.HostileTo(Faction.OfPlayer))
                    {
                        if (p.IsPrisonerInPrisonCell() && !PrisonBreakUtility.IsPrisonBreaking(p) && PrisonBreakUtility.CanParticipateInPrisonBreak(p))
                            PrisonBreakUtility.StartPrisonBreak(p);
                        if (p.Faction.IsPlayer)
                            p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, (string)("MBReason".Translate() + "SCP-3033-1"));
                        Hediff firstHediffOfDef = p.health?.hediffSet?.GetFirstHediffOfDef(SCPDefOF.SCP_TranqHediff);
                        if (firstHediffOfDef != null)
                            firstHediffOfDef.Severity = 0.0f;
                        return (ThoughtState)true;
                    }
                }
            }
            return (ThoughtState)false;
        }
    }
}
