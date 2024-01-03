using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Foundation.Utilities;

namespace Foundation.Containment
{
    public class MentalState_ContainmentBreaker : MentalState
    {
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
        private int lastContainmentBreachTicks = -1;
        private int anomaliesBreach;
        //private const int NoAnimalToSlaughterCheckInterval = 600;
        //private const int MinTicksBetweenSlaughter = 3750;
        //private const int MaxAnimalsSlaughtered = 4;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.anomaliesBreach, "anomaliesBreach");
        }
        public bool SlaughteredRecently => this.lastContainmentBreachTicks >= 0 && Find.TickManager.TicksGame - this.lastContainmentBreachTicks < 3750;

        protected override bool CanEndBeforeMaxDurationNow => this.lastContainmentBreachTicks >= 0;

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (!this.pawn.IsHashIntervalTick(600) || this.pawn.CurJob != null && this.pawn.CurJob.def == SCPDefOf.InduceContaintmentBreak || ContainmentBreakerMentalStateUtility.FindSCP(this.pawn) != null)
                return;
            this.RecoverFromState();
        }
        public override void Notify_SlaughteredAnimal()
        {
            this.lastContainmentBreachTicks = Find.TickManager.TicksGame;
            ++this.anomaliesBreach;
            if (this.anomaliesBreach < 2)
                return;
            this.RecoverFromState();
        }

    }
}