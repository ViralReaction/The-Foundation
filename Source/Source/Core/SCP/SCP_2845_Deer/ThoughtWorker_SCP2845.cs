using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class ThoughtWorker_SCP2845 : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(SCPDefOF.SCP_2845_Transmute_Hediff);
            if (firstHediffOfDef == null || firstHediffOfDef.FullyImmune())
                return (ThoughtState)false;
            if ((double)firstHediffOfDef.Severity >= 0.8)
            {
                PawnKindDef kindDef = PawnKindDefOf_SCP.SCP_28451R;
                IntVec3 result;
                CellFinder.TryFindRandomSpawnCellForPawnNear(p.Position, p.Map, out result);
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, p.Map, 10);
                if (GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(kindDef), loc, p.Map) is Pawn pawn)
                {
                    pawn.health.AddHediff(HediffDefOf.Scaria);
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                }
            }
            return (ThoughtState)true;
        }
    }
}