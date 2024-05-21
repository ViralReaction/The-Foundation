using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Foundation
{
    public class CompVoices : ThingComp
    {

    private int voicesCycle = 0;
    private bool targetLured;
    private Pawn targetHunted;
    private SPTuples.SPTuple<Pawn, bool, Pawn> voiceAttempted;

    public bool TargetLured => this.targetHunted != null && !this.targetHunted.Downed && !this.targetHunted.Dead && this.targetLured;

    public bool VoicesActive { get; private set; }

    public Pawn Pawn => this.parent as Pawn;

    public CompProperties_Voices Props => (CompProperties_Voices)this.props;

    private bool WithinVoiceRange(IntVec3 targetPos) => SPExtra.Distance(this.Pawn.Position, targetPos) <= (double)this.Props.voiceRange;

    private bool TooCloseToVoice()
    {
        if (this.targetHunted == null)
        {
            this.ResetVoices();
            return true;
        }
        return this.targetHunted.Downed || this.targetHunted.Dead || this.targetHunted.CanSee((Thing)this.Pawn) && this.WithinVoiceRange(this.targetHunted.Position) && SPExtra.Distance(this.Pawn.Position, this.targetHunted.Position) <= (double)this.Props.directAttackRange;
    }

    private bool TooFarToVoice() => this.targetHunted == null || this.targetHunted.Downed || this.targetHunted.Dead || !this.WithinVoiceRange(this.targetHunted.Position);

    public override void CompTick()
    {
        base.CompTick();
        if (!this.TargetLured || SPExtra.Distance(this.Pawn.Position, this.targetHunted.Position) > (double)this.Props.directAttackRange)
            return;
        this.targetLured = false;
    }

    public override void CompTickRare()
    {
        base.CompTickRare();
        if (this.Pawn.CurJobDef == JobDefOf.PredatorHunt && this.WithinVoiceRange(this.Pawn.CurJob.targetA.Thing.Position) && !this.VoicesActive && (object)this.voiceAttempted == null && !this.TargetLured)
        {
            this.targetHunted = this.Pawn?.CurJob?.targetA.Thing as Pawn;
            if (!this.TooCloseToVoice())
            {
                this.VoicesActive = true;
                List<Pawn> source = new List<Pawn>();
                foreach (Pawn pawn in this.targetHunted.MapHeld.mapPawns.AllPawnsSpawned)
                {
                    if (pawn.RaceProps.Humanlike && pawn != this.targetHunted && this.targetHunted.relations.everSeenByPlayer)
                        source.Add(pawn);
                }
                this.voiceAttempted = new SPTuples.SPTuple<Pawn, bool, Pawn>(this.targetHunted, false, source.Count > 0 ? source.RandomElement<Pawn>() : (Pawn)null);
            }
            else
                goto label_19;
        }
        if (this.VoicesActive && this.voiceAttempted?.First != null && !this.voiceAttempted.Second)
        {
            if (this.TooFarToVoice())
            {
                this.ResetVoices();
                return;
            }
            ++this.voicesCycle;
            if (this.voicesCycle >= 4)
            {
                this.VoicesActive = false;
                this.voiceAttempted.Second = true;
                if ((int)Mathf.Lerp(0.0f, 50f, (float)((this.voiceAttempted.Third != null ? this.targetHunted.relations.OpinionOf(this.voiceAttempted.Third) : 0) + 100) / 200f) > Rand.Range(0, 100))
                {
                    this.targetLured = true;
                    this.targetHunted.jobs.TryTakeOrderedJob(new Job(JobDefOf.GotoWander, (LocalTargetInfo)this.Pawn.Position), JobTag.InMentalState);
                    this.StartMentalStateSpecificPos(this.targetHunted, this.Pawn.Position);
                }
            }
        }
    label_19:
        if (this.targetHunted == null || !this.targetHunted.Dead && this.targetHunted == this.Pawn?.CurJob?.targetA.Thing as Pawn)
            return;
        this.ResetVoices();
    }

    private void StartMentalStateSpecificPos(Pawn pawn, IntVec3 position)
    {
        MentalState_Voices instance = (MentalState_Voices)Activator.CreateInstance(FoundationDefOf.FollowTheVoices.stateClass);
        instance.voicesHeardFrom = position;
        instance.pawn = pawn;
        instance.def = FoundationDefOf.FollowTheVoices;
        instance.causedByMood = false;
        instance.PreStart();
        if (pawn.Drafted)
            pawn.drafter.Drafted = false;
        AccessTools.Field(typeof(MentalStateHandler), "curStateInt").SetValue((object)pawn.mindState.mentalStateHandler, (object)instance);
    }

    private void ResetVoices()
    {
        this.VoicesActive = false;
        this.voicesCycle = 0;
        this.voiceAttempted = (SPTuples.SPTuple<Pawn, bool, Pawn>)null;
        this.targetHunted = (Pawn)null;
        this.targetLured = false;
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look<int>(ref this.voicesCycle, "voicesCycle");
    }
}
}