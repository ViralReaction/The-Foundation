using Verse;
using LudeonTK;
using Foundation.Containment;

namespace Foundation.Utilities
{
    public static class Debug
    {
        [DebugAction("Pawns", null, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void StartContainmentBreak(Pawn p)
        {
            if (!p.IsSCP())
                return;
            ContainmentBreakUtility.StartPrisonBreak(p);
        }
    }
}
