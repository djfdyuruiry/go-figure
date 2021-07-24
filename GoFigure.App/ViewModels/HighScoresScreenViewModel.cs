using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels
{
  public class HighScoresScreenViewModel : BaseScreenViewModel, IHighScoresScreenViewModel
  {
    public HighScoresScreenViewModel(
      IEventAggregatorWrapper eventAggregator
    ) : base(eventAggregator)
    {
    }
  }
}
