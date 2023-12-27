using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP.Cage
{
    public class CompAssignableToPawn_Cage : CompAssignableToPawn
    {
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                var cage = this.parent as Building_Bed;
                return !(cage?.Spawned is true)
                    ? Enumerable.Empty<Pawn>()
                    : this.parent.Map.mapPawns.AllPawnsSpawned.FindAll(pawn =>
                        pawn.BodySize <= this.parent.def.building.bed_maxBodySize &&
                        pawn.AnimalOrWildMan() != this.parent.def.building.bed_humanlike &&
                        pawn.IsSCP());
            }
        }
        public override void TryAssignPawn(Pawn pawn)
        {
            Building_Bed parent = (Building_Bed)this.parent;
            pawn.ownership.ClaimBedIfNonMedical(parent);
            parent.NotifyRoomAssignedPawnsChanged();
            this.uninstalledAssignedPawns.Remove(pawn);
        }
        protected override string GetAssignmentGizmoLabel() => "SCP_AssignSCP".Translate();
        protected override string GetAssignmentGizmoDesc() => "SCP_AssignSCPDesc".Translate();
    }
}
