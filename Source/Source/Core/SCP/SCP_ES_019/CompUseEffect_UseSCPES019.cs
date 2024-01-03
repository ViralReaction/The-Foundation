using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    internal class CompUseEffect_UseSCPES019 : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            int result = Rand.Range(0, 8);
            if (result <= 3)
                this.MentalBreak(result, usedBy);
            else if (result == 4 || result == 5)
            {
                this.BadThoughts(usedBy);
            }
            else
            {
                if (result != 6)
                    return;
                this.IdeologyCrisis(usedBy);
            }
        }

        private void MentalBreak(int result, Pawn pawn)
        {
            MentalStateDef stateDef = MentalStateDefOf.Berserk;
            foreach (MentalStateDef allDef in DefDatabase<MentalStateDef>.AllDefs)
            {
                if (result == 0 && allDef.defName == "TargetedTantrum")
                {
                    stateDef = allDef;
                    break;
                }
                if (result == 1 && allDef.defName == "Slaughterer")
                {
                    stateDef = allDef;
                    break;
                }
                if (result == 2 && allDef.defName == "SadisticRage")
                {
                    stateDef = allDef;
                    break;
                }
                if (result == 3 && allDef.defName == "TargetedInsultingSpree")
                {
                    stateDef = allDef;
                    break;
                }
            }
            pawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "Cause: SCP-ES-019");
            this.BadThoughts(pawn);
        }

        private void BadThoughts(Pawn pawn)
        {
            Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(SCPDefOf.SCP_ES_019_BadThought);
            pawn.needs.mood.thoughts.memories.TryGainMemory(newThought);
        }

        private void IdeologyCrisis(Pawn pawn)
        {
            if (ModLister.CheckIdeology("Ideoligion certainty"))
            {
                pawn.ideo.Debug_ReduceCertainty(0.5f);
                Messages.Message((string)(pawn.Name.ToString() + "SCPES019".Translate()), MessageTypeDefOf.NegativeEvent);
            }
            this.BadThoughts(pawn);
        }
    }
}
