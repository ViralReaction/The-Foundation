using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Foundation.Containment
{
    public static class ContainmentBreakerMentalStateUtility
    {
        private static List<Pawn> tmpAnimals = new List<Pawn>();

        public static Pawn FindSCP(Pawn pawn)
        {
            if (!pawn.Spawned)
                return (Pawn)null;
            ContainmentBreakerMentalStateUtility.tmpAnimals.Clear();
            List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
            for (int index = 0; index < allPawnsSpawned.Count; index++)
            {
                Pawn pawn1 = allPawnsSpawned[index];
                if (pawn1.IsSCP() && pawn1.GetRoom().Role == SCP_Startup.containmentRoom && pawn1.RaceProps.Animal && pawn1 != pawn && !pawn1.IsBurning() && pawn.CanReserveAndReach((LocalTargetInfo)(Thing)pawn1, PathEndMode.Touch, Danger.Deadly))
                    ContainmentBreakerMentalStateUtility.tmpAnimals.Add(pawn1);
            }
            if (!ContainmentBreakerMentalStateUtility.tmpAnimals.Any<Pawn>())
                return (Pawn)null;
            Pawn animal = ContainmentBreakerMentalStateUtility.tmpAnimals.RandomElement<Pawn>();
            ContainmentBreakerMentalStateUtility.tmpAnimals.Clear();
            return animal;
        }
    }
}