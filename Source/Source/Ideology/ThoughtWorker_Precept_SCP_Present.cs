using RimWorld;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class ThoughtWorker_Precept_SCP_Present : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive || p.IsSCP() || p.MapHeld == null)
                return ThoughtState.Inactive;
            //List<Pawn> pawnList = p.MapHeld.mapPawns.AllPawnsSpawned;
            //for (int index = 0; index < pawnList.Count; index++)
            //{
            //    Pawn pawn = pawnList[index];
            //    if (p.IsSCP() && (pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony || pawn.IsColonist))
            //        return ThoughtState.ActiveDefault;

            //}
            List<Pawn> aliveOfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            for (int index = 0; index < aliveOfPlayerFaction.Count; index++)
            {
                Pawn pawn = aliveOfPlayerFaction[index];
                if (pawn.IsSCP())
                    return ThoughtState.ActiveDefault;
            }
            return ThoughtState.Inactive;
        }
    }
}