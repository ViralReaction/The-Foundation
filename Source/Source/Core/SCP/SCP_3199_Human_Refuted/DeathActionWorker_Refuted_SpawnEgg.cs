using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation;
using Verse.AI.Group;

namespace Foundation
{
    internal class DeathActionWorker_Refuted_SpawnEgg : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            if (corpse.Map == null)
                return;
            GenPlace.TryPlaceThing(ThingMaker.MakeThing(FoundationDefOf.Foundation_Refuted_Egg), corpse.Position, corpse.Map, ThingPlaceMode.Near);
        }
    }
}