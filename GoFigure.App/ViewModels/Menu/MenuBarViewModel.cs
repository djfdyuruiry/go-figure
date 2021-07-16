using Caliburn.Micro;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    public class MenuBarViewModel : GameMenuViewModel
    {
        public MenuBarViewModel(
            IEventAggregator eventAggregator,
            IWindowManager windowManager,
            MessageBoxManager messageBoxManager,
            SolutionGenerator generator,
            ISoundEffectPlayer soundEffectPlayer,
            HighScoresScreenViewModel highScores,
            GameSettings gameSettings
        ) : base(
            eventAggregator,
            windowManager,
            messageBoxManager,
            generator,
            soundEffectPlayer,
            highScores,
            gameSettings
        )
        {
        }
    }
}
