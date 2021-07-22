using Xunit;

using GoFigure.App.Model;
using GoFigure.App.Utils;

namespace GoFigure.Tests.Utils
{
  public class CalculatorTests
  {
    private Calculator _calculator;

    public CalculatorTests()
    {
      _calculator = new Calculator();
    }

    [Theory]
    [InlineData(Operator.Add, 8)]
    [InlineData(Operator.Subtract, 4)]
    [InlineData(Operator.Multiply, 12)]
    [InlineData(Operator.Divide, 3)]
    public void When_Exec_IsCalled_WithCalculation_Then_ResultIsCorrect(Operator op, int expectedResult)
    {
      var result = _calculator.Exec(new Calculation
      {
        LeftHandSide = 6,
        Operator = op,
        RightHandSide = 2
      });

      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void When_Exec_IsCalled_WithNullCalculation_Then_ResultIsZero()
    {
      Calculation calc = null;

      Assert.Equal(0, _calculator.Exec(calc));
    }

    [Fact]
    public void When_Exec_IsCalled_WithExpression_Then_ResultIsCorrect() =>
      Assert.Equal(12, _calculator.Exec("2 * 6 - 4 * 2 + 8"));

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\t\t")]
    [InlineData(null)]
    [InlineData("\n\n")]
    public void When_Exec_IsCalled_WithEmptyExpression_Then_ResultIsZero(string expression) =>
      Assert.Equal(0, _calculator.Exec(expression));
  }
}
