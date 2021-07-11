namespace GoFigure.App.Model.Solution
{
    class OperatorSlotValue : ISolutionSlotValue
    {
        public Operator Value { get; set; }

        public override string ToString() =>
            $"{Value.ToCharacter()}";
    }
}
