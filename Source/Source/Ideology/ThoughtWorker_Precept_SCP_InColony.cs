using RimWorld;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;
using Verse.Noise;

namespace Foundation
{
    public class ThoughtWorker_Precept_SCP_InColony : ThoughtWorker_Precept
    {
        private const int MinimumTicksBeforeFewAnimals = 900000;
        private string Density;
        //int colonistNum = 0;
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive || p.Faction == null || p.MapHeld == null)
                return ThoughtState.Inactive;
            int colonistNum = 0;
            int scpNum = 0;
            int stageIndex = 1;
            List<Pawn> pawnList = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            for (int index = 0; index < pawnList.Count; index++)
            {
                Pawn pawn = pawnList[index];
                if (pawn.IsFreeColonist && !pawn.IsQuestLodger())
                    ++colonistNum;
                if (pawn.IsSCP())
                    ++scpNum;
            }
            List<Pawn> scpList = p.MapHeld.mapPawns.AllPawnsSpawned;
            for (int index = 0; index < scpList.Count; index++)
            {
                Pawn pawn = scpList[index];
                if (pawn.IsCaptiveOf() && pawn.Faction != Faction.OfPlayer)
                {
                    ++scpNum;
                }
            }
            if (colonistNum < 1)
            {
                if (GenTicks.TicksAbs < 900000)
                {
                    return ThoughtState.Inactive;
                }
                return ThoughtState.ActiveAtStage(0);
            }
            int scpDensity = scpNum / colonistNum;
            Density = scpDensity.ToString();
            for (int index = 0; index < scpDensity; index++)
            {
                ++stageIndex;
                if (stageIndex < 2 && GenTicks.TicksAbs < 900000)
                    return ThoughtState.Inactive;
            }
            if (stageIndex > 6)
                stageIndex = 6;
            return ThoughtState.ActiveAtStage(stageIndex);
        }
        public override string PostProcessDescription(Pawn p, string description) => (string)(base.PostProcessDescription(p, description) + "\n\n" + "CurrentTotalAnomalyPerColonist".Translate() + ": " + Density + "\n" + "MinAnomalyPerColonist".Translate((NamedArgument)4f.ToString("F1")));
        public IEnumerable<NamedArgument> GetDescriptionArgs()
        {
            yield return ((string)("(" + "AnomalyPerColonist".Translate() + ": ") + (object)1f + ")").Named("STAGE1");
            yield return ((string)("(" + "AnomalyPerColonist".Translate() + ": ") + (object)2f + ")").Named("STAGE2");
            yield return ((string)("(" + "AnomalyPerColonist".Translate() + ": ") + (object)6f + ")").Named("STAGE4");
            yield return ((string)("(" + "AnomalyPerColonist".Translate() + ": ") + (object)8f + ")").Named("STAGE5");
        }
    }
}