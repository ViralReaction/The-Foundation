using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    internal class ThoughtWorker_EquippedKatana : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.equipment.Primary == null)
                return (ThoughtState)false;
            if (p.equipment.Primary.def != ThingDefOf_SCP.SCP_572R)
                return (ThoughtState)false;
            p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, (string)("MBReason".Translate() + "SCP-572"), true);
            return (ThoughtState)true;
        }
    }
}
