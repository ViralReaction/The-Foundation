using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class DeathActionWorker_DestroyBody : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            if (corpse.Map == null)
                return;
            corpse.Destroy(DestroyMode.Vanish);
        }
    }
}
