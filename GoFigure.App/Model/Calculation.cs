namespace GoFigure.App.Model
{
  public class Calculation
  {
    public int LeftHandSide { get; set; }

    public Operator Operator { get; set; }

    public int RightHandSide { get; set; }

    public Calculation()
    {
    }

    public Calculation(Calculation calculation)
    {
      LeftHandSide = calculation.LeftHandSide;
      Operator = calculation.Operator;
      RightHandSide = calculation.RightHandSide;
    }
  }
}
