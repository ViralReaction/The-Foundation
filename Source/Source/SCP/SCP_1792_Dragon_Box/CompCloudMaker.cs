using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using SCP;
using Verse.AI;

namespace SCP.Comps
{
    public class CompCloudMaker : ThingComp
    {
        public int tickCounter = 0;
        private Effecter effecter;
        private static List<IntVec3> tmpValidCells = new List<IntVec3>();

        private CompProperties_CloudMaker Props => (CompProperties_CloudMaker)this.props;

        public override void CompTick()
        {
            base.CompTick();
            ++tickCounter;
            if (tickCounter > Props.tickerInterval)
            {
                List<Pawn> pawnsAround = SCPRadius.GetPawnsAround(this.parent.Position, 1, this.parent.Map);
                if (pawnsAround.Count >= 1)
                {
                    if (this.effecter == null)
                    {
                        this.effecter = this.Props.sourceStreamEffect.Spawn();
                        this.effecter.Trigger((TargetInfo)(Thing)this.parent, (TargetInfo)(Thing)this.parent);
                    }
                    this.effecter.EffectTick((TargetInfo)(Thing)this.parent, (TargetInfo)(Thing)this.parent);
                    if (!Rand.MTBEventOccurs(this.Props.fleckSpawnMTB, 1f, 1f))
                        return;
                    CompCloudMaker.tmpValidCells.Clear();
                    Room room = this.parent.GetRoom();
                    CellRect cellRect = this.parent.OccupiedRect();
                    foreach (IntVec3 intVec3 in cellRect.ExpandedBy(Mathf.FloorToInt(this.Props.cloudRadius + 1f)))
                    {
                        if ((double)cellRect.ClosestCellTo(intVec3).DistanceTo(intVec3) <= (double)this.Props.cloudRadius && intVec3.GetRoom(this.parent.Map) == room)
                            CompCloudMaker.tmpValidCells.Add(intVec3);
                    }
                    if (CompCloudMaker.tmpValidCells.Count <= 0)
                        return;
                    this.parent.Map.flecks.CreateFleck(FleckMaker.GetDataStatic(CompCloudMaker.tmpValidCells.RandomElement<IntVec3>().ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead), this.parent.Map, this.Props.cloudFleck, this.Props.fleckScale));
                }
                else
                {
                    this.effecter?.Cleanup();
                    this.effecter = (Effecter)null;
                }
                tickCounter = 0;
            }

        }

        public override void PostDeSpawn(Map map)
        {
            this.effecter?.Cleanup();
            this.effecter = (Effecter)null;
        }
    }
}