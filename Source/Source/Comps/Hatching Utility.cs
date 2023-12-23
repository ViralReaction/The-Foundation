using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SCP
{
    internal class CompHatcher : ThingComp
    {
        public float gestateProgress;
        public Pawn hatcheeParent;
        public Pawn otherParent;
        public Faction hatcheeFaction;

        public CompProperties_Hatcher Props => (CompProperties_Hatcher)this.props;

        private CompTemperatureRuinable FreezerComp => this.parent.GetComp<CompTemperatureRuinable>();

        public bool TemperatureDamaged
        {
            get
            {
                if (this.FreezerComp == null)
                    return false;
                if (this.FreezerComp.Ruined)
                    this.EggConvert();
                return this.FreezerComp.Ruined;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress");
            Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent");
            Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent");
            Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction");
        }

        public override void CompTick()
        {
            if (this.TemperatureDamaged)
                return;
            this.gestateProgress += (float)(1.0 / ((double)this.Props.hatcherDaystoHatch * 60000.0));
            if ((double)this.gestateProgress > 1.0)
                this.Hatch();
        }

        public void EggConvert()
        {
            ThingDef thing = Props.ruinedEgg;
            GenPlace.TryPlaceThing(ThingMaker.MakeThing(thing), this.parent.Position, this.parent.Map, ThingPlaceMode.Near);
            this.parent.Destroy(DestroyMode.Vanish);
        }

        public void Hatch()
        {
            try
            {
                PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction);
                for (int index = 0; index < this.parent.stackCount; ++index)
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(request);
                    if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, (Thing)this.parent))
                    {
                        if (pawn != null)
                        {
                            if (this.hatcheeParent != null)
                            {
                                if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
                                    pawn.playerSettings.AreaRestriction = this.hatcheeParent.playerSettings.AreaRestriction;
                                if (pawn.RaceProps.IsFlesh)
                                    pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
                            }
                            if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
                                pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
                        }
                        if (this.parent.Spawned)
                            FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid);
                    }
                    else
                        Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                }
            }
            finally
            {
                this.parent.Destroy(DestroyMode.Vanish);
            }
        }

        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            float num = (float)count / (float)(this.parent.stackCount + count);
            this.gestateProgress = Mathf.Lerp(this.gestateProgress, ((ThingWithComps)otherStack).GetComp<CompHatcher>().gestateProgress, num);
        }

        public override void PostSplitOff(Thing piece)
        {
            CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
            comp.gestateProgress = this.gestateProgress;
            comp.hatcheeParent = this.hatcheeParent;
            comp.otherParent = this.otherParent;
            comp.hatcheeFaction = this.hatcheeFaction;
        }

        public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
        {
            base.PrePreTraded(action, playerNegotiator, trader);
            switch (action)
            {
                case TradeAction.PlayerBuys:
                    this.hatcheeFaction = Faction.OfPlayer;
                    break;
                case TradeAction.PlayerSells:
                    this.hatcheeFaction = trader.Faction;
                    break;
            }
        }

        public override void PostPostGeneratedForTrader(
          TraderKindDef trader,
          int forTile,
          Faction forFaction)
        {
            base.PostPostGeneratedForTrader(trader, forTile, forFaction);
            this.hatcheeFaction = forFaction;
        }

        public override string CompInspectStringExtra() => !this.TemperatureDamaged ? (string)("EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent() + "\n" + "HatchesIn".Translate() + ": " + "PeriodDays".Translate((NamedArgument)(this.Props.hatcherDaystoHatch * (1f - this.gestateProgress)).ToString("F1"))) : (string)null;
    }
}
