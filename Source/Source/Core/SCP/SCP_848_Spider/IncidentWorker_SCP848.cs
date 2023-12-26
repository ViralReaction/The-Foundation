using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{ 
    internal class IncidentWorker_SCP848 : IncidentWorker
    {
        private static readonly IntRange CountRange = new IntRange(2, 6);
        private const int MinRoomCells = 64;
        private const int SpawnRadius = 5;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
                return false;
            Map target = (Map)parms.target;
            return target.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow && this.TryFindRootCell(target, out IntVec3 _);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 cell;
            if (!this.TryFindRootCell(map, out cell))
                return false;
            Thing thing1 = (Thing)null;
            int randomInRange = IncidentWorker_SCP848.CountRange.RandomInRange;
            ThingDef def = ThingDefOf_SCP.SCP_848R;
            IntVec3 result;
            for (int index = 0; index < randomInRange && CellFinder.TryRandomClosewalkCellNear(cell, map, 6, out result, (Predicate<IntVec3>)(x => this.CanSpawnAt(x, map))); ++index)
            {
                result.GetPlant(map)?.Destroy(DestroyMode.Vanish);
                Thing thing2 = GenSpawn.Spawn(def, result, map);
                if (thing1 == null)
                    thing1 = thing2;
            }
            if (thing1 == null)
                return false;
            this.SendStandardLetter("LetterLabelSCP848".Translate((NamedArgument)def.label.CapitalizeFirst()), "LetterSCP848".Translate((NamedArgument)def.label), LetterDefOf.NeutralEvent, parms, (LookTargets)thing1);
            return true;
        }

        private bool TryFindRootCell(Map map, out IntVec3 cell) => CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (Predicate<IntVec3>)(x => this.CanSpawnAt(x, map) && x.GetRoom(map).CellCount >= 64), map, out cell);

        private bool CanSpawnAt(IntVec3 c, Map map)
        {
            if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map))
                return false;
            Plant plant = c.GetPlant(map);
            if (plant != null && (double)plant.def.plant.growDays > 10.0)
                return false;
            List<Thing> thingList = c.GetThingList(map);
            for (int index = 0; index < thingList.Count; index++)
            {
                if (thingList[index].def == ThingDefOf.Plant_Ambrosia)
                    return false;
            }
            return true;
        }
    }
}