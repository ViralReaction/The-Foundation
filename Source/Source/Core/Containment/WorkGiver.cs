using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using System.Reflection;
using Foundation.Utilities;

namespace Foundation.Containment
{
    internal class WorkGiver_TakeSCPBed : WorkGiver_RescueDowned
    {
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Pawn pawn1) || !pawn1.Downed || !pawn1.IsSCP() || pawn1.InBed() || !pawn.CanReserve((LocalTargetInfo)(Thing)pawn1, ignoreOtherReservations: forced) || GenAI.EnemyIsNear(pawn1, 40f) || CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(pawn1))
                return false;
            Thing target = (Thing)null;
            target = (Thing)this.FindBed(pawn, pawn1);
            if (pawn1.ownership.OwnedBed == target)
            {
                return target != null && pawn1.CanReserve((LocalTargetInfo)target);
            }
            return false;
            
        }
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            List<Pawn> pawnList = pawn.Map.mapPawns.SpawnedDownedPawns;
            for (int index = 0; index < pawnList.Count; ++index)
            {
                Pawn spawnedDownedPawn = pawnList[index];
                if (spawnedDownedPawn.IsSCP() && spawnedDownedPawn.GetRoom().Role == SCP_Startup.containmentRoom)
                    return false;
            }
            return true;
        }
    }
}
