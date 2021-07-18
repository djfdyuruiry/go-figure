using System.Data;

using GoFigure.App.Model;

namespace GoFigure.App.Utils
{
    public class Calculator : ICalculator
    {
        public int Exec(int lhs, Operator op, int rhs)
        {
            if (op == Operator.Divide)
            {
                return lhs / rhs;
            }
            else if (op == Operator.Multiply)
            {
                return lhs * rhs;
            }
            else if (op == Operator.Add)
            {
                return lhs + rhs;
            }

            return lhs - rhs;
        }

        public int Exec(Calculation calculation) =>
            calculation is null
            ? 0
            : Exec(calculation.LeftHandSide,
                 calculation.Operator,
                 calculation.RightHandSide);

        public int Exec(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return 0;
            }

            using var table = new DataTable();
            var result = table.Compute($"{expression}", string.Empty);

            if (result is int)
            {
                return (int)result;
            }

            return 0;
        }
    }
}
