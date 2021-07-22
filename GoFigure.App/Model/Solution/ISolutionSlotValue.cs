namespace GoFigure.App.Model.Solution
{
  public abstract class ISolutionSlotValue
  {
    public U As<U>() where U : class =>
      this as U;
  }
}
