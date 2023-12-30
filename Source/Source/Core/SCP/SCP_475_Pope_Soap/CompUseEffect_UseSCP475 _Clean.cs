using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using DubsBadHygiene;

namespace Foundation.DBHCompat
{
internal class CompUseEffect_UseSCP475_Clean : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            if (ModsConfig.IsActive("Dubwise.DubsBadHygiene"))
            {
                usedBy.needs.TryGetNeed<Need_Hygiene>().clean(100);
            }
        }
    }
}