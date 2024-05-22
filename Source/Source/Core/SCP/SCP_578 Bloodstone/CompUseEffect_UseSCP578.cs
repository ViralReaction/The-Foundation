using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static UnityEngine.GraphicsBuffer;
using Foundation;

namespace Foundation
{
    public class IngestionOutcomeDoer_IngestBloodOpal : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Thing thing = GenSpawn.Spawn(FoundationDefOf.Foundation_BloodOpal, pawn.Position, pawn.Map);
            thing.stackCount = 100;
            GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Direct);
            this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
            pawn.Destroy(DestroyMode.KillFinalize);
        }

        public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false, bool rememberPrimary = false)
        {
            if (this.kindDef.destroyGearOnDrop)
            {
                this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
                this.apparel.DestroyAll(DestroyMode.Vanish);
            }
            if (!this.InContainerEnclosed)
            {
                if (base.SpawnedOrAnyParentSpawned)
                {
                    Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
                    if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null)
                    {
                        Thing thing;
                        this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
                    }
                    if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
                    {
                        Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
                        if (pawn_EquipmentTracker != null)
                        {
                            pawn_EquipmentTracker.DropAllEquipment(base.PositionHeld, true, rememberPrimary);
                        }
                        if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
                        {
                            this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
                        }
                    }
                }
                return;
            }
            Pawn_CarryTracker pawn_CarryTracker2 = this.carryTracker;
            if (((pawn_CarryTracker2 != null) ? pawn_CarryTracker2.CarriedThing : null) != null)
            {
                this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
            }
            Pawn_EquipmentTracker pawn_EquipmentTracker2 = this.equipment;
            if (((pawn_EquipmentTracker2 != null) ? pawn_EquipmentTracker2.Primary : null) != null)
            {
                this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
            }
            Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
            if (pawn_InventoryTracker == null)
            {
                return;
            }
            pawn_InventoryTracker.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
        }
    }
}