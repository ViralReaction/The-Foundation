using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP
{
    [DefOf]
    internal class IdeologyDefOf_SCP
    {
        [MayRequireIdeology]
        public static HistoryEventDef SCP_Died;
        [MayRequireIdeology]
        public static PreceptDef SCP_Tools;

        static IdeologyDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(IdeologyDefOf_SCP));
    }
}

