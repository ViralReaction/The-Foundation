using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{ 
// Removes Hediff specified between <hediff></hediff> in in the comp upon ingestion. Useful for antidotes and such.
public class IngestionOutcomeDoer_RemoveHediff : IngestionOutcomeDoer
{
    public HediffDef hediffDef;
    protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
    {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
            if (hediff is null)
                return;
            hediff.Severity = 0f;

    }
}
}