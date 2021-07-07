using GoFigure.App.Model;
using GoFigure.App.Model.Solution;
using System;

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

            return ResolveSolution(solution);
        }

        private int ResolveSolution(SolutionPlan solution)
        {
            Calculation calculation = null;

            foreach (var slot in solution.Slots)
            {
                if (slot is null)
                {
                    continue;
                }

                if (calculation is null)
                {
                    calculation = new Calculation();

                    calculation.LeftHandSide = slot.As<NumberSlotValue>().Value;
                    continue;
                }

                if (slot is OperatorSlotValue)
                {
                    calculation.Operator = slot.As<OperatorSlotValue>().Value;
                    continue;
                }

                calculation.RightHandSide = slot.As<NumberSlotValue>().Value;

                calculation.LeftHandSide = _calculator.Exec(calculation);
            }

            return calculation?.LeftHandSide ?? 0;
        }
    }
}
