using System.Collections.Generic;

using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.Model.Solution;
using GoFigure.App.Model;

namespace GoFigure.Tests.Utils
{
    public class SolutionComputerTests
    {
        private GameSettings _gameSettings;

        private SolutionComputer _solutionComputer;

        private SolutionPlan _testSolutionPlan;

        public SolutionComputerTests()
        {
            _gameSettings = new GameSettings();

            _solutionComputer = new SolutionComputer(
                new Calculator(),
                _gameSettings
            );

            // 12 + 2 * 6 / 3
            _testSolutionPlan = new SolutionPlan
            {
                Slots = new List<ISolutionSlotValue>
                {
                    new NumberSlotValue
                    {
                        Value = 12
                    },
                    new OperatorSlotValue
                    {
                        Value = Operator.Add
                    },
                    new NumberSlotValue
                    {
                        Value = 2
                    },
                    new OperatorSlotValue
                    {
                        Value = Operator.Multiply
                    },
                    new NumberSlotValue
                    {
                        Value = 6
                    },
                    new OperatorSlotValue
                    {
                        Value = Operator.Divide
                    },
                    new NumberSlotValue
                    {
                        Value = 3
                    }
                }
            };
        }

        // 14 * 6 / 3
        // 84 / 3
        // 28
        [Fact]
        public void When_LeftToRightPrecendeceUsed_And_ResultForIsCalled_Then_ResultIsCorrect()
        {
            var result = _solutionComputer.ResultFor(_testSolutionPlan);

            Assert.Equal(28, result);
        }

        // 12 + 2 * 2
        // 12 + 4
        // 16
        [Fact]
        public void When_OperatorPrecendeceUsed_And_ResultForIsCalled_Then_ResultIsCorrect()
        {
            _gameSettings.UseOperatorPrecedence = true;

            var result = _solutionComputer.ResultFor(_testSolutionPlan);

            Assert.Equal(16, result);
        }
    }
}
