﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation;

namespace Foundation
{
    internal class IncidentWorker_SCP2845 : IncidentWorker_AggressiveAnimals
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef kindDef = PawnKindDefOf_SCP.Foundation_Deer;
            PawnKindDef animalKind = PawnKindDefOf_SCP.Foundation_28451R;
            if (animalKind == null && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, target, out animalKind) || AggressiveAnimalIncidentUtility.GetAnimalsCount(animalKind, parms.points) == 0)
                return false;
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points * 1f, parms.pawnCount);
            Rot4 rot = Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            for (int index = -1; index < animals.Count; ++index)
            {
                if (index == -1)
                {
                    List<Pawn> deer = AggressiveAnimalIncidentUtility.GenerateAnimals(kindDef, target.Tile, parms.points * 1f, 1);
                    Pawn deerThing = deer[0];
                    deerThing.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
                    deerThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
                }
                Pawn newThing = animals[index];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, target, 10);
                QuestUtility.AddQuestTag((object)GenSpawn.Spawn((Thing)newThing, loc, target, rot), parms.questTag);
                if (!ModsConfig.AnomalyActive || this.def != IncidentDefOf.FrenziedAnimals)
                    newThing.health.AddHediff(HediffDefOf.Scaria);
                newThing.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
                newThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
            }
            if (ModsConfig.AnomalyActive)
            {
                if (this.def == IncidentDefOf.FrenziedAnimals)
                    this.SendStandardLetter("FrenziedAnimalsLabel".Translate(), "FrenziedAnimalsText".Translate((NamedArgument)animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
                else
                    this.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate((NamedArgument)animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
            }
            else
                this.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate((NamedArgument)animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            return true;
        }
    }
}
