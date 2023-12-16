using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{ 
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