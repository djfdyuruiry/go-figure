namespace GoFigure.App.Model.Solution
{
    class NumberSlotValue : ISolutionSlotValue
    {
        public int Value { get; set; }

        public override string ToString() =>
            $"{Value}";
    }
}
