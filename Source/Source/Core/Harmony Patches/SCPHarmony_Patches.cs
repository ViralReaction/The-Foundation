using HarmonyLib;
using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace SCP
{
    [StaticConstructorOnStartup]
    internal static class SCPHarmony
    {
        static SCPHarmony()
        {
            Harmony harmony = new Harmony("rimworld.scp");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SCP939_Starving"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(FoodUtility), "IsAcceptablePreyFor"), new HarmonyMethod(typeof(SCPHarmony), "SCP939_HumansOnlyAcceptablePrey"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Pawn), "TicksPerMove"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SCP939_VoicesMovementSpeed"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(JobDriver_PredatorHunt), "CheckWarnPlayer"), new HarmonyMethod(typeof(SCPHarmony), "SCP939_DontWarnPlayerHunted"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Pawn), "TickRare"), postfix: new HarmonyMethod(typeof(SCPHarmony), "TickMindstateLeaveDaylight"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(WorldPawns), "GetSituation"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SituationSCPEvent"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SCP1675_Starving"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(FoodUtility), "IsAcceptablePreyFor"), new HarmonyMethod(typeof(SCPHarmony), "SCP1675_GeeseOnlyAcceptablePrey"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SCP2584_AlwaysFull"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(SCPHarmony), "SCP2845_AlwaysFull"));
        }

        public static void SCP939_Starving(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP939_A"))
                return;
            __instance.CurLevel = 0.1f;
        }

        public static bool SCP939_HumansOnlyAcceptablePrey(Pawn predator, Pawn prey, ref bool __result)
        {
            if (!(predator.def.defName == "SCP939_A"))
                return true;
            __result = false;
            if (prey.RaceProps.Humanlike)
                __result = true;
            return false;
        }

        public static void SCP939_VoicesMovementSpeed(bool diagonal, Pawn __instance, ref int __result)
        {
            if (__instance.TryGetComp<CompVoices>() == null)
                return;
            if (__instance.GetComp<CompVoices>().VoicesActive)
                __result *= 100;
            else if (__instance.GetComp<CompVoices>().TargetLured)
                __result *= 10000;
        }

        public static bool SCP939_DontWarnPlayerHunted(JobDriver_PredatorHunt __instance) => __instance.pawn.GetComp<CompVoices>() == null;

        public static void TickMindstateLeaveDaylight(Pawn __instance)
        {
            IntVec3 result;
            if (__instance?.kindDef != PawnKindDefOf_SCP.SCP_939_Incident || !__instance.Spawned || GenLocalDate.HourOfDay(__instance.Map) < 5 || GenLocalDate.HourOfDay(__instance.Map) >= 19 || !CellFinder.TryFindRandomPawnExitCell(__instance, out result))
                return;
            Job job = new Job(JobDefOf_SCP.LeaveMapDaylight, (LocalTargetInfo)result);
            __instance.jobs.TryTakeOrderedJob(job, JobTag.DraftedOrder);
        }

        public static void SituationSCPEvent(
          Pawn p,
          ref WorldPawnSituation __result,
          WorldPawns __instance)
        {
            if (__result != WorldPawnSituation.Free || p.kindDef != PawnKindDefOf_SCP.SCP_939_Incident)
                return;
            Log.Message("Checking " + p.LabelShort, false);
            foreach (Map map in Find.Maps)
            {
                if (map.GameConditionManager.ActiveConditions.Any<GameCondition>((Predicate<GameCondition>)(x => x is GameCondition_SCP939 && (x as GameCondition_SCP939).ActiveSCPInArea.Contains(p))))
                {
                    Log.Message("Result Changed ", false);
                    __result = WorldPawnSituation.InTravelingTransportPod;
                    break;
                }
            }
        }
        public static void SCP1675_Starving(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP_1675_Goose_Terminator"))
                return;
            __instance.CurLevel = 0.1f;
        }
        public static bool SCP1675_GeeseOnlyAcceptablePrey(Pawn predator, Pawn prey, ref bool __result)
        {
            if (!(predator.def.defName == "SCP_1675_Goose_Terminator"))
                return true;
            __result = false;
            if (prey.def.defName == "Goose")
                __result = true;
            return false;
        }
        public static void SCP2584_AlwaysFull(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP_2584_Snake"))
                return;
            __instance.CurLevel = 1f;
        }
        public static void SCP2845_AlwaysFull(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP_2845_Deer"))
                return;
            __instance.CurLevel = 1f;
        }

    }
}