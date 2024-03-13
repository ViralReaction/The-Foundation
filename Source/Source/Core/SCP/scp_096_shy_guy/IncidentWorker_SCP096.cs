using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;
using UnityEngine;

namespace Foundation
{
    internal class IncidentWorker_SCP096 : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, target.Tile, out PawnKindDef _) && RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 _, target, CellFinder.EdgeRoadChance_Animal);
        }


        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            IntVec3 cell;
            if (!this.TryFindEntryCell(target, out cell))
                return false;
            PawnKindDef kindDef = PawnKindDefOf_SCP.SCP_096_Shy_Guy;
            //int num1 = Mathf.Clamp(GenMath.RoundRandom(StorytellerUtility.DefaultThreatPointsNow((IIncidentTarget)target) / kindDef.combatPower), 2, Rand.RangeInclusive(3, 6));
            //int num2 = Rand.RangeInclusive(90000, 150000);
            IntVec3 result = IntVec3.Invalid;
            if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(cell, target, 10f, out result))
                result = IntVec3.Invalid;
            Pawn newThing = (Pawn)null;
            Thing thing = GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(kindDef), CellFinder.RandomClosewalkCellNear(result, target, 10), target);
            //for (int index = 0; index < num1; ++index)
            //{
            //    IntVec3 loc = CellFinder.RandomClosewalkCellNear(cell, target, 10);
            //    newThing = PawnGenerator.GeneratePawn(kindDef);
            //    GenSpawn.Spawn((Thing)newThing, loc, target, Rot4.Random);
            //    newThing.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
            //    if (result.IsValid)
            //        newThing.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(result, target, 10);
            //}
            this.SendStandardLetter("LetterLabelThrumboPasses".Translate((NamedArgument)kindDef.label).CapitalizeFirst(), "LetterThrumboPasses".Translate((NamedArgument)kindDef.label), LetterDefOf.PositiveEvent, parms, (LookTargets)(Thing)newThing);
            return true;
        }
        private bool TryFindEntryCell(Map map, out IntVec3 cell) => RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
    }
}