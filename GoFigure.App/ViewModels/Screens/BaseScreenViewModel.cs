using System.Threading.Tasks;

using Caliburn.Micro;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Screens
{
  public class BaseScreenViewModel : Screen
  {
    private readonly IEventAggregatorWrapper _eventAggregator;

    protected BaseScreenViewModel(
      IEventAggregatorWrapper eventAggregator
    )
    {
      _eventAggregator = eventAggregator;
      _eventAggregator.SubscribeOnPublishedThread(this);
    }

    protected async Task PublishMessage<T>(T message) =>
      await _eventAggregator.PublishOnCurrentThreadAsync(message);
  }
}
