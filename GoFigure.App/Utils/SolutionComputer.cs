using System;

using GoFigure.App.Model;
using GoFigure.App.Model.Settings;
using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils
{
    class SolutionComputer
    {
        private readonly Calculator _calculator;
        private readonly GameSettings _gameSettings;

        public SolutionComputer(Calculator calculator, GameSettings gameSettings)
        {
            _calculator = calculator;
            _gameSettings = gameSettings;
        }

        public int ResultFor(SolutionPlan solution)
        {
            if (solution?.Slots is null)
            {
                return 0;
            }

            return ResolveSolution(solution);
        }

        private int ResolveSolution(SolutionPlan solution) =>
            _gameSettings.UseOperatorPrecedence
                ? ResolveSolutionUsingOperatorPrecedence(solution)
                : ResolveSolutionUsingLeftToRightPrecedence(solution);

        private int ResolveSolutionUsingOperatorPrecedence(SolutionPlan solution)
        {
            // TODO: implement operator precedence solution
            throw new NotImplementedException();
        }

        private int ResolveSolutionUsingLeftToRightPrecedence(SolutionPlan solution)
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
