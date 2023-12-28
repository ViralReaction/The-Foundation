using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class IncidentWorker_SCP529_Josie : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms) => base.CanFireNowSub(parms) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, (Map)parms.target, CellFinder.EdgeRoadChance_Animal);

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            PawnKindDef pawnKind = PawnKindDefOf_SCP.SCP_529_Josie;
            IntVec3 result;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            this.SpawnAnimal(result, target, pawnKind, new Gender?(Gender.Female));
            this.SendStandardLetter((TaggedString)(parms.customLetterLabel ?? (string)"LetterLabelSCP529Join".Translate((NamedArgument)pawnKind.GetLabelPlural()).CapitalizeFirst()), (TaggedString)(parms.customLetterText ?? (string)"LetterSCP529Join".Translate((NamedArgument)pawnKind.GetLabelPlural())), LetterDefOf.PositiveEvent, parms, (LookTargets)new TargetInfo(result, target));
            return true;
        }

        private void SpawnAnimal(IntVec3 location, Map map, PawnKindDef pawnKind, Gender? gender = null)
        {
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(location, map, 12);
            GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(pawnKind), loc, map, Rot4.Random).SetFaction(Faction.OfPlayer);
        }
    }
}