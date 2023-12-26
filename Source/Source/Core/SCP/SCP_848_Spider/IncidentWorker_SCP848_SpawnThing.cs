using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    internal class IncidentWorker_SCP848_SpawnThing : IncidentWorker
    {
        private ThingDef scp848 = ThingDefOf_SCP.SCP_848R;
        private List<Thing> allSCP848s;
        private static readonly IntRange CountRange = new IntRange(0, 50);

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
                return false;
            List<Thing> all = ((Map)parms.target).listerThings.AllThings.FindAll((Predicate<Thing>)(o => o.def == this.scp848));
            this.allSCP848s = all;
            return !all.NullOrEmpty<Thing>() && all.Count >= 1;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map target = (Map)parms.target;
            if (this.allSCP848s.Count == 0)
            {
                this.allSCP848s = target.listerThings.AllThings.FindAll((Predicate<Thing>)(o => o.def == this.scp848));
                if (this.allSCP848s.Count == 0)
                {
                    Log.Error("Secure Contain Rimworld: SCP-848 event tried to fire when there are no SCP-848-2s on the map.");
                    return false;
                }
            }
            Log.Warning("Secure Contain Rimworld: If SCP-848 is spawning new artifacts that you already have and it causes errors, you can probably ignore them!");
            foreach (Thing allScP848 in this.allSCP848s)
            {
                int randomInRange1 = IncidentWorker_SCP848_SpawnThing.CountRange.RandomInRange;
                if (randomInRange1 > 30)
                {
                    PawnKindDef kind;
                    switch (new IntRange(0, 5).RandomInRange)
                    {
                        case 0:
                            kind = PawnKindDefOf_SCP.Rat;
                            break;
                        case 1:
                            kind = PawnKindDefOf_SCP.Boomrat;
                            break;
                        case 2:
                            kind = PawnKindDefOf_SCP.Cobra;
                            break;
                        case 3:
                            kind = PawnKindDefOf.Alphabeaver;
                            break;
                        case 4:
                            kind = PawnKindDefOf.Megascarab;
                            break;
                        default:
                            kind = PawnKindDefOf.Megaspider;
                            break;
                    }
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind));
                    GenSpawn.Spawn((Thing)pawn, allScP848.Position, target);
                    HealthUtility.DamageUntilDowned(pawn, false);
                }
                else if (randomInRange1 > 10)
                {
                    int randomInRange2 = new IntRange(0, 14).RandomInRange;
                    ThingDef def;
                    switch (randomInRange2)
                    {
                        case 0:
                            def = ThingDefOf_SCP.SCP_113_Stone;
                            break;
                        case 1:
                            def = ThingDefOf_SCP.SCP_475_Soap;
                            break;
                        case 2:
                            def = ThingDefOf_SCP.SCP_578_Bloodstone_Maker;
                            break;
                        case 3:
                            def = ThingDefOf_SCP.SCP_1577_Flare_Gun;
                            break;
                        case 4:
                            def = ThingDefOf_SCP.SCP_ES_019R;
                            break;
                        case 5:
                            def = ThingDefOf_SCP.SCP_113_Stone; // Placeholder
                           // def = ThingDefOf_SCP.SCP_732J;
                            break;
                        case 6:
                            def = ThingDefOf_SCP.SCP_127R;
                            break;
                        case 7:
                            def = ThingDefOf_SCP.SCP_572R;
                            break;
                        case 8:
                            def = ThingDefOf_SCP.SCP_3118R;
                            break;
                        case 9:
                            def = ThingDefOf_SCP.SCP_113_Stone; // Placeholder
                            //def = ThingDefOf_SCP.SCP_262R;
                            break;
                        case 10:
                            //def = ThingDefOf_SCP.SCP_043R;
                            def = ThingDefOf_SCP.SCP_113_Stone; // Placeholder
                            break;
                        case 11:
                            def = ThingDefOf_SCP.SCP_1762_Dragon_Box;
                            break;
                        case 12:
                            def = ThingDefOf_SCP.SCP_2228_Playset;
                            break;
                        case 13:
                            def = ThingDefOf_SCP.SCP_500_Panacea;
                            break;
                        default:
                            def = ThingDefOf_SCP.SCP_1797Consume;
                            break;
                    }
                    Thing thing = GenSpawn.Spawn(def, allScP848.Position, target);
                    if (randomInRange2 == 2)
                        thing.stackCount = 10;
                    GenPlace.TryPlaceThing(thing, allScP848.Position, target, ThingPlaceMode.Direct);
                    if (randomInRange2 == 11 || randomInRange2 == 12)
                        thing.MakeMinified();
                }
                else
                {
                    PawnKindDef kind;
                    switch (new IntRange(0, 6).RandomInRange)
                    {
                        case 0:
                            kind = PawnKindDefOf_SCP.SCP_111_Slimybellie;
                            break;
                        case 1:
                            kind = PawnKindDefOf_SCP.SCP_131_Eye_Pod;
                            break;
                        case 2:
                            kind = PawnKindDefOf_SCP.SCP_529_Josie;
                            break;
                        case 3:
                            kind = PawnKindDefOf_SCP.SCP_5185R;
                            break;
                        case 4:
                            kind = PawnKindDefOf_SCP.SCP_2845_Deer;
                            break;
                        case 5:
                            kind = PawnKindDefOf_SCP.SCP_160_Predator_Drone;
                            break;
                        default:
                            kind = PawnKindDefOf_SCP.SCP_5893R;
                            break;
                    }
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind));
                    GenSpawn.Spawn((Thing)pawn, allScP848.Position, target);
                    HealthUtility.DamageUntilDowned(pawn, false);
                }
            }
            this.SendStandardLetter("LetterLabelSCP848_SpawnItem".Translate((NamedArgument)this.scp848.label.CapitalizeFirst()), "LetterSCP848_SpawnItem".Translate((NamedArgument)this.scp848.label), LetterDefOf.NeutralEvent, parms, (LookTargets)this.allSCP848s[0]);
            return true;
        }
    }
}
