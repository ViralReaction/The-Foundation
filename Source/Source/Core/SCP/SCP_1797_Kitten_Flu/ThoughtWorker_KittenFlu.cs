﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.Sound;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    internal class ThoughtWorker_KittenFlu : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(SCPDefOf.SCP_1797_Flu);
            if (firstHediffOfDef == null || firstHediffOfDef.FullyImmune())
                return (ThoughtState)false;
            if ((double)firstHediffOfDef.Severity >= 0.3 && (double)firstHediffOfDef.Severity < 0.301 || (double)firstHediffOfDef.Severity >= 0.6 && (double)firstHediffOfDef.Severity < 0.601 || (double)firstHediffOfDef.Severity >= 0.8 && (double)firstHediffOfDef.Severity < 0.801)
            {
                PawnUtility.TrySpawnHatchedOrBornPawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf_SCP.SCP_1797A_Kitten, Faction.OfPlayerSilentFail, forceGenerateNewPawn: true, allowDead: true, canGeneratePawnRelations: false, mustBeCapableOfViolence: true, allowGay: false, allowPregnant: true, allowAddictions: false)), (Thing)p);
                ThingDefOf_SCP.Hive_Spawn.PlayOneShot((SoundInfo)new TargetInfo(p.Position, p.Map));
                FilthMaker.TryMakeFilth(p.Position, p.Map, RimWorld.ThingDefOf.Filth_AmnioticFluid);
            }
            return (ThoughtState)true;
        }
    }
}