using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace SCP
{
    public class MentalState_Voices : MentalState
    {
        public IntVec3 voicesHeardFrom;

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (!(this.pawn.Position == this.voicesHeardFrom))
                return;
            this.RecoverFromState();
        }

        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Quiet;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<IntVec3>(ref this.voicesHeardFrom, "voicesHeardFrom");
        }
    }
}