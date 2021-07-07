using System.Collections.Generic;
using System.Linq;

namespace GoFigure.App.Model.Solution
{
    class SolutionPlan
    {
        public IList<ISolutionSlotValue> Slots { get; set; }

        public IList<int> AvailableNumbers =>
            Slots.Where(s => s is NumberSlotValue)
                .Cast<NumberSlotValue>()
                .Select(n => n.Value)
                .ToList();

        public SolutionPlan() =>
            Slots = new List<ISolutionSlotValue>();
    }
}
