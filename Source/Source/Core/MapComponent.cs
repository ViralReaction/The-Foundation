using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using Foundation.Containment;
using Foundation.Utilities;

namespace Foundation
{
    internal class MapComponent_SCPManagement : MapComponent
    {
        private static PawnKindDef scp19051 = PawnKindDefOf_SCP.SCP_19051R;
        private static ThingDef scp1905 = ThingDefOf_SCP.SCP_1905_Dino_Hunter;
        public bool isFirstLoad = true;
        private int tickCounter = 0;

        //public static Dictionary<Pawn, int> ContainmentBreakDict = new Dictionary<Pawn, int>();
        //public static List<Pawn> CapturedSCP = new List<Pawn>();
        //public static List<int> lastContainmentBreak = new List<int>();
        public MapComponent_SCPManagement(Map map) : base(map)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.isFirstLoad, "isFirstLoad", true, true);
            //Scribe_Collections.Look(ref lastContainmentBreak, "lastContainmentBreak", LookMode.Deep);
            //Scribe_Collections.Look(ref CapturedSCP, "CapturedSCP", LookMode.Deep);
            //Scribe_Collections.Look(ref ContainmentBreakDict, "ContainmentBreakDict", LookMode.Reference, LookMode.Value, ref CapturedSCP, ref lastContainmentBreak);
            //ContainmentBreakDict.RemoveAll(entry => (entry.Key?.Destroyed ?? true) || (entry.Value.Equals(null)));
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            if (!this.isFirstLoad || !(this.map.Biome.defName == "SCP_610_Biome"))
                return;
            Log.Message("610 Biome");
            this.spawnSCP610Hive(Rand.Range(4, 8));
            this.isFirstLoad = false;
        }

        public void spawnSCP610Hive(int counter)
        {
            foreach (IntVec3 intVec3 in this.map.AllCells.InRandomOrder<IntVec3>())
            {
                if (counter <= 0)
                    break;
                TerrainDef terrain = intVec3.GetTerrain(this.map);
                if (terrain.IsSoil && (double)terrain.fertility < 0.6)
                {
                    Thing newThing = ThingMaker.MakeThing(ThingDefOf_SCP.SCP_610Hive);
                    if (GenAdj.OccupiedRect(intVec3, newThing.Rotation, newThing.def.size).InBounds(this.map))
                    {
                        GenSpawn.Spawn(newThing, intVec3, this.map);
                        --counter;
                    }
                }
            }
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            ++tickCounter;
            if (tickCounter == 60000)
            { 
                if (this.map != null)
                {
                    this.SCP1905Management();
                    this.ContainmentBreak();
                }
                tickCounter = 0;
            }
        }
        private void ContainmentBreak()
        {
            if (map.IsPlayerHome)
            {
                IReadOnlyList<Pawn> pawnList = map.mapPawns.AllPawnsSpawned;
                for (int index = 0; index < pawnList.Count; index++)
                {
                    Pawn pawn = pawnList[index];
                    if (pawn.IsCaptiveOf() && pawn.Faction == null)
                    {
                        //Log.Message("Checking Prison Break MTB");
                        float mtb = ContainmentBreakUtility.InitiatePrisonBreakMtbDays(pawn);
                        FoundationComponent.ContainmentBreakDay(pawn);
                        if ((double)mtb >= 0.0 && Rand.MTBEventOccurs(mtb, 1f, 2500f))
                        ContainmentBreakUtility.StartPrisonBreak(pawn);  

                    }
                }
            }
        }
        private void SCP1905Management()
        {
            //Log.Message("Firing Dino");
            Thing[] array = this.map.listerThings.ThingsOfDef(MapComponent_SCPManagement.scp1905).ToArray();
            bool flag1 = false;
            bool flag2 = false;
            Pawn pawn1 = (Pawn)null;
            Thing thing = (Thing)null;
            IReadOnlyList<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
            foreach (Pawn pawn2 in allPawnsSpawned)
            {
                if (pawn2.def.defName == MapComponent_SCPManagement.scp19051.defName && !pawn2.Downed)
                    return;
            }
            if (array.Length >= 1)
            {
                foreach (Thing a in array)
                {
                    for (int index = 0; index < allPawnsSpawned.Count; index++)
                    {
                        if (a.AdjacentTo8WayOrInside((Thing)allPawnsSpawned[index]) && !allPawnsSpawned[index].AnimalOrWildMan())
                        {
                            thing = a;
                            flag1 = true;
                            goto label_15;
                        }
                    }
                }
            }
        label_15:
            if (!flag1)
            {
                foreach (Pawn p in allPawnsSpawned)
                {
                    if (!p.AnimalOrWildMan())
                    {
                        foreach (Thing orInventoryThing in p.EquippedWornOrInventoryThings)
                        {
                            if (orInventoryThing.def == MapComponent_SCPManagement.scp1905)
                            {
                                pawn1 = p;
                                thing = orInventoryThing;
                                flag2 = true;
                                goto label_31;
                            }
                        }
                    }
                }
            }
        label_31:
            if (!(flag1 | flag2) || thing == null)
                return;
            if (pawn1 != null)
                this.SpawnAnimalManhunter(pawn1.Position, MapComponent_SCPManagement.scp19051);
            else
                this.SpawnAnimalManhunter(thing.Position, MapComponent_SCPManagement.scp19051);
            //this.counter -= 2500;
            Messages.Message((string)"MessageSCP1905Spawn".Translate(), MessageTypeDefOf.NegativeEvent);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
        }

        private void SpawnAnimalManhunter(IntVec3 location, PawnKindDef pawnKind, Gender? gender = null)
        {
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(location, this.map, 14);
            if (!(GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(pawnKind), loc, this.map, Rot4.Random) is Pawn pawn))
                return;
            pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
        }
    }
}
