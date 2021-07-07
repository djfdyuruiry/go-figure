using Caliburn.Micro;

using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    class MenuBarViewModel : GameMenuViewModel
    {
        public MenuBarViewModel(
            IEventAggregator eventAggregator,
            SolutionGenerator generator
        ) : base(eventAggregator, generator)
        {
        }
    }
}
