using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Containment
{
    internal class RoomRoleWorker_ContainmentRoom : RoomRoleWorker
    {
        private static ThingDef zoneSmall = ThingDefOf_SCP.Containment_Zone_Small;
        private static ThingDef zoneMedium = ThingDefOf_SCP.Containment_Zone_Medium;
        private static ThingDef zoneLarge = ThingDefOf_SCP.Containment_Zone_Large;

        public override float GetScore(Room room)
        {
            int num = 0;
            List<Thing> thingList = room.ContainedAndAdjacentThings;
            for (int index = 0; index < thingList.Count; index++)
            {
                Thing andAdjacentThing = thingList[index];
                if (andAdjacentThing.def == RoomRoleWorker_ContainmentRoom.zoneLarge || andAdjacentThing.def == RoomRoleWorker_ContainmentRoom.zoneMedium || andAdjacentThing.def == RoomRoleWorker_ContainmentRoom.zoneSmall)
                    ++num;
            }
            return 30f * (float)num;
        }
    }
}