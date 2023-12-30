using HarmonyLib;
using Mono.Security;
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
using static UnityEngine.GraphicsBuffer;

namespace Foundation.HarmonyPatches
{

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
}