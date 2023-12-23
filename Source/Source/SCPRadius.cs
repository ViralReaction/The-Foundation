using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public class SCPRadius : Def
    {
        public static List<Pawn> GetPawnsAround(IntVec3 center, int radius, Map map)
        {
            List<Pawn> pawnList = new List<Pawn>();

            int numCells = GenRadial.NumCellsInRadius(radius);
            var thingGrid = map.thingGrid;

            for (int i = 0; i < numCells; i++)
            {
                var c = center + GenRadial.RadialPattern[i];
                if (c.InBounds(map) == false) continue;

                var things = thingGrid.ThingsListAtFast(c);

                for (int j = 0; j < things.Count; j++)
                {
                    if (things[j].def.category == ThingCategory.Pawn)
                    {
                        pawnList.Add(things[j] as Pawn);
                    }
                }
            }
            return pawnList;
        }
    }
}
