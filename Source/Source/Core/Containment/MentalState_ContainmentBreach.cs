using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Foundation
{
    internal class MentalState_ContainmentBreach : MentalState
    {
        public Thing target;
        private static List<Thing> candidates = new List<Thing>();
        private int currentTargetPriority = -1;
        private int tickOffsetter;

        public override void MentalStateTick()
        {
            if (!this.target.DestroyedOrNull())
            {
                if (this.target is Pawn && (((Pawn)this.target).Downed || !this.pawn.CanReach((LocalTargetInfo)this.target, PathEndMode.Touch, Danger.Deadly)))
                {
                    this.tickOffsetter = -1;
                    this.currentTargetPriority = -1;
                    this.ChooseNextTarget();
                }
                this.ForceHostileTo(Faction.OfPlayer);
                this.pawn.jobs.TryTakeOrderedJob(this.meleeJob());
                if (this.tickOffsetter >= 30)
                    this.ChooseNextTarget(true);
                else
                    ++this.tickOffsetter;
            }
            else
                this.ChooseNextTarget();
        }

        public override bool ForceHostileTo(Faction f) => f.def.humanlikeFaction || f == Faction.OfMechanoids;

        public override bool ForceHostileTo(Thing t) => (!(t is Pawn pawn) || !pawn.RaceProps.Roamer) && t.Faction != null && this.ForceHostileTo(t.Faction);

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            this.ChooseNextTarget();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Thing>(ref this.target, "target");
        }

        private void ChooseNextTarget(bool isStillTargeting = false)
        {
            MentalState_ContainmentBreach.candidates.Clear();
            if (this.pawn.CanReachMapEdge())
            {
                this.TryEscape();
            }
            else
            {
                this.GetPotentialTargets();
                Thing candidate1 = MentalState_ContainmentBreach.candidates[0];
                Thing thing;
                int num;
                if (!isStillTargeting)
                {
                    thing = MentalState_ContainmentBreach.candidates[0];
                    num = -1;
                }
                else
                {
                    thing = this.target;
                    num = this.currentTargetPriority;
                }
                if (!MentalState_ContainmentBreach.candidates.NullOrEmpty<Thing>())
                {
                    List<Thing> thingList = MentalState_ContainmentBreach.candidates;
                    for (int index = 0; index < thingList.Count; index++)
                    {
                        Thing candidate2 = thingList[index];
                        if (candidate2.HitPoints >= 0 && this.pawn.CanReach((LocalTargetInfo)candidate2.Position, PathEndMode.Touch, Danger.Deadly))
                        {
                            if (this.target is Pawn && !((Pawn)this.target).Downed && candidate2.Faction != this.pawn.Faction)
                            {
                                num = 5;
                                thing = candidate2;
                                break;
                            }
                            if (candidate2.def.IsBuildingArtificial && candidate2.def.IsDoor)
                            {
                                if (candidate2.def.GetModExtension<ContainmentExtension>() == null)
                                {
                                    if (num < 3)
                                    {
                                        num = 3;
                                        thing = candidate2;
                                    }
                                }
                                else if (num < 1)
                                {
                                    num = 1;
                                    thing = candidate2;
                                }
                            }
                            else if (candidate2.def.IsBuildingArtificial)
                            {
                                if (candidate2.def.GetModExtension<ContainmentExtension>() == null)
                                {
                                    if (num < 2)
                                    {
                                        num = 2;
                                        thing = candidate2;
                                    }
                                }
                                else if (num < 0)
                                {
                                    num = 0;
                                    thing = candidate2;
                                }
                            }
                        }
                    }
                    this.target = thing;
                    this.currentTargetPriority = num;
                }
                else
                {
                    if (isStillTargeting)
                        return;
                    this.target = (Thing)null;
                    this.TryEscape();
                }
            }
        }

        private void GetPotentialTargets() => MentalState_ContainmentBreach.candidates = this.pawn.GetRoom().ContainedAndAdjacentThings;

        private void TryEscape()
        {
            if (this.pawn.def.GetModExtension<ContainmentExtension>().willManhuntAfterBreach)
            {
                this.pawn.mindState.mentalStateHandler.Reset();
                this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, transitionSilently: true);
            }
            else
            {
                this.pawn.mindState.mentalStateHandler.Reset();
                this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, transitionSilently: true);
                this.pawn.mindState.exitMapAfterTick = 0;
            }
        }

        private Job meleeJob()
        {
            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, (LocalTargetInfo)this.target);
            job.maxNumMeleeAttacks = 1;
            job.expiryInterval = Rand.Range(420, 900);
            job.attackDoorIfTargetLost = true;
            job.canBashDoors = true;
            job.canBashFences = true;
            return job;
        }
    }
}