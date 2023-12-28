using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class Hediff_610_Infection : HediffWithComps
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            if (this.comps == null)
                return;
            if (this.pawn.kindDef == PawnKindDefOf_SCP.SCP_610A || this.pawn.kindDef == PawnKindDefOf_SCP.SCP_610B)
            {
                Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(SCPDefOF.SCP_610_Infection);
                this.pawn.health.RemoveHediff(hediff);
            }
            for (int index = 0; index < this.comps.Count; ++index)
                this.comps[index].CompPostPostAdd(dinfo);

        }
    }
}