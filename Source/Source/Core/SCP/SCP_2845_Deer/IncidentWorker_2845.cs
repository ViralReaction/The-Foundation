using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class IncidentWorker_SCP2845 : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, target.Tile, out PawnKindDef _) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef kindDef = PawnKindDefOf_SCP.SCP_2845_Deer;
            PawnKindDef animalKind = PawnKindDefOf_SCP.SCP_28451R;
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points * 3f, parms.pawnCount);
            Rot4 rot = Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            for (int index = 0; index < animals.Count; index++)
            {
                Pawn newThing = animals[index];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, target, 10);
                QuestUtility.AddQuestTag((object)GenSpawn.Spawn((Thing)newThing, loc, target, rot), parms.questTag);
                newThing.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                newThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(100000, 200000);
            }
            Thing thing = GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(kindDef), CellFinder.RandomClosewalkCellNear(result, target, 10), target);
            if (thing is Pawn pawn)
                pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(100000, 200000);
            QuestUtility.AddQuestTag((object)thing, parms.questTag);
            this.SendStandardLetter("LetterLabelSCP2845Attack".Translate((NamedArgument)animalKind.label.CapitalizeFirst()), "LetterSCP2845Attack".Translate((NamedArgument)kindDef.label), LetterDefOf.ThreatBig, parms, (LookTargets)(Thing)animals[0]);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
            return true;
        }
    }
}
