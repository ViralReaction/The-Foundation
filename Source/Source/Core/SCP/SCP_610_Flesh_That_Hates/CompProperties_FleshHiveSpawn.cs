using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class CompProperties_FleshHiveSpawner : CompProperties

    {
        public List<string> spawnablePawnKinds;
        public SoundDef spawnSound;
        public float defendRadius = 20f;
        public int initialPawnCount;
        public int maxPawnCount = 20;
        public FloatRange pawnSpawnIntervalDays = new FloatRange(0.5f, 2f);
        public int pawnSpawnRadius = 2;
        public string nextSpawnInspectStringKey;

        public CompProperties_FleshHiveSpawner() => this.compClass = typeof(Comp_FleshHiveSpawn);

    }
}