using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class DeathActionWorker_SCP3199_SpawnEgg : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            if (corpse.Map == null)
                return;
            GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf_SCP.SCP_3199_Egg), corpse.Position, corpse.Map, ThingPlaceMode.Near);
        }
    }
}