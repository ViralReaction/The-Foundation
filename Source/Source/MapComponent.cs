using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace SCP
{
    internal class MapComponent_SCPManagement : MapComponent
    {
        private int counter = 0;

        public MapComponent_SCPManagement(Map map)
          : base(map)
        {
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            if (this.counter < 300)
            {
                ++this.counter;
            }
            else
            {
                this.counter = 0;
                if (this.map != null)
                {
                    this.SCP2845Management();
                }
            }
        }

        private void SCP2845Management()
        {
            Thing[] array1 = this.map.listerThings.ThingsOfDef(ThingDef.Named("SCP_2845_Deer")).ToArray();
            Thing[] array2 = this.map.listerThings.ThingsOfDef(ThingDef.Named("SCP_28451R")).ToArray();
            if (array1.Length == 0)
                return;
            foreach (Thing thing in array1)
            {
                if (thing is Pawn pawn)
                {
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs.ToArray())
                    {
                        if (hediff.def == HediffDef.Named("SCR_SCP2845TransmuteHediff"))
                            pawn.health.RemoveHediff(hediff);
                    }
                }
            }
            foreach (Thing thing in array2)
            {
                if (thing is Pawn pawn)
                {
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs.ToArray())
                    {
                        if (hediff.def == HediffDef.Named("SCP_2845_Transmute_Hediff"))
                            pawn.health.RemoveHediff(hediff);
                    }
                }
            }
        }
    }
}
