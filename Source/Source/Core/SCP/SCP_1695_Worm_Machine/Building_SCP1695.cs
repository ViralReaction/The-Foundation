using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.Sound;
using Verse;

namespace SCP
{
    internal class Building_SCP1695 : Building_CryptosleepCasket
    {
        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            Pawn pawn;
            int num;
            pawn = thing as Pawn;
            if (base.TryAcceptThing(thing, allowSpecialEffects))
            {
                pawn = thing as Pawn;
                num = pawn != null ? 1 : 0;
            }
            else
                num = 0;
            return num != 0 && !pawn.InMentalState;
        }

        public override void EjectContents()
        {
            ThingDef filthSlime = ThingDefOf.Filth_Slime;
            foreach (Thing thing in (IEnumerable<Thing>)this.innerContainer)
            {
                if (thing is Pawn pawn)
                {
                    PawnComponentsUtility.AddComponentsForSpawn(pawn);
                    pawn.filth.GainFilth(filthSlime);
                    if (pawn.RaceProps.IsFlesh)
                    {
                        DamageInfo dinfo = new DamageInfo(DamageDefOf.Cut, 1f);
                        pawn.TakeDamage(dinfo);
                    }
                    Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(SCPDefOF.SCP_1695_BadThought);
                    pawn.needs.mood.thoughts.memories.TryGainMemory(newThought);
                }
            }
            if (!this.Destroyed)
                SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.Position, this.Map)));
            this.innerContainer.TryDropAll(this.InteractionCell, this.Map, ThingPlaceMode.Near);
            this.contentsKnown = true;
        }
    }
}
