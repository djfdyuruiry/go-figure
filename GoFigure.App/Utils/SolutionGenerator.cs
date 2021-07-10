using System;
using System.Collections.Generic;

using GoFigure.App.Model;
using GoFigure.App.Model.Settings;
using GoFigure.App.Model.Solution;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    class SolutionGenerator
    {
        private static readonly Operator[] Operators = Enum.GetValues(typeof(Operator)) as Operator[];

        private readonly Calculator _calculator;
        private readonly SolutionComputer _solutionComputer;
        private readonly GameSettings _gameSettings;

        public SolutionGenerator(
            Calculator calculator,
            SolutionComputer solutionComputer,
            GameSettings gameSettings
        )
        {
            _calculator = calculator;
            _solutionComputer = solutionComputer;
            _gameSettings = gameSettings;
        }

        public SolutionPlan Generate(int level)
        {
            var skillLevel = SkillLevels[_gameSettings.CurrentSkill];
            var random = new Random();
            var solution = new SolutionPlan();
            var result = 0;

            while (result < skillLevel.MinTarget || result > skillLevel.MaxTarget)
            { 
                var slots = new List<ISolutionSlotValue>();
                var current = GenerateFirstSlot(slots, random, skillLevel, level);

                for (int i = 0; i < OperatorsPerSolution; i++)
                {
                    GenerateNextTwoSlots(random, slots, skillLevel, level, ref current);
                }

                solution = new SolutionPlan
                {
                    Slots = slots
                };

                result = _solutionComputer.ResultFor(solution);
            }

            return solution;
        }

        private int GenerateFirstSlot(
            List<ISolutionSlotValue> slots,
            Random random,
            SkillRules skillLevel,
            int level
        )
        {
            var firstNumber = random.Next(skillLevel.MinRandom, skillLevel.MaxRandom);

            slots.Add(
                new NumberSlotValue
                {
                    Value = firstNumber
                }
            );

            return firstNumber;
        }

        private void GenerateNextTwoSlots(
            Random random,
            List<ISolutionSlotValue> slots,
            SkillRules skillLevel,
            int level,
            ref int current
        )
        {
            var result = -1;
            var step = 0;
            var randomOp = Operator.Divide;

            while (result < 0)
            {
                randomOp = Operators[random.Next(0, Operators.Length - 1)];
                step = random.Next(skillLevel.MinRandom, skillLevel.MaxRandom);

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
