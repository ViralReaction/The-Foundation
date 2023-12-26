using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace SCP.Containment
{
    public class MentalStateWorker_ContainmentBreaker : MentalStateWorker
    {
        public override bool StateCanOccur(Pawn pawn) => base.StateCanOccur(pawn) && ContainmentBreakerMentalStateUtility.FindSCP(pawn) != null;
    }
}
