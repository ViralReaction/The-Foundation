using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public class CompProperties_PsychicSuppressionField : CompProperties
    {

        public float MinEffect = -2f;
        public float MaxEffect = 0f;
        public float EffectStep = 0.1f;
        public float EffectChangeSpeedPerSecond = 0.01f;

        public float MinRadius = 1f;
        public float MaxRadius = 5f;
        public float RadiusChangeSpeedPerSecond = 0.02f;

        public float BasePowerConsumption = 50f;
        public float PowerPerCellEffect = 2f;

        public CompProperties_PsychicSuppressionField()
        {
            compClass = typeof(CompPsychicSuppressionField);
        }

    }
}