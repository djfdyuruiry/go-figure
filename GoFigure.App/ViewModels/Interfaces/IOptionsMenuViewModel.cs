using System.Threading.Tasks;
using System.Windows.Controls;

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

    Task UseLeftToRightPrecedence(MenuItem view);

    Task UseOperatorPrecedence(MenuItem view);
  }
}
