using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class RoomRoleWorker_ContainmentRoom : RoomRoleWorker
    {
        private static ThingDef containSign = ThingDef.Named("SCP_ContainRoomMarker");

        public override float GetScore(Room room)
        {
            int num = 0;
            foreach (Thing andAdjacentThing in room.ContainedAndAdjacentThings)
            {
                if (andAdjacentThing.def == RoomRoleWorker_ContainmentRoom.containSign)
                    ++num;
            }
            return 30f * (float)num;
        }
    }
}