using Foundation.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class CompUseEffect_UseSCP35 : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            usedBy.Kill(null);
        }
    }
}