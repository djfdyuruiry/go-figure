using Caliburn.Micro;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    public class MenuBarViewModel : GameMenuViewModel
    {
        public MenuBarViewModel(
            IEventAggregator eventAggregator,
            MessageBoxManager messageBoxManager,
            SolutionGenerator generator,
            GameSettings gameSettings
        ) : base(eventAggregator, messageBoxManager, generator, gameSettings)
        {
        }
    }
}
