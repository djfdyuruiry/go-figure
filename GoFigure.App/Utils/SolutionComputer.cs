using GoFigure.App.Model;
using GoFigure.App.Model.Settings;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils.Interfaces;

namespace GoFigure.App.Utils
{
  public class SolutionComputer : ISolutionComputer
  {
    private readonly ICalculator _calculator;
    private readonly GameSettings _gameSettings;

    public SolutionComputer(ICalculator calculator, GameSettings gameSettings)
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

    private int ResolveSolutionUsingOperatorPrecedence(SolutionPlan solution) =>
      _calculator.Exec(solution.ToString());

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
