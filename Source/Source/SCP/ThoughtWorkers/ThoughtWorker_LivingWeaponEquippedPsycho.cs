using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
  public class ThoughtWorker_LivingWeaponEquippedPsycho : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.equipment.Primary == null)
                return (ThoughtState)false;
            return p.equipment.Primary.def == ThingDefOf_SCP.SCP_127R ? (ThoughtState)true : (ThoughtState)false;
        }
    }
}