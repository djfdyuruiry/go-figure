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

        public bool IsWellFormed =>
            !Slots.Select((s, idx) => new { v = s, idx = idx })
                .Any(s =>
                {
                    if (s.idx == 0 || s.idx == Slots.Count - 1)
                    {
                        // first and last slots cannot be operators
                        return s.v is OperatorSlotValue;
                    }

                    if (s.v is OperatorSlotValue)
                    {
                        // can't have two operators in a row
                        return Slots[s.idx - 1] is OperatorSlotValue
                            || Slots[s.idx + 1] is OperatorSlotValue;
                    }

                    if (s.v is NumberSlotValue)
                    {
                        // can't have two numbers in a row
                        return Slots[s.idx - 1] is NumberSlotValue
                            || Slots[s.idx + 1] is NumberSlotValue;
                    }

                    return false;
                });

        public SolutionPlan() =>
            Slots = new List<ISolutionSlotValue>();
    }
}
