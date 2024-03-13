using Foundation.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class IncidentWorker_682 : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
                return false;
            Map target = (Map)parms.target;
            return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, target.Tile, out PawnKindDef _) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        }
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef animalKind = PawnKindDefOf_SCP.SCP_682_Reptile;
            if (animalKind == null && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, target.Tile, out animalKind) || ManhunterPackIncidentUtility.GetAnimalsCount(animalKind, parms.points) == 0)
                return false;
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points * 1f, 1);
            Rot4 rot = Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            for (int index = 0; index < animals.Count; ++index)
            {
                Pawn newThing = animals[index];
                QuestUtility.AddQuestTag((object)GenSpawn.Spawn((Thing)newThing, CellFinder.RandomClosewalkCellNear(result, target, 10), target, rot), parms.questTag);
                newThing.health.AddHediff(HediffDefOf.Scaria);
                newThing.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
                //newThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
            }
            this.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate((NamedArgument)animalKind.GetLabelPlural()), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
            return true;
        }
    }
}
