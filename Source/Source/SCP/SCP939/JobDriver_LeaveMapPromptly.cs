using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;

namespace SCP
{
    public class JobDriver_LeaveMapPromptly : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => true;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            yield return new Toil()
            {
                initAction = (Action)(() => ((GameCondition_SCP939)this.pawn.Map.GameConditionManager.ActiveConditions.Find((Predicate<GameCondition>)(x => x is GameCondition_SCP939))).AddToRegistry(this.pawn))
            };
        }
    }
}