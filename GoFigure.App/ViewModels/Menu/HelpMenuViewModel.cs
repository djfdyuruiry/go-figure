using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Menu
{
  public class HelpMenuViewModel : BaseControlViewModel, IHelpMenuViewModel
  {
    public HelpMenuViewModel(IEventAggregatorWrapper eventAggregator) : base(eventAggregator)
    {
    }

    public void ShowHelp()
    {
      // TODO: show help contents
    }
  }
}
