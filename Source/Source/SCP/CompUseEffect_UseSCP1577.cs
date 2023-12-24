using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class CompUseEffect_UseSCP1577 : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            Map map = usedBy.Map;
            float num = map.PlayerWealthForStoryteller;
            List<Thing> thingList = new List<Thing>();
            if ((double)map.wealthWatcher.WealthTotal < (double)num)
                num = map.wealthWatcher.WealthTotal;
            if ((double)num <= 81000.0)
            {
                Thing thing1 = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack);
                thing1.stackCount = 40;
                thingList.Add(thing1);
                Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Bedroll, ThingDefOf.Cloth);
                thing2.stackCount = map.mapPawns.ColonistCount;
                thingList.Add(thing2);
                Thing thing3 = ThingMaker.MakeThing(ThingDefOf.ComponentIndustrial);
                thing3.stackCount = 5;
                thingList.Add(thing3);
                Thing thing4 = ThingMaker.MakeThing(ThingDefOf_SCP.Apparel_Jacket, ThingDefOf_SCP.Synthread);
                thing4.stackCount = map.mapPawns.ColonistCount;
                thingList.Add(thing4);
                Thing thing5 = ThingMaker.MakeThing(ThingDefOf_SCP.Gun_BoltActionRifle);
                thingList.Add(thing5);
            }
            else if ((double)num < 182000.0)
            {
                Thing thing6 = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack);
                thing6.stackCount = 10;
                thingList.Add(thing6);
                Thing thing7 = ThingMaker.MakeThing(ThingDefOf.ComponentIndustrial);
                thing7.stackCount = 2;
                thingList.Add(thing7);
                Thing thing8 = ThingMaker.MakeThing(ThingDefOf_SCP.Apparel_Jacket, ThingDefOf.Leather_Plain);
                thingList.Add(thing8);
            }
            else
            {
                Thing thing = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack);
                thing.stackCount = 5;
                thingList.Add(thing);
            }
            DropPodUtility.DropThingsNear(usedBy.RandomAdjacentCell8Way(), map, (IEnumerable<Thing>)thingList, 150, leaveSlag: true, canRoofPunch: false);
            Messages.Message((string)"SCP1577".Translate(), MessageTypeDefOf.PositiveEvent);
        }
    }
}