using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Controls
{
  public class BaseControlViewModel : PropertyChangedBase
  {
    private readonly IEventAggregatorWrapper _eventAggregator;

    protected BaseControlViewModel(IEventAggregatorWrapper eventAggregator)
    {
      _eventAggregator = eventAggregator;
      _eventAggregator.SubscribeOnPublishedThread(this);
    }

    protected async Task PublishMessage<T>(T message) =>
      await _eventAggregator.PublishOnCurrentThreadAsync(message);
  }
}
