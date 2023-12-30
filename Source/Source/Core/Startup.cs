using RimWorld;
using Foundation;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Diagnostics;
using Verse;

namespace Foundation
{
    [StaticConstructorOnStartup]
    public class SCP_Startup
    {
        public static string version = "v0.0.1";
        public static RoomRoleDef containmentRoom;
        public static MentalStateDef containBreachState;
        public static IncidentDef containBreachIncident;

        static SCP_Startup()
        {
            foreach (MentalStateDef allDef in DefDatabase<MentalStateDef>.AllDefs)
            {
                if (allDef.defName == "SCP_BreachContainment")
                {
                    SCP_Startup.containBreachState = allDef;
                    break;
                }
            }
            foreach (RoomRoleDef allDef in DefDatabase<RoomRoleDef>.AllDefs)
            {
                if (allDef.defName == "SCP_ContainmentRoom")
                {
                    SCP_Startup.containmentRoom = allDef;
                    break;
                }
            }
            foreach (IncidentDef allDef in DefDatabase<IncidentDef>.AllDefs)
            {
                if (allDef.defName == "SCP_Incident_ContainBreach")
                {
                    SCP_Startup.containBreachIncident = allDef;
                    break;
                }
            }
            Log.Message("Level 5 security credentials accepted. Welcome back to the Foundation, [REDACTED]\nSecure Contain Protect [" + SCP_Startup.version + "] loaded.");
        }

        public static bool isInContainCrate(Thing thing, bool advanced = false)
        {
            if (thing != null && !thing.IsInAnyStorage())
                return false;
            Building thing1 = thing != null ? thing.PositionHeld.GetFirstBuilding(thing.Map) : (Building)null;
            if (thing1 == null)
                return false;
            int num;
            if (SCP_Startup.isAnyContainmentStorage(thing1.def.defName))
            {
                CompPowerTrader comp = thing1.TryGetComp<CompPowerTrader>();
                num = comp != null ? (comp.PowerOn ? 1 : 0) : 0;
            }
            else
                num = 0;
            return num != 0 || thing.def.HasModExtension<ContainmentExtension>() && thing.def.GetModExtension<ContainmentExtension>().containmentTier >= 1;
        }
 
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool IsSCP(this Pawn p)
        //{
        //    if (p.def.GetModExtension<ContainmentExtension>() != null)
        //        return true;
        //    return false;
        //}

        //public static bool IsCage(ThingDef thing) => thing.GetModExtension<CageExtension>().isCage = true;
        public static bool isCage(string defName)
        {
            switch (defName)
            {
                case "Test_AnimalSleepingSpot":
                    return true;
                default:
                    return false;
            }
        }
        public static bool isAnyContainmentStorage(string defName)
        {
            switch (defName)
            {
                case "SCR_Shelf":
                    return true;
                case "SCR_AdvShelf":
                    return true;
                case "SCR_ContainPedestal":
                    return true;
                case "SCR_AdvContainPedestal":
                    return true;
                default:
                    return false;
            }
        }
    }
}