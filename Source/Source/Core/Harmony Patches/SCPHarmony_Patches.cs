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
using Mono.Security;
using UnityEngine;
using Foundation.Containment;
using System.Reflection.Emit;
using Verse.Noise;
using Foundation.Utilities;

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
            harmony.Patch((MethodBase)AccessTools.Method(typeof(Need_Food), "NeedInterval"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "SCP1675_Starving"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(FoodUtility), "IsAcceptablePreyFor"), new HarmonyMethod(typeof(FoundationHarmony), "SCP1675_GeeseOnlyAcceptablePrey"));
            harmony.Patch((MethodBase)AccessTools.Method(typeof(ThingDef), "SpecialDisplayStats"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "StatDrawEntry_Patch"));
            harmony.Patch(AccessTools.Method(typeof(BedInteractionCellSearchPattern), "BedCellOffsets"), prefix: new HarmonyMethod(typeof(FoundationHarmony), "BedCellOffsets_Patch"));
            harmony.Patch(AccessTools.Method(typeof(RestUtility), "CanUseBedEver"), postfix: new HarmonyMethod(typeof(FoundationHarmony), "CanUseBedEverPostfix"));
            harmony.Patch(AccessTools.Method(typeof(RestUtility), "IsValidBedFor"), prefix: new HarmonyMethod(typeof(FoundationHarmony), "IsValidBedEverPostfix"));
        }

        public static void SCP096_Full(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP_096_Shy_Guy"))
                return;
            __instance.CurLevel = 1f;
        }

        public static void SCP106_Starving(Need_Food __instance, Pawn ___pawn)
        {
            if (!(___pawn.def.defName == "SCP_106_Old_Man"))
                return;
            __instance.CurLevel = 0.1f;
        }

        public static bool SCP106_HumansOnlyAcceptablePrey(Pawn predator, Pawn prey, ref bool __result)
        {
            if (!(predator.def.defName == "SCP_106_Old_Man"))
                return true;
            __result = false;
            if (prey.RaceProps.Humanlike)
                __result = true;
            return false;
        }
        public static bool SCP106_DontWarnPlayerHunted(JobDriver_PredatorHunt __instance) => __instance.pawn.GetComp<Comp_OldMan>() == null;

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
            Job job = new Job(SCPDefOf.LeaveMapDaylight, (LocalTargetInfo)result);
            __instance.jobs.TryTakeOrderedJob(job, JobTag.DraftedOrder);
        }

        public static void SituationSCPEvent(
          Pawn p,
          ref WorldPawnSituation __result,
          WorldPawns __instance)
        {
            if (__result != WorldPawnSituation.Free || p.kindDef != PawnKindDefOf_SCP.SCP_939_Incident)
                return;
            Log.Message("Checking " + p.LabelShort);
            foreach (Map map in Find.Maps)
            {
                if (map.GameConditionManager.ActiveConditions.Any<GameCondition>((Predicate<GameCondition>)(x => x is GameCondition_SCP939 && (x as GameCondition_SCP939).ActiveSCPInArea.Contains(p))))
                {
                    Log.Message("Result Changed ");
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
        public static void StatDrawEntry_Patch(ThingDef __instance, ref IEnumerable<StatDrawEntry> __result)
        {
            if (!__instance.IsSCP() || __instance.IsCorpse)
                return;
            StatCategoryDef category = StatCategoryDefOf.Basics;
            if (__instance.IsWeapon)
                category = StatCategoryDefOf.Weapon;
            else if (__instance.IsApparel)
                category = StatCategoryDefOf.Apparel;
            else if (__instance.IsDrug)
                category = StatCategoryDefOf.Drug;
            else if (__instance.isTechHediff)
                category = StatCategoryDefOf.Implant;
            ContainmentExtension modExtension = __instance.GetModExtension<ContainmentExtension>();
            if (modExtension.classRating.Count == 3)
            {
                __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Contain".Translate(), (string)modExtension.classRating[0].Translate(), (string)modExtension.FindDescription(0).Translate(), 1));
                __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Disrupt".Translate(), (string)modExtension.classRating[1].Translate(), (string)modExtension.FindDescription(1).Translate(), 1));
                __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Risk".Translate(), (string)modExtension.classRating[2].Translate(), (string)modExtension.FindDescription(2).Translate(), 1));
            }
            else
                Log.Error("SCP Harmony Patching Error: " + __instance.defName + " Does not have correct number of containment procedures.");
        }
        public static bool BedCellOffsets_Patch(List<IntVec3> offsets, IntVec2 size, int slot)
        {
            if (size.z < 2)
                return true;
            bool rightEdge = slot == 0;
            bool leftEdge = slot == BedUtility.GetSleepingSlotsCount(size) - 1;
            BedInteractionCellSearchPattern.BedCellOffsets2xN(offsets, rightEdge, leftEdge);
            return false;
        }
        public static void CanUseBedEverPostfix(ref bool __result, Pawn p, ThingDef bedDef)
        {
            if (__result)
            {
                if (p.IsSCP())
                {
                    if (bedDef == ThingDefOf_SCP.Containment_Zone_Small || bedDef == ThingDefOf_SCP.Containment_Zone_Medium || bedDef == ThingDefOf_SCP.Containment_Zone_Large)
                        __result = true;
                    else
                        __result = false;
                }
                else if (bedDef == ThingDefOf_SCP.Containment_Zone_Small || bedDef == ThingDefOf_SCP.Containment_Zone_Medium || bedDef == ThingDefOf_SCP.Containment_Zone_Large)
                {
                    __result = false;
                }
            }
        }
        public static bool IsValidBedEverPostfix(Thing bedThing, Pawn sleeper)
        {
            if (sleeper.IsSCP() && bedThing.GetRoom().Role != SCP_Startup.containmentRoom)
                return false;
            return true;
        }

        [HarmonyPatch(typeof(TradeUtility), "AllSellableColonyPawns")]
        class SellCapturedSCP_Patch
        {
            public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> values, Map map, bool checkAcceptableTemperatureOfAnimals)
            {
                foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
                {
                    if (pawn.IsCaptiveOf() && pawn.HostFaction == null && !pawn.InMentalState && !pawn.Downed && (!checkAcceptableTemperatureOfAnimals || map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn.def)))
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