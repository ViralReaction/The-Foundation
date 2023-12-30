using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.Containment
{
        internal class ContainmentExtension : DefModExtension
        {
            public List<string> classRating = (List<string>)null;
            public int containmentTier = 0;
            public bool isUniqueSCP = false;
            public bool isTranqable = true;
            public bool willManhuntAfterBreach = false;
            public bool hatesRoofs = false;

            public string FindDescription(int pos) => this.classRating[pos] + "Desc";
        }
    }
