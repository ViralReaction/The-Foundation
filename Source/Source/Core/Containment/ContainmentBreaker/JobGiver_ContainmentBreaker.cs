using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using Foundation.Utilities;

namespace Foundation.Containment
{
    public class JobGiver_ContainmentBreaker : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.MentalState is MentalState_ContainmentBreaker mentalState && mentalState.SlaughteredRecently)
            {
                return (Job)null;
            }
            Pawn animal = ContainmentBreakerMentalStateUtility.FindSCP(pawn);
            if (animal == null || !pawn.CanReach((LocalTargetInfo)(Thing)animal, PathEndMode.Touch, Danger.Deadly))
                return (Job)null;
            Job job = JobMaker.MakeJob(SCPDefOf.InduceContaintmentBreak, (LocalTargetInfo)(Thing)animal);
            job.ignoreDesignations = true;
            return job;
        }
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            Job job = this.TryGiveJob(pawn);
            return job == null ? ThinkResult.NoJob : new ThinkResult(job, (ThinkNode)this);
        }
    }
}