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
    public class ThoughtWorker_Precept_AnomalyMap : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p.Faction == null || !p.IsColonist)
            {
                return false;
            }
            IReadOnlyList<Pawn> spawnedAnomaly = p.MapHeld.mapPawns.AllPawnsSpawned;
            for (int i = 0; i < spawnedAnomaly.Count; i++)
            {
                Pawn pawn = spawnedAnomaly[i];
                if (pawn.IsMutant || pawn.IsEntity)
                    return ThoughtState.ActiveDefault;
            }
            if (p.MapHeld.IsPlayerHome)
            {
                IReadOnlyList<Pawn> containedAnomaly = p.MapHeld.mapPawns.AllPawnsUnspawned;
                for (int i = 0; i < containedAnomaly.Count; i++)
                {
                    Pawn pawn = containedAnomaly[i];
                    if (pawn.IsOnHoldingPlatform)
                        return ThoughtState.ActiveDefault;
                }
            }
            return ThoughtState.Inactive;

        }
    }
}