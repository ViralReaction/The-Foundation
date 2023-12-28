using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Foundation.Containment
{
    internal class JobDriver_ContainmentBreaker : JobDriver
    {
        public const int SlaughterDuration = 180;

        protected Pawn Victim => (Pawn)this.job.targetA.Thing;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            JobDriver_ContainmentBreaker f = this;
            f.EndOnDespawnedOrNull(TargetIndex.A);
            f.FailOnDowned(TargetIndex.A);
            f.FailOnAggroMentalState<JobDriver_ContainmentBreaker>(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.WaitWith(TargetIndex.A, 180, true);
            yield return new Toil
            {
                initAction = delegate ()
                {
                    var target = this.Victim;
                    if (target.InMentalState)
                        return;
                    else
                    target.mindState.mentalStateHandler.TryStartMentalState(SCP_Startup.containBreachState, forceWake: true, transitionSilently: true);
                }
            };
        }
    }
}
