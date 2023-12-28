using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class IncidentWorker_SCP610_Local : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            return !(target.Biome.defName != "SCP_610_Biome") && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, target.Tile, out PawnKindDef _) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef animalKind = PawnKindDefOf_SCP.SCP_610A;
            //PawnKindDef.Named("SCR_SCP610B");
            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            List<Pawn> animals = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, target.Tile, parms.points, parms.pawnCount);
            Rot4.FromAngleFlat((target.Center - result).AngleFlat);
            if (animals.Count <= 0)
                GenSpawn.Spawn(ThingDefOf_SCP.SCP_610A, target.Center - result, target);
            for (int index = 0; index < animals.Count; ++index)
            {
                Pawn pawn = animals[index];
                CellFinder.RandomClosewalkCellNear(result, target, 10);
            }
            this.SendStandardLetter("LetterLabelSCP610Local".Translate((NamedArgument)animalKind.label.CapitalizeFirst()), "LetterSCP610Local".Translate((NamedArgument)animalKind.label), LetterDefOf.NegativeEvent, parms, (LookTargets)(Thing)animals[0]);
            return true;
        }
    }
}