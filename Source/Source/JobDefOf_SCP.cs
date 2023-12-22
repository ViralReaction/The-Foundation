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
    public static class JobDefOf_SCP
    {
        public static JobDef LeaveMapDaylight;
        public static JobDef InduceContaintmentBreak;

        static JobDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf_SCP));
    }
}