using Caliburn.Micro;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    public class MenuBarViewModel : GameMenuViewModel
    {
        public MenuBarViewModel(
            IEventAggregatorWrapper eventAggregator,
            IWindowManager windowManager,
            IMessageBoxManager messageBoxManager,
            ISolutionGenerator generator,
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
