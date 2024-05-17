using RimWorld;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    public class ThoughtWorker_Precept_AnomalyPresent : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive || p.IsSCP() || p.MapHeld == null)
                return ThoughtState.Inactive;
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