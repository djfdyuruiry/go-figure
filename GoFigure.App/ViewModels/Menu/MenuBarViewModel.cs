using Caliburn.Micro;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Menu
{
    public class MenuBarViewModel : GameMenuViewModel, IMenuBarViewModel
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
