using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class Utilities
    {
        public static float PlayerAnomalyBodySizePerCapita(Pawn colonist)
        {
            float anomalyNum = 0f;
            int colonistNum = 0;
            List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.Count; i++)
            {
                Pawn pawn = allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction[i];
                if (pawn.IsFreeColonist && !pawn.IsQuestLodger() && !pawn.IsGhoul)
                {
                    colonistNum++;
                }
                if (pawn.IsGhoul || IsMutantAnimal(pawn))
                {
                    anomalyNum += pawn.BodySize;
                }
            }
            IReadOnlyList<Pawn> anomalyList = colonist.MapHeld.mapPawns.AllPawnsUnspawned;
            for (int i = 0; i < anomalyList.Count; i++)
            {
                Pawn pawn = anomalyList[i];
                if (pawn.IsOnHoldingPlatform && pawn.IsEntity)
                {
                    anomalyNum += pawn.BodySize;
                }
            }
            if (colonistNum <= 0)
            {
                return 0f;
            }
            return anomalyNum / (float)colonistNum;
        }

        public static bool IsMutantAnimal(Pawn pawn)
        {
            return pawn.RaceProps.Animal && pawn.IsMutant;
        }

    }
}
