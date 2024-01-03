using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Foundation.Utilities;

namespace Foundation
{
    public class ThoughtWorker_Precept_SCP_Social : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn) => !ModsConfig.IdeologyActive ? ThoughtState.Inactive : (ThoughtState)otherPawn.IsSCP();
    }
}