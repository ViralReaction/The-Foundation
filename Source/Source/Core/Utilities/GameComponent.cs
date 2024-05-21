using Foundation.SRA;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class FoundationComponent : GameComponent
    {
        public static Dictionary<Pawn, int> ContainmentBreakDict = new Dictionary<Pawn, int>();
        public static List<Pawn> CapturedSCP = new List<Pawn>();
        public static List<int> lastContainmentBreak = new List<int>();
        public FoundationComponent(Game game) { }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref lastContainmentBreak, "lastContainmentBreak", LookMode.Deep);
            Scribe_Collections.Look(ref CapturedSCP, "CapturedSCP", LookMode.Deep);
            Scribe_Collections.Look(ref ContainmentBreakDict, "ContainmentBreakDict", LookMode.Reference, LookMode.Value, ref CapturedSCP, ref lastContainmentBreak);
            ContainmentBreakDict.RemoveAll(entry => (entry.Key?.Destroyed ?? true) || (entry.Value.Equals(null)));
        }

        public static int ContainmentBreakCheck(Pawn pawn)
        {
            if (FoundationComponent.ContainmentBreakDict.ContainsKey(pawn))
            {
                if (FoundationComponent.ContainmentBreakDict.TryGetValue(pawn, out int value))
                {
                    //FoundationComponent.ContainmentBreak.Increment(pawn);
                    return value;
                }
                return 0;
            }
            else
                FoundationComponent.ContainmentBreakDict.Add(pawn, 0);
            return 0;
        }
        public static void ContainmentBreakDay(Pawn pawn)
        {
            if (FoundationComponent.ContainmentBreakDict.ContainsKey(pawn))
            {
                FoundationComponent.ContainmentBreakDict.TryGetValue(pawn, out int value);
                FoundationComponent.ContainmentBreakDict.SetOrAdd(pawn, (value+1));
            }
        }

        public static void ContainmentBreakOut(Pawn pawn)
        {
            FoundationComponent.ContainmentBreakDict.SetOrAdd(pawn, 0);
        }
        public void Notify_PawnDied(Pawn pawn)
        {
            if (!ContainmentBreakDict.TryGetValue(pawn, out var tracker)) return;
            ContainmentBreakDict.Remove(pawn);
        }
        //public static Pawn adfa(Pawn pawn)
        //{
        //    if (pawn == null) 
        //        return null;
        //    Foundation
        //    FoundationComponent.ContainmentBreak.Add(pawn, value)
        //         if (FoundationComponent.ContainmentBreak.ContainsKey(pawn))
        //            FoundationComponent.ContainmentBreak.TryGetValue(pawn, out int value)
        //        else 
        //        FoundationComponent.ContainmentBreak.Add(pawn, 0);
        //    if (FoundationComponent.ContainmentBreak.TryGetValue(pawn, out int value)
        //        return existing;

        //    //FoundationComponent.ContainmentBreak.Add(pawn);
        // CachedManagers.Add(map.Index, manager);
        //return manager;
    }
}
