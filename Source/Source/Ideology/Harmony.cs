using HarmonyLib;
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
    [HarmonyPatch(typeof(ThoughtHandler), "GetSocialThoughts", new System.Type[] { typeof(Pawn), typeof(List<ISocialThought>) })]
    public static class Foundation_Tools_Precept_Patch
    {
        public static bool Prefix(Pawn otherPawn, List<ISocialThought> outThoughts, ThoughtHandler __instance)
        {
            if (!__instance.pawn.IsSCP())
            {
                Ideo ideo = __instance.pawn.Ideo;
                if ((ideo != null ? (ideo.HasPrecept(SCPDefOf.Foundation_Tools) ? 1 : 0) : 0) != 0 && otherPawn.IsSCP())
                    return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(ThoughtHandler), "GetSocialThoughts", new System.Type[] { typeof(Pawn), typeof(List<ISocialThought>) })]
    public static class Foundation_Tools_Bonded_Precept_Patch
    {
        public static bool Prefix(
          Pawn otherPawn,
          List<ISocialThought> outThoughts,
          ThoughtHandler __instance)
        {
            if (!__instance.pawn.IsSCP())
            {
                Ideo ideo = __instance.pawn.Ideo;
                if ((ideo != null ? (ideo.HasPrecept(SCPDefOf.Foundation_Tools) ? 1 : 0) : 0) != 0 && otherPawn.IsSCP())
                    return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ThoughtUtility), "CanGetThought")]
    public static class ThoughtUtility_CanGetThought_Patch
    {
        public static bool Prefix(Pawn pawn) => !pawn.IsSCP();
    }
    [HarmonyPatch(typeof(TaleUtility), "Notify_PawnDied")]
    public static class Foundation_Died_Precept_Patch
    {
        public static void Postfix(Pawn victim, DamageInfo? dinfo)
        {
            if (!victim.IsSCP())
                return;
            if (dinfo?.Instigator is Pawn instigator)
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(SCPDefOf.Foundation_Died, new SignalArgs(instigator.Named(HistoryEventArgsNames.Doer))));
            else
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(SCPDefOf.Foundation_Died));
        }
    }
}