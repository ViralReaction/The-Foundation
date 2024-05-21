﻿using HarmonyLib;
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
using Mono.Security;
using UnityEngine;
using System.Reflection.Emit;
using Verse.Noise;
using Foundation;

namespace Foundation.HarmonyPatches
{
    [StaticConstructorOnStartup]
    internal static class FoundationHarmony
    {
        static FoundationHarmony()
        {
            Harmony harmony = new Harmony("rw.foundation");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SCP096_Full"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SCP106_Starving"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(FoodUtility), "IsAcceptablePreyFor"), new HarmonyMethod(typeof(FoundationHarmony), "SCP106_HumansOnlyAcceptablePrey"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(JobDriver_PredatorHunt), "CheckWarnPlayer"), new HarmonyMethod(typeof(FoundationHarmony), "SCP106_DontWarnPlayerHunted"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SCP939_Starving"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(FoodUtility), "IsAcceptablePreyFor"), new HarmonyMethod(typeof(FoundationHarmony), "SCP939_HumansOnlyAcceptablePrey"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Pawn), "TicksPerMove"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SCP939_VoicesMovementSpeed"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(JobDriver_PredatorHunt), "CheckWarnPlayer"), new HarmonyMethod(typeof(FoundationHarmony), "SCP939_DontWarnPlayerHunted"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Pawn), "TickRare"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "TickMindstateLeaveDaylight"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(WorldPawns), "GetSituation"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SituationSCPEvent"));
        }

        public static void SCP096_Full(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "Foundation_096_Shy_Guy"))
                return;
            __instance.CurLevel = 1f;
        }

        public static void SCP106_Starving(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "Foundation_106_Old_Man"))
                return;
            __instance.CurLevel = 0.1f;
        }

        public static bool SCP106_HumansOnlyAcceptablePrey(Pawn predator, Pawn prey, ref bool __result)
        {
            if (!(predator.def.defName == "Foundation_106_Old_Man"))
                return true;
            __result = false;
            if (prey.RaceProps.Humanlike)
                __result = true;
            return false;
        }
        public static bool SCP106_DontWarnPlayerHunted(JobDriver_PredatorHunt __instance) => __instance.pawn.GetComp<Comp_OldMan>() == null;

        public static void SCP939_Starving(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "Foundation_ManyVoices"))
                return;
            __instance.CurLevel = 0.1f;
        }

        public static bool SCP939_HumansOnlyAcceptablePrey(Pawn predator, Pawn prey, ref bool __result)
        {
            if (!(predator.def.defName == "Foundation_ManyVoices"))
                return true;
            __result = false;
            if (prey.RaceProps.Humanlike)
                __result = true;
            return false;
        }

        public static void SCP939_VoicesMovementSpeed(bool diagonal, Pawn __instance, ref float __result)
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
            if (__instance?.kindDef != PawnKindDefOf_SCP.Foundation_ManyVoices_Incident || !__instance.Spawned || GenLocalDate.HourOfDay(__instance.Map) < 5 || GenLocalDate.HourOfDay(__instance.Map) >= 19 || !CellFinder.TryFindRandomPawnExitCell(__instance, out result) || __instance.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse) || __instance.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.VolcanicWinter) || __instance.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness))
                return;
            Job job = new Job(SCPDefOf.LeaveMapDaylight, (LocalTargetInfo)result);
            __instance.jobs.TryTakeOrderedJob(job, JobTag.DraftedOrder);
        }

        public static void SituationSCPEvent(
          Pawn p,
          ref WorldPawnSituation __result,
          WorldPawns __instance)
        {
            if (__result != WorldPawnSituation.Free || p.kindDef != PawnKindDefOf_SCP.Foundation_ManyVoices_Incident)
                return;
            foreach (Map map in Find.Maps)
            {
                if (map.GameConditionManager.ActiveConditions.Any<GameCondition>((Predicate<GameCondition>)(x => x is GameCondition_SCP939 && (x as GameCondition_SCP939).ActiveSCPInArea.Contains(p))))
                {
                    __result = WorldPawnSituation.InTravelingTransportPod;
                    break;
                }
            }
        }

        [HarmonyPatch(typeof(TradeUtility), "AllSellableColonyPawns")]
        class SellCapturedFoundation_Patch
        {
            public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> values, Map map, bool checkAcceptableTemperatureOfAnimals)
            {
                foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
                {
                    if (pawn.IsOnHoldingPlatform && pawn.HostFaction == null && !pawn.InMentalState && !pawn.Downed && (!checkAcceptableTemperatureOfAnimals || map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn.def)))
                        yield return pawn;
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "Kill")]
        public class NotifyPawnDiedPatch
        {
            public static void Postfix(Pawn __instance)
            {
                Current.Game.GetComponent<FoundationComponent>().Notify_PawnDied(__instance);
            }

        }
    }
}