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
    public class CompGiveHediff : ThingComp
    {
        public int tickCounter = 0;
        public Pawn thisPawn;
        //List<Pawn> pawnList = new List<Pawn>();

        private CompProperties_GiveHediffSeverity Props => (CompProperties_GiveHediffSeverity)this.props;

        public override void CompTick()
        {
            base.CompTick();
            ++tickCounter;
            if (tickCounter > Props.tickInterval)
            {
                
                thisPawn = this.parent as Pawn;
                if (thisPawn != null && thisPawn.Map != null && !thisPawn.Dead && !thisPawn.Downed)
                {
                    List<Pawn> pawnsAround = SCPRadius.GetPawnsAround(thisPawn.Position, Props.range, thisPawn.Map);
                    for (int index = 0; index < pawnsAround.Count; index++)
                    {
                        Pawn pawn = pawnsAround[index];
                        if (pawn != null && !Props.defNamesImmune.Contains(pawn.def.defName))
                        {
                            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
                            if (!pawn.Dead)
                            {
                                if (Props.psychicCheck == true && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > 0f)
                                {
                                    if (firstHediffOfDef != null)
                                        firstHediffOfDef.Severity += Props.severityTick;
                                    else
                                        pawn.health.AddHediff(Props.hediff);
                                }
                                else
                                {
                                    if (firstHediffOfDef != null)
                                        firstHediffOfDef.Severity += Props.severityTick;
                                    else
                                        pawn.health.AddHediff(Props.hediff);
                                }
                            }
                        }
                    }
                }
                tickCounter = 0;
            }
        }
    }
}