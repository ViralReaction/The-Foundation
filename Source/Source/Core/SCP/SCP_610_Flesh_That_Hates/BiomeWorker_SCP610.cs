using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP
{
    internal class BiomeWorker_SCP610 : BiomeWorker
    {
        public override float GetScore(Tile tile, int tileID)
        {
            if (tile.WaterCovered || tile.hilliness == Hilliness.Flat)
                return -100f;
            return (double)tile.temperature < -20.0 || (double)tile.rainfall < 400.0 || (double)tile.rainfall >= 2500.0 ? 0.0f : (float)((22.5 + ((double)tile.temperature - 20.0) + ((double)tile.rainfall - 600.0) / 100.0) / 3.0);
        }
    }
}