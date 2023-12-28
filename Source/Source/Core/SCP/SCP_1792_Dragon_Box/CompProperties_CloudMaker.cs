using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Comps
{
    public class CompProperties_CloudMaker : CompProperties
    {
        public EffecterDef sourceStreamEffect;
        public float cloudRadius;
        public FleckDef cloudFleck;
        public float fleckScale = 1f;
        public float fleckSpawnMTB;
        public int checkRadius = 1;
        public int tickerInterval = 300;

        public CompProperties_CloudMaker() => this.compClass = typeof(CompCloudMaker);
    }
}
