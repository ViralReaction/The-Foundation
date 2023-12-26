using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class DeathActionWorker_DestroyBody : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            if (corpse.Map == null)
                return;
            corpse.Destroy(DestroyMode.Vanish);
        }
    }
}
