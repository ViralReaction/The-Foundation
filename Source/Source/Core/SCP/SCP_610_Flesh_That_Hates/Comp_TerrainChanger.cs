using Foundation.Comps;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using static HarmonyLib.Code;
using Foundation.Utilities;

namespace Foundation
{
    public class Comp_TerrainChanger : ThingComp
    {
        public CompProperties_TerrainChanger Props => (CompProperties_TerrainChanger)this.props;
        private IntVec3 nextChangeCell = IntVec3.Invalid;
        private int terrainChangeProgressTicks;
        private TerrainDef terrain = SCPDefOf.SCP_610_Fleshy_Soil;
        public override void CompTick()
        {
            base.CompTick();
            ++this.terrainChangeProgressTicks;
            if (this.terrainChangeProgressTicks < this.Props.fleshHateIntervalTicks)
                return;
            this.terrainChangeProgressTicks = 0;
            this.ChangeCell();
        }

        private void ChangeCell(bool silent = false)
        {
            Thing thing = this.parent as Thing;
            int num1 = GenRadial.NumCellsInRadius(this.Props.radius);
            int num2 = 0;
            switch (new IntRange(0, 3).RandomInRange)
            {
                case 0:
                    terrain = SCPDefOf.SCP_610_Fleshy_Soil;
                    break;
                case 1:
                    terrain = SCPDefOf.SCP_610_Fleshy_Gravel;
                    break;
                case 2:
                    terrain = SCPDefOf.SCP_610_Fleshy_Gravel;
                    break;
                case 4:
                    terrain = SCPDefOf.SCP_610_Fleshy_Gravel;
                    break;
            }
            for (int index = 0; index < num1; ++index)
            {
                IntVec3 cell = thing.Position + GenRadial.RadialPattern[index];
                if (cell.InBounds(this.parent.Map) && NextChangeCellValidator(cell))
                {
                    thing.Map.terrainGrid.SetTerrain(cell, terrain);
                    ++num2;
                    if (num2 >= this.Props.cellsToChange)
                        break;
                }
            }
            if (silent)
                return;
        }
        bool NextChangeCellValidator(IntVec3 cell) => cell.InBounds(this.parent.Map) && (cell.GetTerrain(this.parent.Map) != SCPDefOf.SCP_610_Fleshy_Soil && cell.GetTerrain(this.parent.Map) != SCPDefOf.SCP_610_Fleshy_Gravel);

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
                base.PostSpawnSetup(respawningAfterLoad);
                if (respawningAfterLoad)
                    return;
                this.terrainChangeProgressTicks = this.Props.fleshHateIntervalTicks;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look<IntVec3>(ref this.nextChangeCell, "nextChangeCell", IntVec3.Invalid);
            Scribe_Values.Look<int>(ref this.terrainChangeProgressTicks, "terrainChangeProgressTicks");
        }
    }
}