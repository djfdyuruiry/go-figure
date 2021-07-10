using System;
using System.Collections.Generic;

using GoFigure.App.Model;
using GoFigure.App.Model.Settings;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils;

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

            // generate solutions until target is within bounds
            while (result < skillLevel.MinTarget || result > skillLevel.MaxTarget)
            { 
                var slots = new List<ISolutionSlotValue>();
                var current = GenerateFirstSlot(slots, random, skillLevel, level);
                var operatorCounts = new Dictionary<Operator, int>();

                for (int i = 0; i < OperatorsPerSolution; i++)
                {
                    GenerateNextTwoSlots(
                        random,
                        slots,
                        skillLevel,
                        level,
                        ref current,
                        operatorCounts
                    );
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
            ref int current,
            IDictionary<Operator, int> operatorCounts
        )
        {
            var result = -1;
            var step = 0;
            Operator? randomOp = null;

            while (result < 0)
            {
                // pick a random operator that is not present, or is only present once
                while (
                    !randomOp.HasValue 
                    || operatorCounts.GetOrSet(randomOp.Value, 0) > 1
                )
                {
                    randomOp = Operators[random.Next(0, Operators.Length - 1)];
                }

                operatorCounts[randomOp.Value]++;

                step = random.Next(skillLevel.MinRandom, skillLevel.MaxRandom);

                result = _calculator.Exec(current, randomOp.Value, step);
            }

            slots.Add(
                new OperatorSlotValue()
                {
                    Value = randomOp.Value
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
