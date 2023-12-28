using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using Foundation;
using UnityEngine.Assertions.Must;

namespace Foundation
{
    internal class ThinkNode_ContainmentBreach : ThinkNode_ConditionalMentalStates
    {
        private JobTag tagToGive;
        public List<MentalStateDef> states;
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalMentalStates conditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
            conditionalMentalStates.states = this.states;
            return (ThinkNode)conditionalMentalStates;
        }
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.def.GetModExtension<ContainmentExtension>() == null || pawn.GetRoom().Role != SCP_Startup.containmentRoom || pawn.InMentalState)
                return !pawn.CanReachMapEdge();
            Map map = pawn.Map;
            if (!pawn.InMentalState)
            {
                Find.Storyteller.TryFire(new FiringIncident(SCP_Startup.containBreachIncident, (StorytellerComp)null, new IncidentParms()
                {
                    target = (IIncidentTarget)map,
                    controllerPawn = pawn,
                    forced = true
                }));
            }
            return false;
        }
    }
}