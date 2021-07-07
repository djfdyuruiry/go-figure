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

        public SolutionPlan Generate(int level)
        {
            var random = new Random();

            var slots = new List<ISolutionSlotValue>();
            var current = GenerateFirstSlot(slots, random);

            for (int i = 0; i < OperatorsPerSolution; i++)
            {
                GenerateNextTwoSlots(random, slots, ref current);
            }

            return new SolutionPlan
            {
                Slots = slots
            };
        }

        private int GenerateFirstSlot(List<ISolutionSlotValue> slots, Random random)
        {
            var firstNumber = random.Next(MinRandom, MaxRandom);

            slots.Add(
                new NumberSlotValue
                {
                    Value = firstNumber
                }
            );

            return firstNumber;
        }

        private void GenerateNextTwoSlots(Random random, List<ISolutionSlotValue> slots, ref int current)
        {
            var result = -1;
            var step = 0;
            var randomOp = Operator.Divide;

            while (result < 0)
            {
                randomOp = Operators[random.Next(0, Operators.Length - 1)];
                step = random.Next(MinRandom, MaxRandom);

                result = _calculator.Exec(current, randomOp, step);
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
    }
}
