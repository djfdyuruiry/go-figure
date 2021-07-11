﻿using GoFigure.App.Model;

namespace GoFigure.App.Utils
{
    public interface ICalculator
    {
        int Exec(Calculation calculation);
        int Exec(int lhs, Operator op, int rhs);
        int Exec(string expression);
    }
}