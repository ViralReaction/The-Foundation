using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public class ThoughtWorker_Precept_SCP : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p) => (ThoughtState)p.IsSCP();
    }
}