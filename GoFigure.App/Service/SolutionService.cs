using GoFigure.App.Model;
using GoFigure.App.Model.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFigure.App.Service
{
    class SolutionService
    {
        private static readonly Operator[] Operators = Enum.GetValues(typeof(Operator)) as Operator[]; 

        public Solution Generate()
        {
            var solution = new Solution();
            var random = new Random();

            solution.Target = random.Next(8, 300);

            var slots = new List<ISolutionSlotValue>();
            var countDown = solution.Target;

            for (int i = 0; i < 3; i++)
            {
                var randomOp = Operators[random.Next(0, Operators.Length - 1)];
                var result = countDown;
                var step = 0;
                var operatorSlot = new OperatorSlotValue();

                step = random.Next(1, countDown);

                if (randomOp == Operator.Divide && countDown != 0)
                {
                    result *= step;

                    operatorSlot.Character = '/';
                }
                else if (randomOp == Operator.Multiply)
                {
                    result /= step;

                    operatorSlot.Character = '*';
                }
                else if (randomOp == Operator.Add)
                {
                    result -= step;

                    operatorSlot.Character = '+';
                }
                else if (randomOp == Operator.Subtract)
                {
                    result += step;

                    operatorSlot.Character = '-';
                }

                if (result > solution.Target)
                {
                    continue;
                }

                operatorSlot.Value = randomOp;

                slots.Add(
                    new NumberSlotValue
                    {
                        Value = step
                    }
                );

                slots.Add(operatorSlot);

                countDown = result;
            }

            return solution;
        }
    }
}
