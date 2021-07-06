using GoFigure.App.Model;
using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils
{
    class SolutionComputer
    {
        private readonly Calculator _calculator;

        public SolutionComputer(Calculator calculator)
        {
            _calculator = calculator;
        }

        public int ResultFor(SolutionPlan solution)
        {
            int? result = null;
            var op = Operator.Divide;
            int rhs;

            if (solution.Slots is null)
            {
                return 0;
            }

            foreach (var slot in solution.Slots)
            {
                if (slot is null)
                {
                    continue;
                }

                if (!result.HasValue)
                {
                    result = slot.As<NumberSlotValue>().Value;
                    continue;
                }

                if (slot is OperatorSlotValue)
                {
                    op = slot.As<OperatorSlotValue>().Value;
                    continue;
                }

                rhs = slot.As<NumberSlotValue>().Value;

                result = _calculator.Exec(result.Value, op, rhs);
            }

            return result ?? 0;
        }
    }
}
