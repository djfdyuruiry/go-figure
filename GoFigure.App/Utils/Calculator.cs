using System.Data;

using GoFigure.App.Model;
using GoFigure.App.Utils.Interfaces;

namespace GoFigure.App.Utils
{
  public class Calculator : ICalculator
  {
    public int Exec(int lhs, Operator op, int rhs) =>
      op switch
      {
        Operator.Divide => lhs / rhs,
        Operator.Multiply => lhs * rhs,
        Operator.Add => lhs + rhs,
        Operator.Subtract => lhs - rhs,
        _ => 0
      };

    public int Exec(Calculation calculation) =>
      calculation is null
      ? 0
      : Exec(
        calculation.LeftHandSide,
        calculation.Operator,
        calculation.RightHandSide
        );

    public int Exec(string expression)
    {
      if (string.IsNullOrWhiteSpace(expression))
      {
        return 0;
      }

      using var table = new DataTable();
      var result = table.Compute(expression, string.Empty);

      if (result is int)
      {
        return (int)result;
      }

      return 0;
    }
  }
}
