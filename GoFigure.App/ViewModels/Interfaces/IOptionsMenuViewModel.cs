using System.Windows.Controls;

namespace GoFigure.App.ViewModels.Interfaces
{
    public interface IOptionsMenuViewModel : ISkillMenuViewModel
    {
        bool HintEnabled { get; set; }

        bool LeftToRightPrecedence { get; }

        bool OperatorPrecedence { get; }

        bool SoundEnabled { get; }

        void ClearSolution();

        void ShowSolutionHint();

        void ToggleSound();

        void UseLeftToRightPrecedence(MenuItem view);

        void UseOperatorPrecedence(MenuItem view);
    }
}
