using GoFigure.App.Model.Solution;

namespace GoFigure.App.Model.Messages
{
    class NewGameStartedMessage
    {
        public SolutionPlan Solution { get; set; }

        public int Level { get; set; }
    }
}
