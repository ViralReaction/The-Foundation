using RimWorld;
using Verse;

namespace Foundation
{
    public class IngestionOutcomeDoer_IngestBloodOpal : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            IntVec3 pos = pawn.Position;
            ThingDef filth = pawn.RaceProps.BloodDef;
            Map map = pawn.MapHeld;
            Thing thing = GenSpawn.Spawn(FoundationDefOf.Foundation_BloodOpal, pos, map);
            thing.stackCount = 100;
            GenPlace.TryPlaceThing(thing, pos, map, ThingPlaceMode.Direct);
            pawn.inventory.DropAllNearPawn(pos, true);
            pawn.equipment.DropAllEquipment(pawn.Position, true);
            pawn.apparel.DropAll(pos, true);
            for (int i = 0; i < 20; i++) 
            {
                IntVec3 c;
                if (!CellFinder.TryFindRandomReachableNearbyCell(pos, map, 3 , TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => x.Standable(pawn.MapHeld), (Region x) => true, out c, 999999))
                {
                    Log.Message("Failing");
                    return;
                }
                Log.Message("Are we getting here");
                FilthMaker.TryMakeFilth(c, map, filth, 1, FilthSourceFlags.None, true);
            }
            
            pawn.Destroy(DestroyMode.KillFinalize);

        }
       
    }
}