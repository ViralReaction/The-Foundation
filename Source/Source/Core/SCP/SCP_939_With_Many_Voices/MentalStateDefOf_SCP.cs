using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    [DefOf]
    public class MentalStateDefOf_SCP
    {
        public static MentalStateDef FollowTheVoices;
        public static MentalStateDef SCP_BreachContainment;
        public static MentalStateDef SCP_ContainmentBreaker;

        static MentalStateDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf_SCP));
    }
}