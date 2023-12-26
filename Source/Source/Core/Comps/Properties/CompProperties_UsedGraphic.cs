using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP.Comps
{
    // Created due to how CompEmptyGraphics works. Checks opposite of CompEmpty, so SCP_1695 can work nicely
    internal class CompProperties_UsedGraphic : CompProperties
    {
        public GraphicData graphicData;

        public CompProperties_UsedGraphic() => this.compClass = typeof(CompUsedGraphic);
    }
}