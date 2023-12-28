using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    public class SuppressionFieldManager : MapComponent
    {

        private Dictionary<IntVec3, SuppressionFieldEntry> suppressionField = new Dictionary<IntVec3, SuppressionFieldEntry>();

        public SuppressionFieldManager(Map map) : base(map) { }

        public void RegisterField(CompPsychicSuppressionField comp)
        {
            foreach (var cell in comp.CellsInRange())
            {
                if (suppressionField.TryGetValue(cell, out var existing))
                {
                    existing.Comps.Add(comp);
                }
                else
                {
                    var entry = new SuppressionFieldEntry();
                    entry.Comps.Add(comp);
                    suppressionField.Add(cell, entry);
                }
            }
        }

        public void UnregisterField(CompPsychicSuppressionField comp)
        {
            var entriesToRemove = new List<IntVec3>();
            foreach (var entry in suppressionField)
            {
                if (!entry.Value.Comps.Contains(comp)) continue;

                entry.Value.Comps.Remove(comp);

                if (entry.Value.Comps.Any()) continue;

                entriesToRemove.Add(entry.Key);
            }

            entriesToRemove.ForEach(entry => suppressionField.Remove(entry));
        }

        public void UpdateFieldRadius(CompPsychicSuppressionField comp)
        {
            UnregisterField(comp);
            RegisterField(comp);
        }

        public float GetEffectOnCell(IntVec3 cell)
        {
            return suppressionField.TryGetValue(cell, out var entry) ? entry.Effect : 0f;
        }

    }

    public class SuppressionFieldEntry
    {

        public List<CompPsychicSuppressionField> Comps = new List<CompPsychicSuppressionField>();
        public float Effect => Comps.Min(comp => comp.GetCurrentEffect());
    }
}