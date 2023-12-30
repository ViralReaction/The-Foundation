using Foundation.Comps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class CompProperties_OldMan : CompProperties
    {
        public int tickInterval = 300;
        public int jumpRange = 10;
        public int directAttackRange = 2;
        public CompProperties_OldMan() => this.compClass = typeof(Comp_OldMan);
    }
}
