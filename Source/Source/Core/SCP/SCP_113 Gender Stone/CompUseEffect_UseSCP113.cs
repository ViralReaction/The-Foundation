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
    internal class CompUseEffect_UseSCP113  : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, 1f);
            usedBy.TakeDamage(dinfo);
            usedBy.TakeDamage(dinfo);
            if (usedBy.gender == Gender.Male)
            {
                usedBy.gender = Gender.Female;
                if (usedBy.story.bodyType == BodyTypeDefOf.Male)
                    usedBy.story.bodyType = BodyTypeDefOf.Female;
            }
            else if (usedBy.gender == Gender.Female)
            {
                usedBy.gender = Gender.Male;
                if (usedBy.story.bodyType == BodyTypeDefOf.Female)
                    usedBy.story.bodyType = BodyTypeDefOf.Male;
            }
            else
            {   
                // To cover players using mods such as Non-Binary Gender. Not perfect may swing back around later.
                Random rnd = new Random();
                int gndr = rnd.Next(1, 2);
                if (gndr == 1)
                {
                    usedBy.gender = Gender.Male;
                }
                else
                {
                    usedBy.gender = Gender.Female;
                }
            }
            string label = usedBy.gender.GetLabel();
            if (usedBy.gender != 0)
            {
                Messages.Message((string)("SCP113".Translate() + label), MessageTypeDefOf.PositiveEvent);
                Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(SCPDefOf.Foundation_113_BadThought);
                usedBy.needs.mood.thoughts.memories.TryGainMemory(newThought);
            }
            else
                Messages.Message((string)"SCP113F".Translate(), MessageTypeDefOf.NeutralEvent);
        }
    }
}