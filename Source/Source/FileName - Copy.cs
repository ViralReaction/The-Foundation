using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.Sound;
using Verse;

namespace Foundation
{
    public class CompProperties_HumeShield : CompProperties
    {
        public int startingTicksToReset = 600;
        public float minDrawSize = 1.2f;
        public float maxDrawSize = 1.55f;
        public float energyOnReset = 40f;
        public bool blocksRangedWeapons = true;
        public string texPath = "Other/ShieldBubble";
        public Color shieldColor = Color.white;
        public int EnergyShieldEnergyMax = 30;
        public int EnergyShieldRechargeRate = 30;

        public CompProperties_HumeShield() => this.compClass = typeof(CompHumeShield);
    }
}