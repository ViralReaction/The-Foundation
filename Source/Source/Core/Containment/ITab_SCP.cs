using Foundation.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foundation.Containment
{
    public class ITab_Foundation_Needs : ITab
    {
#pragma warning disable CS0414 // The field 'ITab_Foundation_Needs.thoughtScrollPosition' is assigned but its value is never used
        private Vector2 thoughtScrollPosition;
#pragma warning restore CS0414 // The field 'ITab_Foundation_Needs.thoughtScrollPosition' is assigned but its value is never used

        public override bool IsVisible
        {
            get
            {
                if (this.SelPawn.IsSCP() && this.SelPawn.IsCaptiveOf() && this.SelPawn.Faction == null)
                    return true;
                return false;
            }
        }

        public ITab_Foundation_Needs()
        {
            this.labelKey = "TabSCPNeeds";
            this.tutorTag = "SCPContainment";
        }

        public override void OnOpen() => this.thoughtScrollPosition = new Vector2();

        protected override void FillTab()
        {
            Verse.Text.Font = GameFont.Small;
            Rect rect1 = new Rect(0.0f, 0.0f, this.size.x, this.size.y).ContractedBy(10f);
            Listing_Standard listing = new Listing_Standard();
            listing.maxOneColumn = true;
            listing.Begin(rect1);
            //Rect rect2 = listing.GetRect(28f);
            //rect2.width = 140f;
            //MedicalCareUtility.MedicalCareSetter(rect2, ref this.SelPawn.playerSettings.medCare);
            //listing.Gap(4f);
            StringBuilder sb = new StringBuilder();
            int num = (int)ContainmentBreakUtility.InitiatePrisonBreakMtbDays(this.SelPawn, sb, true);
            string str1 = (string)("ContainmentBreakMTBDays".Translate() + ": ");
            string label;
            if (ContainmentBreakUtility.IsPrisonBreaking(this.SelPawn))
                label = (string)(str1 + "CurrentlyContainmentBreaking".Translate());
            else if (num < 0)
            {
                label = (string)(str1 + "Never".Translate());
            }
            else
                label = str1 + "PeriodDays".Translate((NamedArgument)num).ToString().Colorize(ColoredText.DateTimeColor);
            Rect rect3 = listing.Label(label);
            string tip1 = (string)"ContainmentBreakMTBDaysDescription".Translate();
            if (sb.Length > 0)
                tip1 = tip1 + "\n\n" + sb.ToString();
            TooltipHandler.TipRegion(rect3, (TipSignal)tip1);
            Widgets.DrawHighlightIfMouseover(rect3);
            this.DoSlavePriceListing(listing, this.SelPawn);
            TaggedString taggedString1;
            if (this.SelPawn.Faction == null || this.SelPawn.Faction.IsPlayer || !this.SelPawn.Faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
            {
                taggedString1 = "None".Translate();
            }
            else
            {
                bool isHealthy;
                bool isInMentalState;
                int adjustedGoodwillChange = this.SelPawn.Faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, this.SelPawn.Faction.GetGoodwillGainForExit(this.SelPawn, true, out isHealthy, out isInMentalState));
                taggedString1 = !isHealthy || isInMentalState ? (isHealthy ? (!isInMentalState ? "None".Translate() : "None".Translate() + " (" + this.SelPawn.MentalState.InspectLine + ")") : "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")") : this.SelPawn.Faction.NameColored + " " + adjustedGoodwillChange.ToStringWithSign();
            }
            this.ContainmentDays(listing, this.SelPawn);
            listing.End();
            this.size = new Vector2(280f, (float)((double)listing.CurHeight + 10 + 24.0));
        }

        private void DoSlavePriceListing(Listing_Standard listing, Pawn pawn)
        {
            float statValue = this.SelPawn.GetStatValue(StatDefOf.MarketValue);
            Rect rect = listing.Label("FoundationSellPrice".Translate() + ": " + statValue.ToStringMoney());
            if (!Mouse.IsOver(rect))
                return;
            Widgets.DrawHighlight(rect);
            TaggedString tip = "FoundationSellPriceDescription".Translate((NamedArgument)"HUH?".Translate().ToLower()) + "\n\n" + StatDefOf.MarketValue.Worker.GetExplanationFull(StatRequest.For((Thing)this.SelPawn), StatDefOf.MarketValue.toStringNumberSense, statValue);
            TooltipHandler.TipRegion(rect, (TipSignal)tip);
        }
        private void ContainmentDays(Listing_Standard listing, Pawn pawn)
        {
            int value = FoundationComponent.ContainmentBreakCheck(pawn);
            Rect rect = listing.Label("DaysinContainment".Translate() + ": " + value.ToString());
            if (!Mouse.IsOver(rect))
                return;
            Widgets.DrawHighlight(rect);
            TaggedString tip = "SlavePriceDescription".Translate((NamedArgument)"Slave".Translate().ToLower()) + "\n\n" + StatDefOf.MarketValue.Worker.GetExplanationFull(StatRequest.For((Thing)this.SelPawn), StatDefOf.MarketValue.toStringNumberSense, value);
            TooltipHandler.TipRegion(rect, (TipSignal)tip);
        }
    }
}