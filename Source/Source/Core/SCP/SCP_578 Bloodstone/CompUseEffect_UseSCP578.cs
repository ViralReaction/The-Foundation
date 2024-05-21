using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static UnityEngine.GraphicsBuffer;
using Foundation;

namespace Foundation
{
    public class IngestionOutcomeDoer_IngestSCP578 : IngestionOutcomeDoer
    {
        //public override void DoEffect(Pawn usedBy)
        //{
        //    Thing thing = GenSpawn.Spawn(ThingDefOf_SCP.Foundation_578_1_Bloodstone, usedBy.Position, usedBy.Map);
        //    thing.stackCount = 20;
        //    GenPlace.TryPlaceThing(thing, usedBy.Position, usedBy.Map, ThingPlaceMode.Direct);
        //    usedBy.Kill(null);
        //}
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Thing thing = GenSpawn.Spawn(ThingDefOf_SCP.Foundation_578_1_Bloodstone, pawn.Position, pawn.Map);
            thing.stackCount = 100;
            GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Direct);
            pawn.Kill(null);
        }
    }
}