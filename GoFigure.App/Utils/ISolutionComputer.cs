using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils
{
    public interface ISolutionComputer
    {
        int ResultFor(SolutionPlan solution);
    }
}