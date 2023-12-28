using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foundation
{
    internal class Projectile_DinoHunter : Bullet
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);
            if (hitThing != null && hitThing is Pawn pawn && pawn.kindDef == PawnKindDefOf_SCP.SCP_19051R)
            {
                hitThing.Kill();
            }
            return;
        }
    }
}
