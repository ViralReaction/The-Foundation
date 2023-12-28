using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Comps
{
    public class CompProperties_GiveHediffSeverity : CompProperties
    {
        public HediffDef hediff;
        public int range = 10;
        public float severityTick = 1.0f;
        public int tickInterval = 300;
        public List<string> defNamesImmune = null;
        public bool psychicCheck = false;

        public CompProperties_GiveHediffSeverity() => this.compClass = typeof(CompGiveHediff);
    }
}