using GoFigure.App.Model;

namespace GoFigure.App.Utils
{
    class Calculator
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
    }
}
