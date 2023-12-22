using Mono.Security;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace SCP.Cage
{
    internal class CageUtility
    {
        public static bool CanUseBedEver(Pawn p, ThingDef bedDef) => !p.RaceProps.IsMechanoid && (double)p.BodySize <= (double)bedDef.building.bed_maxBodySize/* && p.RaceProps.Humanlike == bedDef.building.bed_humanlike*/;
        public static bool BedOwnerWillShare(Building_Bed bed, Pawn sleeper, GuestStatus? guestStatus)
        {
            if (!bed.OwnersForReading.Any<Pawn>())
                return true;
            if (!sleeper.IsPrisoner)
            {
                GuestStatus? nullable = guestStatus;
                GuestStatus guestStatus1 = GuestStatus.Prisoner;
                if (!(nullable.GetValueOrDefault() == guestStatus1 & nullable.HasValue) && !sleeper.IsSlave)
                {
                    nullable = guestStatus;
                    GuestStatus guestStatus2 = GuestStatus.Slave;
                    if (!(nullable.GetValueOrDefault() == guestStatus2 & nullable.HasValue))
                    {
                        if (!bed.AnyUnownedSleepingSlot)
                            return false;
                        goto label_9;
                    }
                }
            }
            if (!bed.AnyUnownedSleepingSlot)
                return false;
            label_9:
            return true;
        }
        public static bool CanUseBedNow(
          Thing bedThing,
          Pawn sleeper,
          bool checkSocialProperness,
          bool allowMedBedEvenIfSetToNoCare = false,
          GuestStatus? guestStatusOverride = null)
        {
            if (!(bedThing is Building_Bed buildingBed) || !buildingBed.Spawned || buildingBed.Map != sleeper.MapHeld || buildingBed.IsBurning() || !RestUtility.CanUseBedEver(sleeper, buildingBed.def))
                return false;
            int? assignedSleepingSlot;
            bool flag1 = buildingBed.IsOwner(sleeper, out assignedSleepingSlot);
            int? sleepingSlot;
            bool flag2 = sleeper.CurrentBed(out sleepingSlot) == buildingBed;
            if (!buildingBed.AnyUnoccupiedSleepingSlot && !flag1 && !flag2)
                return false;
            GuestStatus? nullable1;
            GuestStatus? nullable2 = nullable1 = guestStatusOverride ?? sleeper.GuestStatus;
            GuestStatus guestStatus1 = GuestStatus.Prisoner;
            bool forPrisoner = nullable2.GetValueOrDefault() == guestStatus1 & nullable2.HasValue;
            nullable2 = nullable1;
            GuestStatus guestStatus2 = GuestStatus.Slave;
            bool flag3 = nullable2.GetValueOrDefault() == guestStatus2 & nullable2.HasValue;
            if (checkSocialProperness && !buildingBed.IsSociallyProper(sleeper, forPrisoner) || buildingBed.ForPrisoners != forPrisoner || buildingBed.ForSlaves != flag3 || buildingBed.ForPrisoners && !buildingBed.Position.IsInPrisonCell(buildingBed.Map))
                return false;
            if (buildingBed.Medical)
            {
                if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper) || !HealthAIUtility.ShouldSeekMedicalRest(sleeper))
                    return false;
            }
            else
            {
                if (!flag1 && !RestUtility.BedOwnerWillShare(buildingBed, sleeper, guestStatusOverride))
                    return false;
                if (flag2)
                {
                    int? nullable3 = sleepingSlot;
                    int? nullable4 = assignedSleepingSlot;
                    if (!(nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() & nullable3.HasValue == nullable4.HasValue))
                        return false;
                }
            }
            return true;
        }
        public static bool IsValidBedFor(
      Thing bedThing,
      Pawn sleeper,
      Pawn traveler,
      bool checkSocialProperness,
      bool allowMedBedEvenIfSetToNoCare = false,
      bool ignoreOtherReservations = false,
      GuestStatus? guestStatus = null)
        {
            if (!CageUtility.CanUseBedNow(bedThing, sleeper, checkSocialProperness, allowMedBedEvenIfSetToNoCare, guestStatus))
                return false;
            Building_Bed buildingBed = bedThing as Building_Bed;
            if (!traveler.CanReserveAndReach((LocalTargetInfo)(Thing)buildingBed, PathEndMode.OnCell, Danger.Some, buildingBed.SleepingSlotsCount, ignoreOtherReservations: ignoreOtherReservations) || traveler.HasReserved<JobDriver_TakeToBed>((LocalTargetInfo)(Thing)buildingBed, new LocalTargetInfo?((LocalTargetInfo)(Thing)sleeper)) || buildingBed.IsForbidden(traveler))
                return false;
            GuestStatus? nullable = guestStatus;
            GuestStatus guestStatus1 = GuestStatus.Prisoner;
            int num = nullable.GetValueOrDefault() == guestStatus1 & nullable.HasValue ? 1 : 0;
            nullable = guestStatus;
            GuestStatus guestStatus2 = GuestStatus.Slave;
            bool flag = nullable.GetValueOrDefault() == guestStatus2 & nullable.HasValue;
            return num != 0 || flag || buildingBed.Faction == traveler.Faction || traveler.HostFaction != null && buildingBed.Faction == traveler.HostFaction;
        }
        public static Building_Bed FindBedFor(
      Pawn sleeper,
      Pawn traveler,
      bool checkSocialProperness,
      bool ignoreOtherReservations = false,
      GuestStatus? guestStatus = null)
        {
            //bool flag = false;
            if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
            {
                if (sleeper.InBed() && sleeper.CurrentBed().Medical && CageUtility.IsValidBedFor((Thing)sleeper.CurrentBed(), sleeper, traveler, checkSocialProperness, ignoreOtherReservations: ignoreOtherReservations, guestStatus: guestStatus))
                {
                    return sleeper.CurrentBed();
                }
                for (int index1 = 0; index1 < DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(d => d.IsBed)).OrderBy<ThingDef, float>((Func<ThingDef, float>)(d => d.building.bed_maxBodySize)).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset))).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness))).ToList<ThingDef>().Count; ++index1)
                {
                    ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(d => d.IsBed)).OrderBy<ThingDef, float>((Func<ThingDef, float>)(d => d.building.bed_maxBodySize)).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset))).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness))).ToList<ThingDef>()[index1];
                    if (CageUtility.CanUseBedEver(sleeper, thingDef))
                    {
                        for (int index2 = 0; index2 < 2; ++index2)
                        {
                            Danger maxDanger = index2 == 0 ? Danger.None : Danger.Deadly;
                            Building_Bed bedFor = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler), validator: ((Predicate<Thing>)(b => ((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && CageUtility.IsValidBedFor(b, sleeper, traveler, checkSocialProperness, ignoreOtherReservations: ignoreOtherReservations, guestStatus: guestStatus))));
                            if (bedFor != null && SCP_Startup.IsCage(bedFor.def) && bedFor.GetRoom().Role == SCP_Startup.containmentRoom)
                                return bedFor;
                        }
                    }
                }
            }
            if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null && sleeper.ownership.OwnedBed.GetRoom().Role == SCP_Startup.containmentRoom && CageUtility.IsValidBedFor((Thing)sleeper.ownership.OwnedBed, sleeper, traveler, checkSocialProperness, ignoreOtherReservations: ignoreOtherReservations, guestStatus: guestStatus))
                return sleeper.ownership.OwnedBed;
            for (int dg = 0; dg < 3; dg++)
            {
                Danger maxDanger = dg <= 1 ? Danger.None : Danger.Deadly;
                for (int index = 0; index < DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(d => d.IsBed)).OrderBy<ThingDef, float>((Func<ThingDef, float>)(d => d.building.bed_maxBodySize)).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness))).ToList<ThingDef>().Count; ++index)
                {
                    ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(d => d.IsBed)).OrderBy<ThingDef, float>((Func<ThingDef, float>)(d => d.building.bed_maxBodySize)).ThenByDescending<ThingDef, float>((Func<ThingDef, float>)(d => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness))).ToList<ThingDef>()[index];
                    if (CageUtility.CanUseBedEver(sleeper, thingDef))
                    {
                        Building_Bed bedFor = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.PositionHeld, sleeper.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler), validator: ((Predicate<Thing>)(b =>
                        {
                            if (((Building_Bed)b).Medical /*|| b.Position.GetDangerFor(sleeper, sleeper.MapHeld) > maxDanger*/ || !CageUtility.IsValidBedFor(b, sleeper, traveler, checkSocialProperness, ignoreOtherReservations: ignoreOtherReservations, guestStatus: guestStatus))
                            {
                                return false;
                            }
                            return dg > 0 || !b.Position.GetItems(b.Map).Any<Thing>((Func<Thing, bool>)(thing => thing.def.IsCorpse));
                        })));
                        if (bedFor != null && SCP_Startup.IsCage(bedFor.def) && bedFor.GetRoom().Role == SCP_Startup.containmentRoom)
                        {
                            return bedFor;
                        }
                    }
                }
            }
            return (Building_Bed)null;
        }

    }
}
