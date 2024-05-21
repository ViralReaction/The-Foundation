using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
internal class CompUseEffect_UseSCP475 : CompUseEffect
    {
       public float baseCertaintyGain = 1f;

        public override void DoEffect(Pawn usedBy)
        {
            if (ModLister.CheckIdeology("Ideoligion certainty"))
            {
                usedBy.ideo.Reassure(this.baseCertaintyGain);
                Find.PlayLog.Add((LogEntry)new PlayLogEntry_Interaction(InteractionDefOf.Reassure, usedBy, usedBy, (List<RulePackDef>)null));
            }
            if (usedBy.health.hediffSet.HasHediff(FoundationDefOf.GutWorms))
            {
                usedBy.health.hediffSet.GetFirstHediffOfDef(FoundationDefOf.GutWorms).Severity *= 0.0f;
            }
           
        }
    }
}