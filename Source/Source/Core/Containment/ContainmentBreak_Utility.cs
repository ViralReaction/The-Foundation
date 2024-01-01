using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI.Group;
using Verse.AI;
using Verse;
using static UnityEngine.GraphicsBuffer;
using Foundation.Utilities;

namespace Foundation.Containment
{
    public static class ContainmentBreakUtility
    {
        private const float BaseInitiatePrisonBreakMtbDays = 60f;
        private const float DistanceToJoinPrisonBreak = 20f;
        private const float ChanceForRoomToJoinPrisonBreak = 0.5f;
        private const float SapperChance = 0.5f;
        private static readonly SimpleCurve PrisonBreakMTBFactorForDaysSincePrisonBreak = new SimpleCurve()
    {
      {
        new CurvePoint(0.0f, 20f),
        true
      },
      {
        new CurvePoint(5f, 1.5f),
        true
      },
      {
        new CurvePoint(10f, 1f),
        true
      }
    };
        private static HashSet<Region> tmpRegions = new HashSet<Region>();
        private static HashSet<Room> participatingRooms = new HashSet<Room>();
        private static List<Pawn> allEscapingPrisoners = new List<Pawn>();
        private static List<Room> tmpToRemove = new List<Room>();
        private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();

        public static float InitiatePrisonBreakMtbDays(Pawn pawn, StringBuilder sb = null, bool ignoreAsleep = false)
        {
            if (!ignoreAsleep && !pawn.Awake() || !ContainmentBreakUtility.CanParticipateInContainmentBreak(pawn))
                return -1f;
            Room room = pawn.GetRoom();
            if (room == null || room.Role != SCP_Startup.containmentRoom)
                return -1f;
            float num1 = 60f/ (1+pawn.def.GetModExtension<ContainmentExtension>().containmentTier);
            float f = Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
            float num2 = num1 / f;
            if (sb != null && (double)f != 1.0)
            {
                sb.AppendLineIfNotEmpty();
                sb.Append((string)("FactorForMovement".Translate() + ": " + f.ToStringPercent()));
            }
            float num3 = 0.0f;
            ContainmentBreakUtility.tmpRegions.Clear();
            List<Region> regionList = room.Regions;
            for (int index1 = 0; index1 < regionList.Count; index1++)
            { 
                Region region = regionList[index1];
                List<RegionLink> linkList = region.links;
                for (int index2 = 0; index2 < linkList.Count; index2++)
                { 
                    RegionLink link = linkList[index2];
                    Region otherRegion = link.GetOtherRegion(region);
                    if (otherRegion.type == RegionType.Portal && !ContainmentBreakUtility.tmpRegions.Contains(otherRegion))
                    {
                        ContainmentBreakUtility.tmpRegions.Add(otherRegion);
                        for (int index = 0; index < otherRegion.links.Count; ++index)
                        {
                            Region regionA = otherRegion.links[index].RegionA;
                            Region regionB = otherRegion.links[index].RegionB;
                            if (regionA.Room != room && regionA != otherRegion || regionB.Room != room && regionB != otherRegion)
                            {
                                ++num3;
                                break;
                            }
                        }
                    }
                }
            }
            if ((double)num3 > 0.0)
            {
                num2 /= num3;
                if (sb != null && (double)num3 > 1.0)
                {
                    sb.AppendLineIfNotEmpty();
                    sb.Append((string)("FactorForDoorCount".Translate() + ": " + (1f / num3).ToStringPercent()));
                }
            }
            //int value = FoundationComponent.ContainmentBreakCheck(pawn);
            //float x = (float)(60);
            //num2 *= ContainmentBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
            return num2;
        }

        public static bool CanParticipateInContainmentBreak(Pawn pawn) => !pawn.Downed && pawn.IsSCP() && !ContainmentBreakUtility.IsPrisonBreaking(pawn);
        public static bool IsPrisonBreaking(Pawn pawn)
        {
            if (pawn.InMentalState)
            {
                return true;
            }
            return false;
        }

        public static void StartPrisonBreak(Pawn initiator)
        {
            string letterText;
            string letterLabel;
            LetterDef letterDef;
            ContainmentBreakUtility.StartPrisonBreaker(initiator, out letterText, out letterLabel, out letterDef);
            if (letterText.NullOrEmpty())
                return;
            Find.LetterStack.ReceiveLetter((TaggedString)letterLabel, (TaggedString)letterText, letterDef, (LookTargets)(Thing)initiator);
        }

