using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils.Interfaces
{
    public interface ISolutionGenerator
    {
        SolutionPlan Generate(int level, int numberToExclude);
    }
}
