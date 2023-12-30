using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Foundation.Containment
{
    public class MentalState_ContainmentBreaker : MentalState
    {
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
        private int lastSlaughterTicks = -1;
        //private int animalsSlaughtered;
        //private const int NoAnimalToSlaughterCheckInterval = 600;
        //private const int MinTicksBetweenSlaughter = 3750;
        //private const int MaxAnimalsSlaughtered = 4;

        public bool SlaughteredRecently => this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;

        protected override bool CanEndBeforeMaxDurationNow => this.lastSlaughterTicks >= 0;

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            Pawn animal = ContainmentBreakerMentalStateUtility.FindSCP(this.pawn);
            if (animal != null)
            {
                this.RecoverFromState();
            }
            if (!this.pawn.IsHashIntervalTick(600) || animal != null || this.pawn.CurJob != null && this.pawn.CurJob.def == SCPDefOf.InduceContaintmentBreak)
                return;
            this.RecoverFromState();
        }

    }
}