namespace GoFigure.App.Model.Solution
{
    abstract class ISolutionSlotValue
    {
        public U As<U>() where U : class =>
            this as U;
    }
}
