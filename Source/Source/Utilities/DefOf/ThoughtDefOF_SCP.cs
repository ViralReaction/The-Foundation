using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    [DefOf]
    public static class ThoughtDefOf_SCP
    {
        public static ThoughtDef SCP_113_BadThought;
        public static ThoughtDef SCP_Infected_SCP939;
        public static ThoughtDef SCP_Infected_SCP1797_Flu;
        public static ThoughtDef SCP_ES_019_BadThought;

        static ThoughtDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf_SCP));
    }
}
