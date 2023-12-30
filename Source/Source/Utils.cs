using RimWorld;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static bool IsSCP(this Pawn pawn)
        {
            if (pawn.def.GetModExtension<ContainmentExtension>() != null)
                return true;
            return false;
        }
        public static bool IsSCP(this ThingDef thingdef)
        {
            if (thingdef.GetModExtension<ContainmentExtension>() != null)
                return true;
            return false;
        }
        public static bool IsCaptiveOf(this Pawn pawn)
        {
            if (!pawn.IsSCP())
                return false;
            if (pawn.GetRoom().Role == SCP_Startup.containmentRoom)
                return true;
            return false;
        }
        public static bool IsCage(this ThingDef thing)
        {
            if (thing.GetModExtension<CageExtension>().isCage != null)
                return true;
            return false;
        }
    }
}