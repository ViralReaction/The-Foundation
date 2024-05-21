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
    public static class SCPDefOf
    {
        public static DamageDef Foundation_106_Oldman_Scratch;
        public static EffecterDef HumeShield_Break;
        public static FleckDef ExplosionHumeFlash;
        public static HediffDef Foundation_2687_Poison; // Plastic Organ Poisoning
        public static HediffDef Foundation_939_Breath_Hediff; // With Many Voices Amneisa Breath
        public static HediffDef Foundation_2845_Transmute_Hediff; // The Deer Transmute Aura
        public static HediffDef GutWorms; // Pope Soap Cleanining
        [MayRequireIdeology]
        public static HistoryEventDef Foundation_Died;
        public static JobDef LeaveMapDaylight;
        public static MentalStateDef FollowTheVoices;
        [MayRequireIdeology]
        public static PreceptDef Foundation_Tools;
        //public static RoomRoleDef Foundation_ContainmentRoom;
        public static ThoughtDef Foundation_113_BadThought;
        public static ThoughtDef Foundation_Infected_SCP939;

        static SCPDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(SCPDefOf));
    }
}