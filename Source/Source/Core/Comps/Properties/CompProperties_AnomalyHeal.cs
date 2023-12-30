using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.Sound;
using Verse;

namespace Foundation.Comps
{
    public class CompProperties_AnomalyHeal : CompProperties
    {
        public int tickInterval = 300;
        public int healAmount = 1;

        public CompProperties_AnomalyHeal() => this.compClass = typeof(Comp_AnomalyHeal);
    }
}