using RimWorld;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;
using Verse;
using Foundation.Utilities;
using UnityEngine;

namespace Foundation.Containment
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
                    if (pawn.IsSCP() && pawn.GetRoom().Role == Foundation_Startup.containmentRoom)
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
            IReadOnlyList<Pawn> targetList = target.mapPawns.AllPawnsSpawned;
            for (int index = 0; index < targetList.Count; index++)
            {
                Pawn pawn = targetList[index];
                if (pawn.IsCaptiveOf())
                    list.Add(pawn);
            }
            if (!list.NullOrEmpty<Pawn>())
            {
                int index = Rand.Range(0, list.Count);
                Pawn controllerPawn = list[index];
                if (parms.controllerPawn != null)
                {
                    for (int index2 = 0; index2 < list.Count; index2++)
                    {
                        Pawn pawn = list[index2];
                        if (parms.controllerPawn == pawn)
                        {
                            controllerPawn = parms.controllerPawn;
                            break;
                        }
                    }
                }
                List<Pawn> pawnList = new List<Pawn>();
                pawnList.Add(controllerPawn);
                for (int index3 = 0; index3 < list.Count; index3++)
                {
                    Pawn thing = list[index3];
                    if (thing != controllerPawn && thing.GetRoom() == controllerPawn.GetRoom())
                        pawnList.Add(thing);
                }
                for (int index4 = 0; index4 < pawnList.Count; index4++)
                {
                    Pawn pawn = pawnList[index4];
                    pawn.mindState.mentalStateHandler.TryStartMentalState(SCPDefOf.Foundation_BreachContainment, forceWake: true, transitionSilently: true);
                    FoundationComponent.ContainmentBreakDict.SetOrAdd(pawn, 0);
                }
                this.SendStandardLetter("LetterLabelContainmentBreach".Translate((NamedArgument)controllerPawn.def.label.CapitalizeFirst()), "LetterContainmentBreach".Translate((NamedArgument)controllerPawn.def.label), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)pawnList[0]);
                Find.TickManager.slower.SignalForceNormalSpeedShort();
                return true;
            }
            Log.Error("SCP error: Tried to find SCP to cause containment breach but found none.");
            return false;
        }
    }
}