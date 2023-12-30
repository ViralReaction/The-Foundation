using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Verse;
using Verse.AI;
using Verse.Noise;
using static UnityEngine.GraphicsBuffer;

namespace Foundation
{
    public class Comp_OldMan : ThingComp
    {
        public int tickCounter = 0;
        private Pawn targetHunted;
        public Pawn oldMan => this.parent as Pawn;
        private CompProperties_OldMan Props => (CompProperties_OldMan)this.props;
        private bool validTarget = false;


        public override void CompTick()
        {
            base.CompTick();
            ++tickCounter;
            if (tickCounter > Props.tickInterval && !oldMan.Downed)
            {
                validTarget = false;
                if ((oldMan.CurJobDef == JobDefOf.PredatorHunt || oldMan.CurJob.def == JobDefOf.AttackMelee) && this.WithinJumpRange(oldMan.CurJob.targetA.Thing.Position))
                {

                    this.targetHunted = this.oldMan?.CurJob?.targetA.Thing as Pawn;
                }
                else if (oldMan.mindState.mentalStateHandler.InMentalState || oldMan.GetRoom().Role == SCP_Startup.containmentRoom)
                {
                    List<Pawn> pawnList = SCPRadius.GetPawnsAround(oldMan.Position, Props.jumpRange, oldMan.MapHeld);
                    for (int index = 0; index < pawnList.Count && !validTarget; index++)
                    {
                        this.targetHunted = pawnList[index];
                        if (targetHunted != oldMan && !targetHunted.NonHumanlikeOrWildMan() && !this.TooCloseToJump())
                            validTarget = true;
                    }
                }
                if (!this.TooCloseToJump())
                {
                    List<Thing> source = new List<Thing>();
                    source.Add(oldMan);
                    IntVec3 pawnPos = targetHunted.Position;
                    GenExplosion.DoExplosion(oldMan.Position, targetHunted.MapHeld, 1, SCPDefOf.SCP_106_Oldman_Scratch, oldMan, 1, 100, SoundDefOf.Corpse_Drop, postExplosionSpawnThingDef: ThingDefOf_SCP.Filth_OldMan, postExplosionSpawnChance: 1, postExplosionSpawnThingCount: 1, postExplosionGasType: null, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0, preExplosionSpawnThingCount: 0, chanceToStartFire: 0, damageFalloff: false, ignoredThings: source, doVisualEffects: false, propagationSpeed: 1);
                    GenExplosion.DoExplosion(pawnPos, targetHunted.MapHeld, 1, SCPDefOf.SCP_106_Oldman_Scratch, oldMan, 1, 100, SoundDefOf.Corpse_Drop, postExplosionSpawnThingDef: ThingDefOf_SCP.Filth_OldMan, postExplosionSpawnChance: 1, postExplosionSpawnThingCount: 1, postExplosionGasType: null, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0, preExplosionSpawnThingCount: 0, chanceToStartFire: 0, damageFalloff: false, ignoredThings: source, doVisualEffects: false, propagationSpeed: 1);
                    oldMan.pather.StopDead();
                    oldMan.Position = pawnPos;
                    oldMan.pather.ResetToCurrentPosition();
                    if (oldMan.CurJobDef != JobDefOf.PredatorHunt || oldMan.CurJobDef != JobDefOf.AttackMelee || !oldMan.mindState.mentalStateHandler.InMentalState)
                    {
                        Job job = JobMaker.MakeJob(JobDefOf.PredatorHunt, (LocalTargetInfo)(Thing)targetHunted);
                        oldMan.jobs.TryTakeOrderedJob(job);
                    }
                    oldMan.pather.StartPath(oldMan.CurJob.targetA.Cell, PathEndMode.ClosestTouch);
                }
                tickCounter = 0;
            }
        }
        private bool TooCloseToJump()
        {
            if (this.targetHunted == null)
            {
                return true;
            }
            return this.targetHunted.Downed || this.targetHunted.Dead || this.targetHunted.CanSee((Thing)this.oldMan) && this.WithinJumpRange(this.targetHunted.Position) && SPExtra.Distance(this.oldMan.Position, this.targetHunted.Position) <= (double)this.Props.directAttackRange;
        }
        private bool WithinJumpRange(IntVec3 targetPos) => SPExtra.Distance(oldMan.Position, targetPos) <= (double)this.Props.jumpRange;
    }
}