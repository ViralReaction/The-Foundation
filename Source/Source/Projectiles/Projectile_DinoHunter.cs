using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SCP
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

                //return;
            //DamageInfo dinfo;
            //ref DamageInfo local = ref dinfo;
            //DamageDef damageDef = this.def.projectile.damageDef;
            //double armorPenetration = (double)this.ArmorPenetration;
            //Quaternion exactRotation = this.ExactRotation;
            //double y = (double)((Quaternion)ref exactRotation).eulerAngles.y;
            //Thing launcher = this.launcher;
            //ThingDef equipmentDef = this.equipmentDef;
            //Thing thing = this.intendedTarget.Thing;
            //local = new DamageInfo(damageDef, 99999f, (float)armorPenetration, (float)y, launcher, weapon: equipmentDef, intendedTarget: thing);
            //hitThing.TakeDamage(dinfo);
        }
    }
}
