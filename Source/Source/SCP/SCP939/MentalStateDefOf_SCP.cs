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
    public class MentalStateDefOf_SCP
    {
        public static MentalStateDef FollowTheVoices;

        static MentalStateDefOf_SCP() => DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf_SCP));
    }
}