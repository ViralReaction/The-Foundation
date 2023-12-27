using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SCP.Cage
{

    [StaticConstructorOnStartup]
    public static class HarmonyContainmentPatches
    {
        private static readonly Type patchType = typeof(HarmonyContainmentPatches);
        static HarmonyContainmentPatches()
        {
            Harmony harmony = new Harmony("scp.cages");
            harmony.Patch(AccessTools.Method(typeof(BedInteractionCellSearchPattern), nameof(BedInteractionCellSearchPattern.BedCellOffsets)), prefix: new HarmonyMethod(patchType, nameof(BedCellOffsets_Patch)));
            harmony.Patch(AccessTools.Method(typeof(RestUtility), nameof(RestUtility.CanUseBedEver)), postfix: new HarmonyMethod(patchType, nameof(CanUseBedEverPostfix)));
            harmony.Patch(AccessTools.Method(typeof(RestUtility), nameof(RestUtility.IsValidBedFor)), prefix: new HarmonyMethod(patchType, nameof(IsValidBedEverPostfix)));
        }

        [HarmonyPatch(typeof(FloatMenuMakerMap))]
        [HarmonyPatch("AddHumanlikeOrders")]
        [HarmonyPatch(new System.Type[] { typeof(Vector3), typeof(Pawn), typeof(List<FloatMenuOption>) })]
        internal static class FloatMenuMakerMap_Modify_AddHumanlikeOrders
        {
            [HarmonyPostfix]
            private static void AddMenuItems(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
            {
                IntVec3 c1 = IntVec3.FromVector3(clickPos);
                if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                {
                    foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
                    {
                        Pawn victim = (Pawn)localTargetInfo.Thing;
                        if (victim.IsSCP() && !victim.InBed() && pawn.CanReserveAndReach((LocalTargetInfo)(Thing)victim, PathEndMode.OnCell, Danger.Deadly, ignoreOtherReservations: true) && !victim.mindState.WillJoinColonyIfRescued)
                        {
                            TaggedString label = /*(HealthAIUtility.ShouldSeekMedicalRest(victim) ? 1 : (!victim.ageTracker.CurLifeStage.alwaysDowned ? 1 : 0)) != 0 ? */"Capture".Translate((NamedArgument)victim.LabelCap, (NamedArgument)(Thing)victim) /*: "PutSomewhereSafe".Translate((NamedArgument)victim.LabelCap, (NamedArgument)(Thing)victim)*/;
                            opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((string)label, (Action)(() =>
                            {
                                Building_Bed targetB = RestUtility.FindBedFor(victim, pawn, false) ?? RestUtility.FindBedFor(victim, pawn, false, true);
                                if (targetB == null)
                                {
                                    string str = !victim.RaceProps.Animal ? (string)"NoNonPrisonerBed".Translate() : (string)"NoAnimalBed".Translate();
                                    Messages.Message((string)("CannotRescue".Translate() + ": " + str), (LookTargets)(Thing)victim, MessageTypeDefOf.RejectInput, false);
                                }
                                else
                                {
                                    Job job = JobMaker.MakeJob(JobDefOf.Rescue, (LocalTargetInfo)(Thing)victim, (LocalTargetInfo)(Thing)targetB);
                                    job.count = 1;
                                    pawn.jobs.TryTakeOrderedJob(job);
                                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
                                }
                            }), MenuOptionPriority.Default, revalidateClickTarget: ((Thing)victim)), pawn, (LocalTargetInfo)(Thing)victim));
                        }
                    }
                }
            }
        }
        public static bool BedCellOffsets_Patch(List<IntVec3> offsets, IntVec2 size, int slot)
        {
            if (size.z == 1 && size.x == 1 || size.z == 2)
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
        public static bool IsValidBedEverPostfix (Thing bedThing, Pawn sleeper)
        {
            if (sleeper.IsSCP() && bedThing.GetRoom().Role != SCP_Startup.containmentRoom)
                return false;
            return true;
        }
    }

}