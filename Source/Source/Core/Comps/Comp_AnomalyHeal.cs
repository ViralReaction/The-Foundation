using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Comps
{
    public class Comp_AnomalyHeal : ThingComp
    {
        public int tickCounter = 0;
        public CompProperties_AnomalyHeal Props => (CompProperties_AnomalyHeal)this.props;
        public override void CompTick()
        {
            base.CompTick();
            tickCounter++;
            if (tickCounter > Props.tickInterval)
            {
                Pawn pawn = this.parent as Pawn;
                if (pawn.health != null)
                {
                    List<Hediff_Injury> injuryList = new List<Hediff_Injury>();
                    List<Hediff> injuryCheck = pawn.health.hediffSet.hediffs;
                    for (int index = 0; index < injuryCheck.Count; index++)
                    {
                        Hediff_Injury injury = injuryCheck[index] as Hediff_Injury;
                        if (injury != null)
                        {
                            injuryList.Add(injury);
                        }
                    }
                    if (injuryList.Count > 0)
                    {
                        Hediff_Injury hurt = injuryList.RandomElement();
                        hurt.Severity = hurt.Severity - Props.healAmount;
                    }
                }
                tickCounter = 0;
            }
        }
    }
}
