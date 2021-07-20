using Caliburn.Micro;
using GoFigure.App.Utils;

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
