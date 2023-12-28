using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Comps
{
    // Created due to how CompEmptyGraphics works. Checks opposite of CompEmpty, so SCP_1695 can work nicely
    public class CompProperties_TerrainChanger : CompProperties
    {
        public float radius = 5.9f;
        public int fleshHateIntervalTicks = 60000;
        public int cellsToChange = 6;

        public CompProperties_TerrainChanger() => this.compClass = typeof(Comp_TerrainChanger);
    }
}