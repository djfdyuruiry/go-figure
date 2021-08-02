using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Screens
{
  public class HighScoresScreenViewModel : BaseScreenViewModel, IHighScoresScreenViewModel
  {
    public bool ScoresPresent => false;
    
    public HighScoresScreenViewModel(
      IEventAggregatorWrapper eventAggregator
    ) : base(eventAggregator)
    {
    }
  }
}
