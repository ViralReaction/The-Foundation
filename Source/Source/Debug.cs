using Foundation.Containment;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public static class Debug
    {
        [DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
        private static void StartContainmentBreak(Pawn p)
        {
            if (!p.IsSCP())
                return;
            ContainmentBreakUtility.StartPrisonBreak(p);
        }
    }
}
