namespace GoFigure.App.Model
{
    public enum Operator
    {
        [Character('+')]
        Add,
        [Character('-')]
        Subtract,
        [Character('*')]
        Multiply,
        [Character('/')]
        Divide
    }
}
