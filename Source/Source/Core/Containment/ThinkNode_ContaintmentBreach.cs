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
using Foundation.Utilities;

namespace Foundation.Containment
{
    internal class ThinkNode_ContainmentBreach : ThinkNode_ConditionalMentalStates
    {
#pragma warning disable CS0169 // The field 'ThinkNode_ContainmentBreach.tagToGive' is never used
        private JobTag tagToGive;
#pragma warning restore CS0169 // The field 'ThinkNode_ContainmentBreach.tagToGive' is never used
        public new List<MentalStateDef> states;
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalMentalStates conditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
            conditionalMentalStates.states = this.states;
            return (ThinkNode)conditionalMentalStates;
        }
        protected override bool Satisfied(Pawn pawn)
        {
            if (!pawn.IsCaptiveOf()  || pawn.InMentalState)
                return !pawn.CanReachMapEdge();
            Map map = pawn.Map;
            if (!pawn.InMentalState)
            {
                Find.Storyteller.TryFire(new FiringIncident(SCPDefOf.SCP_Incident_ContainBreach, (StorytellerComp)null, new IncidentParms()
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