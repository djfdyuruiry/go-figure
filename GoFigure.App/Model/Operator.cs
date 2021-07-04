namespace GoFigure.App.Model
{
    enum Operator
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
