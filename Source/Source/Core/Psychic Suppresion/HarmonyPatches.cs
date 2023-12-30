using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.SRA
{
    public static class HarmonyPatch_PsychicSuppression
    {
        private static readonly System.Type patchType = typeof(HarmonyPatch_PsychicSuppression);
        static HarmonyPatch_PsychicSuppression() => new Harmony("SCP.psychic.suppress").PatchAll(Assembly.GetExecutingAssembly());

        [HarmonyPatch(typeof(StatWorker), "GetValueUnfinalized")]
        public static class SCP_StatValuePatch
        {

            public static float Postfix(float __result, StatRequest req, StatDef ___stat)
            {
                if (!ScrantonCachingUtility.EverAffectsStat(___stat) || !req.HasThing) return __result;

                if (req.Thing is Pawn pawn)
                {
                    if (___stat == StatDefOf.PsychicSensitivity)
                    {
                        var field = SuppressionFieldAccessUtility.GetSuppressionFieldManager(pawn.Map)
                            ?.GetEffectOnCell(pawn.Position) ?? 0f;
                        if (field != 0f)
                        {
                            __result += field;
                        }

                        return __result;
                    }
                } return __result;
            }
        }
    }
}
