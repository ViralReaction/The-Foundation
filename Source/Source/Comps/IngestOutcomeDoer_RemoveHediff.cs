using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{ 
// Removes Hediff specified between <hediff></hediff> in in the comp upon ingestion. Useful for antidotes and such.
public class IngestionOutcomeDoer_RemoveHediff : IngestionOutcomeDoer
{
    public HediffDef hediffDef;
    protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
    {
        if (!pawn.health.hediffSet.HasHediff(hediffDef))
            return;
        pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).Severity *= 0.0f;
    }
}
}