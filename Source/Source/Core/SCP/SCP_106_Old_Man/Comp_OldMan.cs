using Foundation.SRA;
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
using Foundation.Utilities;

namespace Foundation
{
    public class Comp_OldMan : ThingComp
    {
        public int tickCounter = 0;
        private Pawn targetHunted;
        public Pawn oldMan => this.parent as Pawn;
        private CompProperties_OldMan Props => (CompProperties_OldMan)this.props;


        public override void CompTick()
        {
            base.CompTick();
            ++tickCounter;
            if (tickCounter > Props.tickInterval)
            {
                if (!ScrantonCheck(oldMan) && !oldMan.Downed)
                {
                    Log.Message("Tick Counter");
                    if ((oldMan.CurJobDef == JobDefOf.PredatorHunt || oldMan.CurJob.def == JobDefOf.AttackMelee ) && this.WithinJumpRange(oldMan.CurJob.targetA.Thing.Position))
                    {
                        Log.Message("Target Hunt");
                        this.targetHunted = this.oldMan?.CurJob?.targetA.Thing as Pawn;
                    }
                    else if (oldMan.CurJob.def == JobDefOf.GotoWander || oldMan.CurJob.def == JobDefOf.Wait_Wander || oldMan.CurJob.def == JobDefOf.Wait_MaintainPosture || oldMan.GetRoom().Role == Foundation_Startup.containmentRoom || oldMan.mindState.mentalStateHandler.InMentalState)
                    {
                        Log.Message("Check");
                        List<Pawn> pawnList = SCPRadius.GetPawnsAround(oldMan.Position, Props.jumpRange, oldMan.MapHeld);
                        for (int index = 0; index < pawnList.Count; index++)
                        {
                            this.targetHunted = pawnList[index];
                            if (targetHunted != oldMan && !targetHunted.NonHumanlikeOrWildMan() && !this.TooCloseToJump())
                            {
                                goto label_19;
                            }
                        }
                    }
                label_19:
                    if (this.targetHunted != null)
                    {
                        if (!this.TooCloseToJump() && !ScrantonCheck(targetHunted))
                        {
                            List<Thing> source = new List<Thing>();
                            source.Add(oldMan);
                            IntVec3 pawnPos = targetHunted.Position;
                            GenExplosion.DoExplosion(oldMan.Position, targetHunted.MapHeld, 1, SCPDefOf.Foundation_106_Oldman_Scratch, oldMan, 1, 100, SoundDefOf.Corpse_Drop, postExplosionSpawnThingDef: ThingDefOf_SCP.Filth_OldMan, postExplosionSpawnChance: 1, postExplosionSpawnThingCount: 1, postExplosionGasType: null, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0, preExplosionSpawnThingCount: 0, chanceToStartFire: 0, damageFalloff: false, ignoredThings: source, doVisualEffects: false, propagationSpeed: 1);
                            GenExplosion.DoExplosion(pawnPos, targetHunted.MapHeld, 1, SCPDefOf.Foundation_106_Oldman_Scratch, oldMan, 1, 100, SoundDefOf.Corpse_Drop, postExplosionSpawnThingDef: ThingDefOf_SCP.Filth_OldMan, postExplosionSpawnChance: 1, postExplosionSpawnThingCount: 1, postExplosionGasType: null, applyDamageToExplosionCellsNeighbors: false, preExplosionSpawnThingDef: null, preExplosionSpawnChance: 0, preExplosionSpawnThingCount: 0, chanceToStartFire: 0, damageFalloff: false, ignoredThings: source, doVisualEffects: false, propagationSpeed: 1);
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
                    }
                }
                tickCounter = 0;
            }
        }
        private bool ScrantonCheck(Pawn pawn)
        {
            var field = SuppressionFieldAccessUtility.GetSuppressionFieldManager(pawn.Map)
                            ?.GetEffectOnCell(pawn.Position) ?? 0f;
            if (field != 0f)
                return true;
            return false;
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