namespace GoFigure.App.Model.Solution
{
  public class NumberSlotValue : ISolutionSlotValue
  {
    public int Value { get; set; }

    public override string ToString() =>
      $"{Value}";
  }
}
