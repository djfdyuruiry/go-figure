using GoFigure.App.Model.Solution;

namespace GoFigure.App.Model.Messages
{
    class NewGameStartedMessage
    {
        public int Level { get; set; }

        public SolutionPlan Solution { get; set; }
    }
}
