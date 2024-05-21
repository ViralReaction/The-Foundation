using HarmonyLib;
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
    [HarmonyPatch(typeof(ThoughtHandler), "GetSocialThoughts", new System.Type[] { typeof(Pawn), typeof(List<ISocialThought>) })]
    public static class Foundation_Tools_Precept_Patch
    {
        public static bool Prefix(Pawn otherPawn, List<ISocialThought> outThoughts, ThoughtHandler __instance)
        {
            if (!__instance.pawn.IsMutant)
            {
                Ideo ideo = __instance.pawn.Ideo;
                if ((ideo != null ? (ideo.HasPrecept(FoundationDefOf.Foundation_Tools) ? 1 : 0) : 0) != 0 && otherPawn.IsMutant)
                    return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(ThoughtHandler), "GetSocialThoughts", new System.Type[] { typeof(Pawn), typeof(List<ISocialThought>) })]
    public static class Foundation_Tools_Bonded_Precept_Patch
    {
        public static bool Prefix(Pawn otherPawn, List<ISocialThought> outThoughts, ThoughtHandler __instance)
        {
            if (!__instance.pawn.IsMutant)
            {
                Ideo ideo = __instance.pawn.Ideo;
                if ((ideo != null ? (ideo.HasPrecept(FoundationDefOf.Foundation_Tools) ? 1 : 0) : 0) != 0 && otherPawn.IsMutant)
                    return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TaleUtility), "Notify_PawnDied")]
    public static class Foundation_Died_Precept_Patch
    {
        public static void Postfix(Pawn victim, DamageInfo? dinfo)
        {
            if (victim.IsEntity || victim.IsMutant)
            {
                if (dinfo?.Instigator is Pawn instigator)
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(FoundationDefOf.Foundation_Died, new SignalArgs(instigator.Named(HistoryEventArgsNames.Doer))));
                else
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(FoundationDefOf.Foundation_Died));
            }
        }
    }
}