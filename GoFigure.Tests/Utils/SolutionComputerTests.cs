using System;
using System.Collections.Generic;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.Model.Solution;
using GoFigure.App.Model;

namespace GoFigure.Tests.Utils
{
    public class SolutionComputerTests
    {
        private ICalculator _calculator;
        private GameSettings _gameSettings;

        private SolutionComputer _solutionComputer;

        private SolutionPlan _testSolutionPlan;

        public SolutionComputerTests()
        {
            _calculator = A.Fake<ICalculator>();
            _gameSettings = new GameSettings();

            _solutionComputer = new SolutionComputer(
                _calculator,
                _gameSettings
            );

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

        [Fact]
        public void When_LeftToRightPrecendeceUsed_And_ResultForIsCalled_Then_CalculatorIsCalled() =>
            RunExecTest<Calculation>(3);

        [Fact]
        public void When_LeftToRightPrecendeceUsed_And_ResultForIsCalled_And_SolutionIsNull_Then_CalculatorIsNotCalled()
        {
            _testSolutionPlan = null;

            RunExecTest<Calculation>(0);
        }

        [Fact]
        public void When_LeftToRightPrecendeceUsed_And_ResultForIsCalled_And_SolutionIsNull_Then_ResultIsZero()
        {
            _testSolutionPlan = null;

            Assert.Equal(0, RunExecTest<Calculation>(0));
        }

        [Fact]
        public void When_LeftToRightPrecendeceUsed_And_ResultForIsCalled_And_Then_CalculatorIsCalled_And_CalculationsArePassed()
        {
            var calcs = new List<Calculation>();

            A.CallTo(() => _calculator.Exec(A<Calculation>.Ignored))
                .ReturnsLazily(c =>
                {
                    calcs.Add(new Calculation(c.Arguments[0] as Calculation));

                    return -1;
                });

            _solutionComputer.ResultFor(_testSolutionPlan);

            Assert.True(CalculationMatches(calcs[0], 12, Operator.Add, 2));
            Assert.True(CalculationMatches(calcs[1], -1, Operator.Multiply, 6));
            Assert.True(CalculationMatches(calcs[2], -1, Operator.Divide, 3));
        }

        [Fact]
        public void When_OperatorPrecendeceUsed_And_ResultForIsCalled_Then_CalculatorIsCalled() =>
            RunOperatorPrecedenceTest();

        [Fact]
        public void When_OperatorPrecendeceUsed_And_ResultForIsCalled_And_SolutionIsNull_Then_CalculatorIsNotCalled()
        {
            _testSolutionPlan = null;

            RunOperatorPrecedenceTest(0);
        }

        [Fact]
        public void When_OperatorPrecendeceUsed_And_ResultForIsCalled_And_SolutionIsNull_Then_ResultIsZero()
        {
            _testSolutionPlan = null;

            Assert.Equal(0, RunOperatorPrecedenceTest(0));
        }

        [Fact]
        public void When_OperatorPrecendeceUsed_And_ResultForIsCalled_Then_CalculatorIsCalled_And_SolutionStringIsPassed() =>
            RunOperatorPrecedenceTest(assertion: s => s == "12 + 2 * 6 / 3");

        private int RunExecTest<T>(int timesCalled = 1, Func<T, bool> assertion = null)
        {
            if (assertion is null)
            {
                assertion = _ => true;
            }

            var result = _solutionComputer.ResultFor(_testSolutionPlan);

            A.CallTo(_calculator)
                .Where(c =>
                    c.Method.Name == nameof(ICalculator.Exec)
                        && c.Arguments[0] is T
                        && assertion.Invoke((T)c.Arguments[0])
                )
                .MustHaveHappened(timesCalled, Times.Exactly);

            return result;
        }

        private bool CalculationMatches(Calculation calculation, int left, Operator op, int right) =>
            calculation.LeftHandSide == left
                && calculation.Operator == op
                && calculation.RightHandSide == right;

        private int RunOperatorPrecedenceTest(int timesCalled = 1, Func<string, bool> assertion = null)
        {
            _gameSettings.UseOperatorPrecedence = true;

            return RunExecTest<string>(timesCalled, assertion: assertion);
        }
    }
}
