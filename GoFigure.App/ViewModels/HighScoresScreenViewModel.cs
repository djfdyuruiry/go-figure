using Caliburn.Micro;
using GoFigure.App.Utils;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels
{
    public class HighScoresScreenViewModel : BaseScreenViewModel
    {
        public HighScoresScreenViewModel(
            IEventAggregatorWrapper eventAggregator
        ) : base(eventAggregator)
        {
        }
    }
}
