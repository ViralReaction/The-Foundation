using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class CompProperties_Voices : CompProperties
    {
        public int voiceRange;
        public int directAttackRange;
        public int startPounceOnLuredTargetRange;

        public CompProperties_Voices() => this.compClass = typeof(CompVoices);
    }
}