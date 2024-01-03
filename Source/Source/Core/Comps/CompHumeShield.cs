using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.Sound;
using Verse;
using Foundation.Utilities;

namespace Foundation.Comps
{
    [StaticConstructorOnStartup]
    public class CompHumeShield : ThingComp
    {
        protected float energy;
        protected int ticksToReset = -1;
        protected int lastKeepDisplayTick = -9999;
        private Vector3 impactAngleVect;
        private int lastAbsorbDamageTick = -9999;
        private const float MaxDamagedJitterDist = 0.05f;
        private const int JitterDurationTicks = 8;
        private int KeepDisplayingTicks = 1000;
        private int energyLossPerDamage = 1;
        protected virtual Material BubbleMat => MaterialPool.MatFrom(Props.texPath, ShaderDatabase.Transparent, Props.shieldColor);

        public CompProperties_HumeShield Props => (CompProperties_HumeShield) this.props;

        public virtual float EnergyMax => this.Props.EnergyShieldEnergyMax;

        private float EnergyGainPerTick => this.Props.EnergyShieldRechargeRate / 60f;

        public float Energy => this.energy;

        public ShieldState ShieldState
        {
            get
            {
                if (this.parent is Pawn parent && (parent.IsCharging() || parent.IsSelfShutdown()))
                    return ShieldState.Disabled;
                CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
                if (comp != null && !comp.Awake)
                    return ShieldState.Disabled;
                return this.ticksToReset <= 0 ? ShieldState.Active : ShieldState.Resetting;
            }
        }

        protected bool ShouldDisplay
        {
            get
            {
                Pawn pawnOwner = this.PawnOwner;
                return pawnOwner.Spawned && !pawnOwner.Dead && !pawnOwner.Downed && (pawnOwner.InAggroMentalState || pawnOwner.Drafted || pawnOwner.Faction.HostileTo(Faction.OfPlayer) && !pawnOwner.IsPrisoner || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks || ModsConfig.BiotechActive && pawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == pawnOwner);
            }
        }

        protected Pawn PawnOwner
        {
            get
            {
                return this.parent is Pawn parent2 ? parent2 : (Pawn)null;
            }
        }

        protected Material bubbleMat;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.energy, "energy");
            Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1);
            Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick");
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
                yield return gizmo;
            IEnumerator<Gizmo> enumerator = (IEnumerator<Gizmo>)null;
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action commandAction1 = new Command_Action();
                commandAction1.defaultLabel = "DEV: Break";
                commandAction1.action = new Action(this.Break);
                yield return (Gizmo)commandAction1;
                if (this.ticksToReset > 0)
                {
                    Command_Action commandAction2 = new Command_Action();
                    commandAction2.defaultLabel = "DEV: Clear reset";
                    commandAction2.action = (Action)(() => this.ticksToReset = 0);
                    yield return (Gizmo)commandAction2;
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
                yield return gizmo;
            IEnumerator<Gizmo> enumerator = (IEnumerator<Gizmo>)null;
                  foreach (Gizmo gizmo2 in this.GetGizmos())
                    yield return gizmo2;
                enumerator = (IEnumerator<Gizmo>)null;
        }

        private IEnumerable<Gizmo> GetGizmos()
        {
            yield return (Gizmo)new Gizmo_HumeShield()
            {
                shield = this
            };
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.PawnOwner == null)
                this.energy = 0.0f;
            else if (this.ShieldState == ShieldState.Resetting)
            {
                --this.ticksToReset;
                if (this.ticksToReset > 0)
                    return;
                this.Reset();
            }
            else
            {
                if (this.ShieldState != ShieldState.Active)
                    return;
                this.energy += this.EnergyGainPerTick;
                if ((double)this.energy > (double)this.EnergyMax)
                    this.energy = this.EnergyMax;
            }
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (this.ShieldState != 0 || !dinfo.Def.isRanged && !dinfo.Def.isExplosive)
                return;
            this.energy -= dinfo.Amount * this.energyLossPerDamage;
            if ((double)this.energy < 0.0)
                this.Break();
            else
                this.AbsorbedDamage(dinfo);
            absorbed = true;
        }

        public void KeepDisplaying() => this.lastKeepDisplayTick = Find.TickManager.TicksGame;

        private void AbsorbedDamage(DamageInfo dinfo)
        {
            SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot((SoundInfo)new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map));
            this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = this.parent.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
            float scale = Mathf.Min(10f, (float)(2.0 + (double)dinfo.Amount / 10.0));
            FleckMaker.Static(loc, this.PawnOwner.Map, FleckDefOf.ExplosionFlash, scale);
            int num = (int)scale;
            for (int index = 0; index < num; ++index)
                FleckMaker.ThrowDustPuff(loc, this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
            this.KeepDisplaying();
        }

        private void Break()
        {
            float scale = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
            SCPDefOf.HumeShield_Break.SpawnAttached((Thing)this.parent, this.parent.MapHeld, scale);
            FleckMaker.Static(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, SCPDefOf.ExplosionHumeFlash, 12f);
            for (int index = 0; index < 6; ++index)
                FleckMaker.ThrowDustPuff(this.parent.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), this.parent.Map, Rand.Range(0.8f, 1.2f));
            this.energy = 0.0f;
            this.ticksToReset = this.Props.startingTicksToReset;
        }

        private void Reset()
        {
            if (this.PawnOwner.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot((SoundInfo)new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map));
                FleckMaker.ThrowLightningGlow(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, 3f);
            }
            this.ticksToReset = -1;
            this.energy = this.Props.energyOnReset;
        }

        public override void PostDraw()
        {
            base.PostDraw();
            this.Draw();
        }

        private void Draw()
        {
            if (this.ShieldState != ShieldState.Active || !this.ShouldDisplay)
                return;
            float num = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, energy);
            var props = Props;
            Vector3 drawPos = this.parent.DrawPos;
            drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
            int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
            if (num2 < 8)
            {
                float num3 = (float)(8 - num2) / 8f * 0.05f;
                drawPos += impactAngleVect * num3;
                num -= num3;
            }

            float angle = 0;
            Vector3 s = new Vector3(num, 1f, num);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
        }

        public override bool CompAllowVerbCast(Verb verb) => !this.Props.blocksRangedWeapons || !(verb is Verb_LaunchProjectile);
    }
}