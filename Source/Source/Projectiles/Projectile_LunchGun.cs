using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using Verse;

namespace SCP
{
    internal class Projectile_LunchGun : Bullet
    {

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);
            int num;
            //Pawn pawn;
            if (hitThing != null && hitThing is Pawn pawn)
            {
                num = pawn != null ? 1 : 0;
                foreach (Hediff hediff in pawn.health.hediffSet.GetHediffsTendable())
                {
                    if (hediff.Part.groups.Contains(BodyPartGroupDefOf.FullHead))
                    {
                        this.LunchTime(pawn);
                        hediff.Heal(10f);
                    }
                }
            }
            else
                num = 0;
            if (num == 0)
                return;
        }

        public void LunchTime(Pawn p)
        {
            ThingDef def = ThingDefOf.MealSurvivalPack;
            int num = 1;
            if (p.IsWildMan())
            {
                def = ThingDefOf.RawBerries;
                num = 10;
            }
            else if (p.Faction.def == FactionDefOf.Empire)
                def = ThingDefOf.MealFine;
            else if (p.Faction.def == FactionDefOf.Ancients || p.Faction.def == FactionDefOf.AncientsHostile || p.Faction.def == FactionDefOf.Pilgrims || p.Faction.def == FactionDefOf.Pirate)
                def = ThingDefOf.MealSurvivalPack;
            else if (p.Faction.def == FactionDefOf.OutlanderCivil || p.Faction.def == FactionDefOf.OutlanderRefugee || p.Faction.def == FactionDefOf.OutlanderRough || p.Faction.def == FactionDefOf.PlayerColony)
                def = ThingDefOf.MealSimple;
            else if (p.Faction.def == FactionDefOf.TribeCivil || p.Faction.def == FactionDefOf.TribeRough || p.Faction.def == FactionDefOf.PlayerTribe || p.Faction.def == FactionDefOf.Beggars)
            {
                def = ThingDefOf.Pemmican;
                num = 10;
            }
            if (p.story.traits.HasTrait(TraitDefOf.Cannibal))
            {
                def = ThingDefOf.Meat_Human;
                num = 10;
            }
            for (int index = 0; index < num; ++index)
                GenSpawn.Spawn(def, p.RandomAdjacentCell8Way(), p.Map);
        }
    }
}