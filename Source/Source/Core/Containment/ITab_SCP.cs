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
    public class ITab_SCP_Needs : ITab
    {
        private Vector2 thoughtScrollPosition;

        public override bool IsVisible
        {
            get
            {
                if (this.SelPawn.IsSCP() && this.SelPawn.IsCaptiveOf() && this.SelPawn.Faction == null)
                    return true;
                return false;
            }
        }

        public ITab_SCP_Needs()
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
            Rect rect2 = listing.GetRect(28f);
            rect2.width = 140f;
            MedicalCareUtility.MedicalCareSetter(rect2, ref this.SelPawn.playerSettings.medCare);
            listing.Gap(4f);
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
                int adjustedGoodwillChange = this.SelPawn.Faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, this.SelPawn.Faction.GetGoodwillGainForPrisonerRelease(this.SelPawn, out isHealthy, out isInMentalState));
                taggedString1 = !isHealthy || isInMentalState ? (isHealthy ? (!isInMentalState ? "None".Translate() : "None".Translate() + " (" + this.SelPawn.MentalState.InspectLine + ")") : "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")") : this.SelPawn.Faction.NameColored + " " + adjustedGoodwillChange.ToStringWithSign();
            }
            this.ContainmentDays(listing, this.SelPawn);
            //Rect rect7 = listing.Label("PrisonerReleasePotentialRelationGains".Translate() + ": " + taggedString1);
            //TooltipHandler.TipRegionByKey(rect7, "PrisonerReleaseRelationGainsDesc");
            //Widgets.DrawHighlightIfMouseover(rect7);
            //}
            //if (prisonerOfColony)
            //{
            //    if (!wildMan)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        int num = (int)PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.SelPawn, sb, true);
            //        string str1 = (string)("PrisonBreakMTBDays".Translate() + ": ");
            //        string label;
            //        if (PrisonBreakUtility.IsPrisonBreaking(this.SelPawn))
            //            label = (string)(str1 + "CurrentlyPrisonBreaking".Translate());
            //        else if (num < 0)
            //        {
            //            label = (string)(str1 + "Never".Translate());
            //            Gene gene;
            //            if (PrisonBreakUtility.GenePreventsPrisonBreaking(this.SelPawn, out gene))
            //            {
            //                sb.AppendLineIfNotEmpty();
            //                sb.AppendTagged((TaggedString)"PrisonBreakingDisabledDueToGene".Translate(gene.def.Named("GENE")).Colorize(ColorLibrary.RedReadable));
            //            }
            //        }
            //        else
            //            label = str1 + "PeriodDays".Translate((NamedArgument)num).ToString().Colorize(ColoredText.DateTimeColor);
            //        Rect rect3 = listing.Label(label);
            //        string tip1 = (string)"PrisonBreakMTBDaysDescription".Translate();
            //        if (sb.Length > 0)
            //            tip1 = tip1 + "\n\n" + sb.ToString();
            //        TooltipHandler.TipRegion(rect3, (TipSignal)tip1);
            //        Widgets.DrawHighlightIfMouseover(rect3);
            //        if (this.SelPawn.guest.Recruitable)
            //        {
            //            Rect rect4 = listing.Label("RecruitmentResistance".Translate() + ": " + this.SelPawn.guest.resistance.ToString("F1"));
            //            if (Mouse.IsOver(rect4))
            //            {
            //                TaggedString taggedString = "RecruitmentResistanceDesc".Translate();
            //                FloatRange floatRange = this.SelPawn.kindDef.initialResistanceRange.Value;
            //                TaggedString tip2 = taggedString + string.Format("\n\n{0}: {1}~{2}", (object)"RecruitmentResistanceFromPawnKind".Translate((NamedArgument)this.SelPawn.kindDef.LabelCap), (object)floatRange.min, (object)floatRange.max);
            //                if (this.SelPawn.royalty != null)
            //                {
            //                    RoyalTitle mostSeniorTitle = this.SelPawn.royalty.MostSeniorTitle;
            //                    if (mostSeniorTitle != null && (double)mostSeniorTitle.def.recruitmentResistanceOffset != 0.0)
            //                    {
            //                        string str2 = (double)mostSeniorTitle.def.recruitmentResistanceOffset > 0.0 ? "+" : "-";
            //                        tip2 += "\n" + "RecruitmentResistanceRoyalTitleOffset".Translate((NamedArgument)mostSeniorTitle.Label.CapitalizeFirst()) + (": " + str2) + mostSeniorTitle.def.recruitmentResistanceOffset.ToString();
            //                    }
            //                }
            //                TooltipHandler.TipRegion(rect4, (TipSignal)tip2);
            //            }
            //            Widgets.DrawHighlightIfMouseover(rect4);
            //        }
            //        else
            //        {
            //            Rect rect5 = listing.Label("RecruitmentResistance".Translate() + ": " + "NonRecruitable".Translate());
            //            string tip3 = (string)"NonRecruitableTip".Translate();
            //            if (ModsConfig.IdeologyActive)
            //                tip3 = (string)(tip3 + ("\n\n" + "NonRecruitableTipCannotConvert".Translate()));
            //            TooltipHandler.TipRegion(rect5, (TipSignal)tip3);
            //            Widgets.DrawHighlightIfMouseover(rect5);
            //        }
            //        if (ModsConfig.IdeologyActive)
            //        {
            //            Rect rect6 = listing.Label("WillLevel".Translate() + ": " + this.SelPawn.guest.will.ToString("F1"));
            //            TaggedString tip4 = "WillLevelDesc".Translate((NamedArgument)2.5f);
            //            if (!this.SelPawn.guest.EverEnslaved)
            //            {
            //                FloatRange floatRange = this.SelPawn.kindDef.initialWillRange.Value;
            //                tip4 += string.Format("\n\n{0} : {1}~{2}", (object)"WillFromPawnKind".Translate((NamedArgument)this.SelPawn.kindDef.LabelCap), (object)floatRange.min, (object)floatRange.max);
            //            }
            //            TooltipHandler.TipRegion(rect6, (TipSignal)tip4);
            //            Widgets.DrawHighlightIfMouseover(rect6);
            //        }
            //    }
            //    this.DoSlavePriceListing(listing, this.SelPawn);
            //    TaggedString taggedString1;
            //    if (this.SelPawn.Faction == null || this.SelPawn.Faction.IsPlayer || !this.SelPawn.Faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
            //    {
            //        taggedString1 = "None".Translate();
            //    }
            //    else
            //    {
            //        bool isHealthy;
            //        bool isInMentalState;
            //        int adjustedGoodwillChange = this.SelPawn.Faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, this.SelPawn.Faction.GetGoodwillGainForPrisonerRelease(this.SelPawn, out isHealthy, out isInMentalState));
            //        taggedString1 = !isHealthy || isInMentalState ? (isHealthy ? (!isInMentalState ? "None".Translate() : "None".Translate() + " (" + this.SelPawn.MentalState.InspectLine + ")") : "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")") : this.SelPawn.Faction.NameColored + " " + adjustedGoodwillChange.ToStringWithSign();
            //    }
            //    Rect rect7 = listing.Label("PrisonerReleasePotentialRelationGains".Translate() + ": " + taggedString1);
            //    TooltipHandler.TipRegionByKey(rect7, "PrisonerReleaseRelationGainsDesc");
            //    Widgets.DrawHighlightIfMouseover(rect7);
            //    if (this.SelPawn.guilt.IsGuilty)
            //    {
            //        if (!this.SelPawn.InAggroMentalState)
            //            listing.Label("ConsideredGuilty".Translate((NamedArgument)this.SelPawn.guilt.TicksUntilInnocent.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor)));
            //        else
            //            listing.Label("ConsideredGuiltyNoTimer".Translate() + " (" + this.SelPawn.MentalStateDef.label + ")");
            //    }
            //    if (ModsConfig.IdeologyActive & prisonerOfColony && this.SelPawn.guest.interactionMode == PrisonerInteractionModeDefOf.Convert)
            //    {
            //        Rect rect8 = listing.GetRect(32f);
            //        Rect rect9;
            //        // ISSUE: explicit constructor call
            //        ((Rect)ref rect9).Set((float)((double)((Rect)ref rect8).xMax - 32.0 - 4.0), ((Rect)ref rect8).y, 32f, 32f);
            //        ((Rect)ref rect8).xMax = ((Rect)ref rect9).xMin;
            //        Verse.Text.Anchor = (TextAnchor)3;
            //        Widgets.Label(rect8, "IdeoConversionTarget".Translate());
            //        Verse.Text.Anchor = (TextAnchor)0;
            //        Widgets.DrawHighlightIfMouseover(rect8);
            //        TooltipHandler.TipRegionByKey(rect8, "IdeoConversionTargetDesc");
            //        this.SelPawn.guest.ideoForConversion.DrawIcon(rect9.ContractedBy(2f));
            //        if (Mouse.IsOver(rect9))
            //        {
            //            Widgets.DrawHighlight(rect9);
            //            TooltipHandler.TipRegion(rect9, (TipSignal)this.SelPawn.guest.ideoForConversion.name);
            //        }
            //        if (Widgets.ButtonInvisible(rect9))
            //        {
            //            List<FloatMenuOption> options = new List<FloatMenuOption>();
            //            foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
            //            {
            //                Ideo newIdeo = allIdeo;
            //                string label = allIdeo.name;
            //                Action action = (Action)(() => this.SelPawn.guest.ideoForConversion = newIdeo);
            //                if (!this.ColonyHasAnyWardenOfIdeo(newIdeo, this.SelPawn.MapHeld))
            //                {
            //                    label = (string)(label + (" (" + "NoWardenOfIdeo".Translate(newIdeo.memberName.Named("MEMBERNAME")) + ")"));
            //                    action = (Action)null;
            //                }
            //                options.Add(new FloatMenuOption(label, action, newIdeo.Icon, newIdeo.Color));
            //            }
            //            Find.WindowStack.Add((Window)new FloatMenu(options));
            //        }
            //    }
            //    if (this.SelPawn.guest.finalResistanceInteractionData != null)
            //    {
            //        ResistanceInteractionData resistanceInteractionData = this.SelPawn.guest.finalResistanceInteractionData;
            //        Rect rect10 = listing.Label("LastRecruitment".Translate() + ": " + resistanceInteractionData.resistanceReduction.ToStringByStyle(ToStringStyle.FloatTwo));
            //        if (Mouse.IsOver(rect10))
            //        {
            //            Widgets.DrawHighlight(rect10);
            //            TaggedString tip = "LastRecruitmentDescription".Translate((NamedArgument)(Thing)this.SelPawn, (NamedArgument)resistanceInteractionData.initiatorName) + "\n\n" + ("StatsReport_BaseValue".Translate() + ": " + 1f.ToStringByStyle(ToStringStyle.FloatTwo)) + ("\n\n" + "Mood".Translate() + ": x" + resistanceInteractionData.recruiteeMoodFactor.ToStringByStyle(ToStringStyle.FloatTwo)) + ("\n" + "RecruiterNegotiationAbility".Translate() + ": x" + resistanceInteractionData.initiatorNegotiationAbilityFactor.ToStringByStyle(ToStringStyle.FloatTwo)) + ("\n" + "OpinionOfRecruiter".Translate() + ": x" + resistanceInteractionData.recruiterOpinionFactor.ToStringByStyle(ToStringStyle.FloatTwo)) + ("\n" + "StatsReport_FinalValue".Translate() + ": " + resistanceInteractionData.resistanceReduction.ToStringByStyle(ToStringStyle.FloatTwo));
            //            TooltipHandler.TipRegion(rect10, (TipSignal)tip);
            //        }
            //    }
            //    ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
            //    ITab_Pawn_Visitor.tmpPrisonerInteractionModes.AddRange((IEnumerable<PrisonerInteractionModeDef>)DefDatabase<PrisonerInteractionModeDef>.AllDefs.Where<PrisonerInteractionModeDef>((Func<PrisonerInteractionModeDef, bool>)(m => CanUsePrisonerInteractionMode(this.SelPawn, m))).OrderBy<PrisonerInteractionModeDef, int>((Func<PrisonerInteractionModeDef, int>)(pim => pim.listOrder)));
            //    float height = (float)(28.0 * (double)ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Count + 20.0);
            //    Rect rect11 = listing.GetRect(height).Rounded();
            //    Widgets.DrawMenuSection(rect11);
            //    Rect rect12 = rect11.ContractedBy(10f);
            //    Widgets.BeginGroup(rect12);
            //    Rect rect13;
            //    // ISSUE: explicit constructor call
            //    ((Rect)ref rect13).Set(0.0f, 0.0f, ((Rect)ref rect12).width, 28f);
            //    foreach (PrisonerInteractionModeDef prisonerInteractionMode in ITab_Pawn_Visitor.tmpPrisonerInteractionModes)
            //    {
            //        if (Widgets.RadioButtonLabeled(rect13, (string)prisonerInteractionMode.LabelCap, this.SelPawn.guest.interactionMode == prisonerInteractionMode))
            //        {
            //            PrisonerInteractionModeDef interactionMode = this.SelPawn.guest.interactionMode;
            //            this.SelPawn.guest.interactionMode = prisonerInteractionMode;
            //            this.InteractionModeChanged(interactionMode, prisonerInteractionMode);
            //        }
            //        if (!prisonerInteractionMode.description.NullOrEmpty() && Mouse.IsOver(rect13))
            //        {
            //            Widgets.DrawHighlight(rect13);
            //            TooltipHandler.TipRegion(rect13, (TipSignal)prisonerInteractionMode.description);
            //        }
            //        ref Rect local = ref rect13;
            //        ((Rect)ref local).y = ((Rect)ref local).y + 28f;
            //    }
            //    Widgets.EndGroup();
            //    ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
            //}
            listing.End();
            this.size = new Vector2(280f, (float)((double)listing.CurHeight + 10 + 24.0));
        }

        //protected override void UpdateSize() => this.size = NeedsCardUtility.GetSize(this.SelPawn) + new Vector2(17f, 17f) * 2f;
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