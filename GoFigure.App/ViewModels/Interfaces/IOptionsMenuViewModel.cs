using System.Threading.Tasks;
using System.Windows;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IOptionsMenuViewModel : ISkillMenuViewModel
  {
    bool HintEnabled { get; set; }

    bool LeftToRightPrecedence { get; }

    bool OperatorPrecedence { get; }

    bool SoundEnabled { get; }

    Task ClearSolution();

    Task ShowSolutionHint();

    Task ToggleSound();

    Task UseLeftToRightPrecedence(DependencyObject view);

    Task UseOperatorPrecedence(DependencyObject view);
  }
}
