using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Foundation.Comps
{
    public class CompShyGuy : ThingComp
    {
        public int tickCounter = 0;
        private Pawn targetHunted;
        public Pawn shyGuy => this.parent as Pawn;
        private CompProperties_ShyGuy Props => (CompProperties_ShyGuy)this.props;
        public override void CompTick()
        {
            base.CompTick();
            tickCounter++;
            if (tickCounter > Props.tickInterval)
            {
                if (!shyGuy.Downed && !shyGuy.InMentalState && (shyGuy.CurJobDef != JobDefOf.PredatorHunt || shyGuy.CurJobDef != JobDefOf.AttackMelee))
                {
                    List<Pawn> targetHuntedList = SCPRadius.GetPawnsAround(shyGuy.Position, Props.radius, shyGuy.MapHeld);
                    for (int index = 0; index < targetHuntedList.Count; index++)
                    {
                        this.targetHunted = targetHuntedList[index];
                        if (this.targetHunted != shyGuy && CanSeeShyGuy(this.targetHunted) && this.targetHunted.RaceProps.Humanlike)
                        {
                            if (!shyGuy.InMentalState)
                            {
                                goto label_19;
                            }
                        }
                        this.targetHunted = null;
                    }
                }
                if (shyGuy.CurJobDef == JobDefOf.AttackMelee)
                {
                    this.targetHunted = shyGuy.CurJob.targetA.Thing as Pawn;
                    if (this.targetHunted.Dead || this.targetHunted != null)
                    {
                        tickCounter = 0;
                        return;
                    }    
                }
                label_19:
                if (this.targetHunted != null && CanSeeShyGuy(this.targetHunted))
                {
                    Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, (LocalTargetInfo)(Thing)this.targetHunted);
                    //job.canBashDoors = true;
                    job.killIncappedTarget = true;
                    job.attackDoorIfTargetLost = true;
                    //job.canBashFences = true;
                    job.maxNumMeleeAttacks = 4;
                    shyGuy.jobs.TryTakeOrderedJob(job);
                }
                tickCounter = 0;
            }
        }

        private bool CanSeeShyGuy(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }
            return pawn.CanSee((Thing)shyGuy) && !pawn.Dead && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
        }
    }
}