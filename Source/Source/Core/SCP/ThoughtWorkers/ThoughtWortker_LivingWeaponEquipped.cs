using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    public class ThoughtWorker_LivingWeaponEquipped : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.equipment.Primary == null)
                return (ThoughtState)false;
                if (p.story.traits.HasTrait(TraitDefOf.Psychopath))
            { 
                    return (ThoughtState)false;

            }
            return p.equipment.Primary.def == ThingDefOf_SCP.SCP_127R ? (ThoughtState)true : (ThoughtState)false;
        }
    }
}