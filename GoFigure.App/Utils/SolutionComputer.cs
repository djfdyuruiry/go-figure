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
            if (solution.Slots is null)
            {
                return 0;
            }

            Calculation calculation = null;

            foreach (var slot in solution.Slots)
            {
                ResultFor(slot, ref calculation);
            }

            return calculation?.LeftHandSide ?? 0;
        }
        
        private void ResultFor(ISolutionSlotValue slot, ref Calculation calculation)
        {
            if (slot is null)
            {
                return;
            }

            if (calculation is null)
            {
                calculation = new Calculation();

                calculation.LeftHandSide = slot.As<NumberSlotValue>().Value;
            }

            if (slot is OperatorSlotValue)
            {
                calculation.Operator = slot.As<OperatorSlotValue>().Value;
                return;
            }

            calculation.RightHandSide = slot.As<NumberSlotValue>().Value;

            calculation.LeftHandSide = _calculator.Exec(calculation);
        }
    }
}
