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
    public static class SCPDefOF
    {
        public static HediffDef SCP_2687_Poison; // Plastic Organ Poisoning
        public static HediffDef SCP_939_Breath_Hediff; // With Many Voices Amneisa Breath
        public static HediffDef SCP_2845_Transmute_Hediff; // The Deer Transmute Aura
        public static HediffDef SCP_1797_Flu; // Kitten Flu
        public static HediffDef GutWorms; // Pope Soap Cleanining
        public static HediffDef SCP_TranqHediff;
        public static HediffDef SCP_30331R;
        public static HediffDef SCP_30332R;
        public static HediffDef SCP_610_Infection;
        [MayRequireIdeology]
        public static HistoryEventDef SCP_Died;
        public static JobDef LeaveMapDaylight;
        public static JobDef InduceContaintmentBreak;
        public static JobDef Play_SCP_1762R;
        [MayRequireIdeology]
        public static PreceptDef SCP_Tools;
        public static TerrainDef SCP_610_Fleshy_Gravel;
        public static TerrainDef SCP_610_Fleshy_Soil;
        public static TerrainDef SCP_610_EvilWater_Shallow;
        public static TerrainDef SCP_610_EvilWater_Deep;
        public static ThoughtDef SCP_113_BadThought;
        public static ThoughtDef SCP_Infected_SCP939;
        public static ThoughtDef SCP_Infected_SCP1797_Flu;
        public static ThoughtDef SCP_ES_019_BadThought;
        public static ThoughtDef SCP_1695_BadThought;

        static SCPDefOF() => DefOfHelper.EnsureInitializedInCtor(typeof(SCPDefOF));
    }
}