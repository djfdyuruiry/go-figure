using System;
using System.Collections.Generic;

using GoFigure.App.Model;
using GoFigure.App.Model.Solution;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    class SolutionGenerator
    {
        private static readonly Operator[] Operators = Enum.GetValues(typeof(Operator)) as Operator[];

        private readonly Calculator _calculator;

        public SolutionGenerator(Calculator calculator)
        {
            _calculator = calculator;
        }

        public SolutionPlan Generate()
        {
            var random = new Random();

            var slots = new List<ISolutionSlotValue>();
            var current = random.Next(MinRandom, MaxRandom);

            slots.Add(
                new NumberSlotValue
                {
                    Value = current
                }
            );

            for (int i = 0; i < OperatorsPerSolution; i++)
            {
                var randomOp = Operators[random.Next(0, Operators.Length - 1)];
                var step = random.Next(MinRandom, MaxRandom);

                var result = _calculator.Exec(current, randomOp, step);

                if (result < 0)
                {
                    continue;
                }

                slots.Add(
                    new OperatorSlotValue()
                    {
                        Value = randomOp
                    }
                );

                slots.Add(
                    new NumberSlotValue
                    {
                        Value = step
                    }
                );

                current = result;
            }

            return new SolutionPlan
            {
                Slots = slots
            };
        }
    }
}
