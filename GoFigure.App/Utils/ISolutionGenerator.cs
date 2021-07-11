using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils
{
    public interface ISolutionGenerator
    {
        SolutionPlan Generate(int level);
    }
}