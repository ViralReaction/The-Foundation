using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;
using UnityEngine;

namespace Foundation
{
    public class GameCondition_SCP939 : GameCondition
    {
        public int scp939Count = Rand.RangeInclusive(1, 6);
        private const int TickCheckSpawn = 1000;
        private const int TickGeneratePawn = 2000;
        private int scp939_GeneratedTotal;
        private List<Pawn> scp939Generated = new List<Pawn>();

        public override void Init()
        {
            base.Init();
            this.scp939_GeneratedTotal = 0;
            this.scp939Generated = new List<Pawn>();
        }

        private bool TimeValid
        {
            get
            {
                if (gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
                {
                    Log.Message("Eclipse");
                    return true;
                }
                int num = GenLocalDate.HourOfDay(this.SingleMap);
                return num >= 20 || num < 4;
            }
        }

        private bool CanProbeMap => this.TimeValid && this.scp939Generated.Count > 0;

        private int HoursTillDawn
        {
            get
            {
                int num = GenLocalDate.HourOfDay(this.SingleMap);
                if (gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
                {
                    Log.Message("Eclipse");
                    return num = 12;
                }
                return num <= 6 ? 6 - num : 24 - num + 6;
            }
        }

        public List<Pawn> ActiveSCPInArea => this.scp939Generated;

        public void AddToRegistry(Pawn p)
        {
                if (p.Spawned)
                    p.DeSpawn(DestroyMode.Vanish);
                if (!this.scp939Generated.Contains(p))
                    this.scp939Generated.Add(p);
                Find.WorldPawns.PassToWorld(p);
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();
            if (Find.TickManager.TicksGame % 1000 != 0)
            {
                return;
            }
            if (this.CanProbeMap)
            {
                IntVec3 result;
                if (!RCellFinder.TryFindRandomPawnEntryCell(out result, this.SingleMap, CellFinder.EdgeRoadChance_Animal))
                    return;
                GenSpawn.Spawn((Thing)this.scp939Generated.Pop<Pawn>(), result, this.SingleMap);
                //Log.Message("SCP SPAWNED - Left: " + (object)this.scp939Generated.Count);
            }
            if (this.scp939_GeneratedTotal < this.scp939Count)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf_SCP.Foundation_ManyVoices_Incident, (Faction)null, PawnGenerationContext.NonPlayer, this.SingleMap.Tile, false, false, false, false, true, 0.0f, false, false, false, false, false, false, false, false, false, 0.0f, 0.0f, (Pawn)null, 0.0f, (Predicate<Pawn>)null, (Predicate<Pawn>)null, (IEnumerable<TraitDef>)null, (IEnumerable<TraitDef>)null, new float?(), new float?(), new float?(), new Gender?(), (string)null, (string)null, (RoyalTitleDef)null));
                ++this.scp939_GeneratedTotal;
                this.scp939Generated.Add(pawn);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.scp939Count, "scp939Count");
            Scribe_Values.Look<int>(ref this.scp939_GeneratedTotal, "scp939_GeneratedTotal");
            Scribe_Collections.Look<Pawn>(ref this.scp939Generated, "scp939Generated", LookMode.Reference);
        }
    }
}