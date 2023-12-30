using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Comps
{
    public class CompProperties_ShyGuy : CompProperties
    {
        public int tickInterval = 300;
        public string shyGraphic;
        public int radius = 10;

        public CompProperties_ShyGuy() => this.compClass = typeof(CompShyGuy);
    }
}
