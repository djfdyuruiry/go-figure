using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Controls
{
  public class EventAggregatorWrapper : IEventAggregatorWrapper
  {
    private readonly IEventAggregator _eventAggregator;

    public EventAggregatorWrapper(IEventAggregator eventAggregator) =>
      _eventAggregator = eventAggregator;

    public void SubscribeOnPublishedThread(object subscriber) =>
       _eventAggregator.SubscribeOnPublishedThread(subscriber);

    public async Task PublishOnCurrentThreadAsync(object message) =>
      await _eventAggregator.PublishOnCurrentThreadAsync(message);
  }
}
