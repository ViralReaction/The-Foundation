using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    internal class DamageWorker_SCP_Flame : DamageWorker_Flame
    {
        public override void ExplosionAffectCell(
          Explosion explosion,
          IntVec3 c,
          List<Thing> damagedThings,
          List<Thing> ignoredThings,
          bool canThrowMotes)
        {
            GenExplosion.DoExplosion(explosion.instigator.Position, explosion.Map, 2f, DamageDefOf.Bomb, explosion.instigator);
            if (this.def != DamageDefOf.Flame || !Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map)))
                return;
            FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f), explosion.instigator);
        }
    }
}