        public static void StartPrisonBreaker(
            Pawn initiator,
            out string letterText,
            out string letterLabel,
            out LetterDef letterDef)
        {
            ContainmentBreakUtility.participatingRooms.Clear();
            foreach (IntVec3 intVec3 in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
            {
                if (intVec3.InBounds(initiator.Map))
                {
                    Room room = intVec3.GetRoom(initiator.Map);
                    if (room != null && ContainmentBreakUtility.IsOrCanBePrisonCell(room))
                    {
                        ContainmentBreakUtility.participatingRooms.Add(room);
                    }
                }
            }
            ContainmentBreakUtility.RemoveRandomRooms(ContainmentBreakUtility.participatingRooms, initiator);
            ContainmentBreakUtility.allEscapingPrisoners.Clear();
            foreach (Room participatingRoom in ContainmentBreakUtility.participatingRooms)
            {
                //Log.Message("Firing Break");
                //ContainmentBreakUtility.StartPrisonBreakIn(participatingRoom, ContainmentBreakUtility.allEscapingPrisoners, ContainmentBreakUtility.participatingRooms);
                List<Thing> adjacentList = participatingRoom.ContainedAndAdjacentThings;
                for (int index = 0; index < adjacentList.Count; index++)
                {
                    //Log.Message("Are we getting here");
                    Thing thing = adjacentList[index];
                    if (thing is Pawn pawn && ContainmentBreakUtility.CanParticipateInContainmentBreak(pawn))
                    {
                        pawn.mindState.mentalStateHandler.TryStartMentalState(SCP_Startup.containBreachState, forceWake: true, transitionSilently: true);
                        FoundationComponent.ContainmentBreakDict.SetOrAdd(pawn, 0);
                        allEscapingPrisoners.Add(pawn);
                    }

                }
            }
            ContainmentBreakUtility.participatingRooms.Clear();
            if (ContainmentBreakUtility.allEscapingPrisoners.Any<Pawn>())
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < ContainmentBreakUtility.allEscapingPrisoners.Count; ++index)
                    stringBuilder.AppendLine("  - " + ContainmentBreakUtility.allEscapingPrisoners[index].NameShortColored.Resolve());
                letterText = (string)"LetterPrisonBreak".Translate((NamedArgument)stringBuilder.ToString().TrimEndNewlines());
                letterLabel = (string)"LetterLabelPrisonBreak".Translate();
                letterDef = LetterDefOf.ThreatBig;
                ContainmentBreakUtility.allEscapingPrisoners.Clear();
            }
            else
            {
                Log.Message("Null Containment Break letter");
                letterText = (string)null;
                letterLabel = (string)null;
                letterDef = (LetterDef)null;
            }
            Find.TickManager.slower.SignalForceNormalSpeed();
        }

        private static bool IsOrCanBePrisonCell(Room room)
        {
            if (room.Role == SCP_Startup.containmentRoom)
            {
                return true;
            }
            if (room.TouchesMapEdge)
                return false;
            bool flag = false;
            List<Thing> andAdjacentThings = room.ContainedAndAdjacentThings;
            for (int index = 0; index < andAdjacentThings.Count; ++index)
            {
                if (andAdjacentThings[index] is Pawn pawn && pawn.IsSCP())
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
        {
            Room room = initiator.GetRoom();
            ContainmentBreakUtility.tmpToRemove.Clear();
            foreach (Room participatingRoom in participatingRooms)
            {
                if (participatingRoom != room && (double)Rand.Value >= 0.5)
                    ContainmentBreakUtility.tmpToRemove.Add(participatingRoom);
            }
            for (int index = 0; index < ContainmentBreakUtility.tmpToRemove.Count; ++index)
                participatingRooms.Remove(ContainmentBreakUtility.tmpToRemove[index]);
            ContainmentBreakUtility.tmpToRemove.Clear();
        }

        //private static void StartPrisonBreakIn(
        //  Room room,
        //  List<Pawn> outAllEscapingPrisoners,
        //  HashSet<Room> participatingRooms)
        //{
        //    ContainmentBreakUtility.escapingPrisonersGroup.Clear();
        //    List<Thing> adjacentList = room.ContainedAndAdjacentThings;
        //    for (int index = 0; index < adjacentList.Count; index++)
        //    {
        //        Log.Message("Are we getting here");
        //        Thing thing = adjacentList[index];
        //        if (thing is Pawn pawn && ContainmentBreakUtility.CanParticipateInContainmentBreak(pawn))
        //        {
        //            pawn.mindState.mentalStateHandler.TryStartMentalState(SCP_Startup.containBreachState, forceWake: true, transitionSilently: true);
        //        }

        //    }
        //    ContainmentBreakUtility.escapingPrisonersGroup.Clear();
        //}

        //private static void AddPrisonersFrom(Room room)
        //{
        //    List<Thing> adjacentList = room.ContainedAndAdjacentThings;
        //    for (int index = 0; index < adjacentList.Count; index++)
        //    {
        //        Log.Message("Are we getting here");
        //        Thing thing = adjacentList[index];
        //        if (thing is Pawn pawn && ContainmentBreakUtility.CanParticipateInContainmentBreak(pawn))
        //        {
        //            pawn.mindState.mentalStateHandler.TryStartMentalState(SCP_Startup.containBreachState, forceWake: true, transitionSilently: true);
        //        }

        //    }
        //}

        //private static bool RoomsAreCloseToEachOther(Room a, Room b)
        //{
        //    IntVec3 anyCell1 = a.FirstRegion.AnyCell;
        //    IntVec3 anyCell2 = b.FirstRegion.AnyCell;
        //    if (a.Map != b.Map || !anyCell1.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors)))
        //        return false;
        //    using (PawnPath path = a.Map.pathFinder.FindPath(anyCell1, (LocalTargetInfo)anyCell2, TraverseParms.For(TraverseMode.PassDoors)))
        //        return path.Found && path.NodesLeftCount < 24;
        //}
    }
}
