using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using SCP;

namespace SCP
{
    internal class ThinkNode_ContainmentBreach : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.def.GetModExtension<ContainmentExtension>() == null || pawn.GetRoom().Role != SCP_Startup.containmentRoom)
                return !pawn.CanReachMapEdge();
            Map map = pawn.Map;
            Find.Storyteller.TryFire(new FiringIncident(SCP_Startup.containBreachIncident, (StorytellerComp)null, new IncidentParms()
            {
                target = (IIncidentTarget)map,
                controllerPawn = pawn,
                forced = true
            }));
            return false;
        }
    }
}