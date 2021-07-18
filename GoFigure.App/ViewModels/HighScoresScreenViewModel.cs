using Caliburn.Micro;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels
{
    public class HighScoresScreenViewModel : BaseScreenViewModel
    {
        public HighScoresScreenViewModel(
            IEventAggregator eventAggregator
        ) : base(eventAggregator)
        {
        }
    }
}
