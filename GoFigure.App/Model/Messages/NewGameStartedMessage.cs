using GoFigure.App.Model.Solution;

namespace GoFigure.App.Model.Messages
{
    public class NewGameStartedMessage
    {
        public int Level { get; set; }

        public SolutionPlan Solution { get; set; }
    }
}
