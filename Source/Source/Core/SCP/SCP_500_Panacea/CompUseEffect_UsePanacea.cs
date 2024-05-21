using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class CompUseEffect_UsePanacea : CompUseEffect
    {
        public override void DoEffect(Pawn pawn)
        {

            for (int index = pawn.health.hediffSet.hediffs.Count - 1; index >= 0; --index)
            {
                Hediff hediff = pawn.health.hediffSet.hediffs[index];
                if (hediff.def.IsAddiction || hediff.def.makesSickThought || hediff.def == HediffDefOf.FoodPoisoning)
                {
                    pawn.health.RemoveHediff(hediff);
                }
            }
        }
    }
}
