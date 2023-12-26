using System.Collections.Generic;
using RimWorld;
using Verse;

namespace PsiTech.Utility
{
    [StaticConstructorOnStartup]
    public static class PsiTechCachingUtility
    {

        public static int _totalStatsInGame;
        private static bool[] _cachedAffectedStats;
        public static readonly List<ThingDef> CachedCryptosleepDefs = new List<ThingDef>();

        static PsiTechCachingUtility()
        {

            // Initialize cache array
            _totalStatsInGame = DefDatabase<StatDef>.DefCount;
            _cachedAffectedStats = new bool[_totalStatsInGame];

            // Cache psychic sensitivity for the suppression field just in case
            _cachedAffectedStats[StatDefOf.PsychicSensitivity.index] = true;

        }

        public static bool EverAffectsStat(StatDef stat)
        {
            return stat.index < _totalStatsInGame && _cachedAffectedStats[stat.index];
        }
    }
}