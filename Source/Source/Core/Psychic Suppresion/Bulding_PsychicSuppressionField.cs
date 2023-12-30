using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation.SRA
{
    public class Building_PsychicSuppressionField : Building
    {

        private CompPsychicSuppressionField comp => this.TryGetComp<CompPsychicSuppressionField>();

        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            if (Map == null) return;

            GenDraw.DrawRadiusRing(Position, comp.InConfigurationWindow ? comp.TargetRadius : comp.CurrentRadius);
        }
    }
}