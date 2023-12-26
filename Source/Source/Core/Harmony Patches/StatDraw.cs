using HarmonyLib;
using RimWorld;
using SCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public static class StatDrawEntry_Patch
    {
        private static readonly System.Type patchType = typeof(StatDrawEntry_Patch);

        static StatDrawEntry_Patch() => new Harmony("rimworld.vr.SCP.main").PatchAll(Assembly.GetExecutingAssembly());

        [HarmonyPatch(typeof(ThingDef))]
        [HarmonyPatch("SpecialDisplayStats")]
        public static class SCP_StatDrawEntry_Patch
        {
            public static void Postfix(ThingDef __instance, ref IEnumerable<StatDrawEntry> __result)
            {
                if (__instance.GetModExtension<ContainmentExtension>() == null || __instance.IsCorpse || __instance.GetModExtension<ContainmentExtension>().classRating == null)
                    return;
                StatCategoryDef category = StatCategoryDefOf.Basics;
                if (__instance.IsWeapon)
                    category = StatCategoryDefOf.Weapon;
                else if (__instance.IsApparel)
                    category = StatCategoryDefOf.Apparel;
                else if (__instance.IsDrug)
                    category = StatCategoryDefOf.Drug;
                else if (__instance.isTechHediff)
                    category = StatCategoryDefOf.Implant;
                ContainmentExtension modExtension = __instance.GetModExtension<ContainmentExtension>();
                if (modExtension.classRating.Count == 3)
                {
                    __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Contain".Translate(), (string)modExtension.classRating[0].Translate(), (string)modExtension.FindDescription(0).Translate(), 1));
                    __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Disrupt".Translate(), (string)modExtension.classRating[1].Translate(), (string)modExtension.FindDescription(1).Translate(), 1));
                    __result = __result.AddItem<StatDrawEntry>(new StatDrawEntry(category, (string)"SCP_Risk".Translate(), (string)modExtension.classRating[2].Translate(), (string)modExtension.FindDescription(2).Translate(), 1));
                }
                else
                    Log.Error("SCP Harmony Patching Error: " + __instance.defName + " Does not have correct number of containment procedures.");
            }
        }
    }
}