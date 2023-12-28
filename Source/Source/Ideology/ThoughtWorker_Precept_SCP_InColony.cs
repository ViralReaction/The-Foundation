using RimWorld;
using Foundation.Cage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class ThoughtWorker_Precept_SCP_InColony : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive || p.Faction == null || p.MapHeld == null)
                return ThoughtState.Inactive;
            int stageIndex = 0;
            List<Pawn> pawnList = p.MapHeld.mapPawns.AllPawnsSpawned;
            for (int index = 0; index < pawnList.Count; index++)
            {
                Pawn pawn = pawnList[index];
                if (pawn.IsCaptiveOf() && pawn.Faction != Faction.OfPlayer)
                {
                    ++stageIndex;
                }
            }
            List<Pawn> aliveOfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            for (int index = 0; index < aliveOfPlayerFaction.Count; index++)
            {
                Pawn pawn = aliveOfPlayerFaction[index];
                if (pawn.RaceProps.Animal && pawn.IsSCP())
                    ++stageIndex;
            }
                if (stageIndex > 10)
                stageIndex = 10;
            return ThoughtState.ActiveAtStage(stageIndex);
        }
    }
}