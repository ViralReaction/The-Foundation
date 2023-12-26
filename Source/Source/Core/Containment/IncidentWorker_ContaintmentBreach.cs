using RimWorld;
using SCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;
using Verse;

namespace SCP
{
    internal class IncidentWorker_ContainmentBreach : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            if (target.IsPlayerHome)
            {   

                if (parms.controllerPawn != null && parms.controllerPawn.IsSCP() && !parms.controllerPawn.InMentalState)
                    return true;
                foreach (Pawn pawn in target.mapPawns.AllPawnsSpawned)
                {
                    ThingDef def = pawn.def;
                    if (pawn.IsSCP() && pawn.GetRoom().Role == SCP_Startup.containmentRoom && !parms.controllerPawn.InMentalState)
                        return true;
                }
            }
            return false;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (!this.CanFireNowSub(parms))
            {
                Log.Warning("SCP: Tried to start a containment breach when stuff isn't ready to go.");
                return false;
            }
            Map target = (Map)parms.target;
            List<Pawn> list = new List<Pawn>();
            foreach (Pawn pawn in target.mapPawns.AllPawnsSpawned)
            {
                if (pawn.IsSCP() && pawn.GetRoom().Role == SCP_Startup.containmentRoom && pawn.def.GetModExtension<ContainmentExtension>().containmentTier > 0)
                    list.Add(pawn);
            }
            if (!list.NullOrEmpty<Pawn>())
            {
                int index = Rand.Range(0, list.Count);
                Pawn controllerPawn = list[index];
                if (parms.controllerPawn != null && parms.controllerPawn.IsSCP() && parms.controllerPawn.GetRoom().Role == SCP_Startup.containmentRoom)
                {
                    foreach (Pawn pawn in list)
                    {
                        if (parms.controllerPawn == pawn)
                        {
                            controllerPawn = parms.controllerPawn;
                            break;
                        }
                    }
                }
                List<Pawn> pawnList = new List<Pawn>();
                pawnList.Add(controllerPawn);
                foreach (Pawn thing in list)
                {
                    if (thing != controllerPawn && thing.GetRoom() == controllerPawn.GetRoom() && !thing.InMentalState)
                        pawnList.Add(thing);
                }
                foreach (Pawn pawn in pawnList)
                    if (!pawn.InMentalState)
                    {
                        pawn.mindState.mentalStateHandler.TryStartMentalState(SCP_Startup.containBreachState, forceWake: true, transitionSilently: true);
                        this.SendStandardLetter("LetterLabelContainmentBreach".Translate((NamedArgument)controllerPawn.def.label.CapitalizeFirst()), "LetterContainmentBreach".Translate((NamedArgument)controllerPawn.def.label), LetterDefOf.ThreatSmall, parms, (LookTargets)(Thing)pawnList[0]);
                        Find.TickManager.slower.SignalForceNormalSpeedShort();
                        return true;
                    }
            }
            Log.Error("SCP error: Tried to find SCP to cause containment breach but found none.");
            return false;
        }
    }
}