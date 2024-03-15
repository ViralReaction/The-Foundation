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
    public class IncidentWorker_682 : IncidentWorker_AggressiveAnimals
    {
        private const float PointsFactor = 1f;
        private const int AnimalsStayDurationMin = 60000;
        private const int AnimalsStayDurationMax = 120000;
        //protected override bool CanFireNowSub(IncidentParms parms)
        //{
        //    if (!base.CanFireNowSub(parms))
        //        return false;
        //    Map target = (Map)parms.target;
        //    return AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, target, out PawnKindDef _) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        //}
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef animalKind = PawnKindDefOf_SCP.SCP_682_Reptile;
            if (animalKind == null && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, target, out animalKind) || AggressiveAnimalIncidentUtility.GetAnimalsCount(animalKind, parms.points) == 0)
                return false;
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points * 1f, parms.pawnCount);
            Rot4 rot = Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            for (int index = 0; index < animals.Count; ++index)
            {
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