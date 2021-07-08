using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    class MenuBarViewModel : GameMenuViewModel
    {
        public MenuBarViewModel(
            IEventAggregator eventAggregator,
            SolutionGenerator generator,
            GameSettings gameSettings
        ) : base(eventAggregator, generator, gameSettings)
        {
        }
    }
}
