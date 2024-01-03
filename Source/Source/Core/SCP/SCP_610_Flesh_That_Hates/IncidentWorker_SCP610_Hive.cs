using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Foundation.Utilities;

namespace Foundation
{
    public class IncidentWorker_SCP610_Hive : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms) => ((Map)parms.target).listerThings.ThingsOfDef(ThingDefOf_SCP.SCP_610Hive_Infestation).Count <= 0;

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<TargetInfo> targetInfoList = new List<TargetInfo>();
            ThingDef hivePartDef = this.def.mechClusterBuilding;
            IntVec3 hiveDropLocation = IncidentWorker_SCP610_Hive.FindHiveDropLocation(map, (Predicate<IntVec3>)(spot => CanPlaceAt(spot)));
            if (hiveDropLocation == IntVec3.Invalid)
                return false;
            GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf_SCP.SCP_610Hive_Infestation), hiveDropLocation, map, WipeMode.FullRefund);
            targetInfoList.Add(new TargetInfo(hiveDropLocation, map));
            this.SendStandardLetter(parms, (LookTargets)targetInfoList);
            return true;

            bool CanPlaceAt(IntVec3 loc)
            {
                CellRect cellRect = GenAdj.OccupiedRect(loc, Rot4.North, hivePartDef.Size);
                if (loc.Fogged(map) || !cellRect.InBounds(map) || !DropCellFinder.SkyfallerCanLandAt(loc, map, hivePartDef.Size))
                    return false;
                foreach (IntVec3 c in cellRect)
                {
                    RoofDef roof = c.GetRoof(map);
                    if (roof != null && roof.isNatural)
                        return false;
                }
                return GenConstruct.CanBuildOnTerrain((BuildableDef)hivePartDef, loc, map, Rot4.North);
            }
        }

        private static IntVec3 FindHiveDropLocation(Map map, Predicate<IntVec3> validator)
        {
            for (int index = 0; index < 200; ++index)
            {
                IntVec3 siegePositionFrom = RCellFinder.FindSiegePositionFrom(DropCellFinder.FindRaidDropCenterDistant(map, true), map, true);
                if (validator(siegePositionFrom))
                    return siegePositionFrom;
            }
            return IntVec3.Invalid;
        }
    }
}
