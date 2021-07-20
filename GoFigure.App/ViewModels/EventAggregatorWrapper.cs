using Caliburn.Micro;

using System.Threading.Tasks;

namespace GoFigure.App.ViewModels
{
    public class EventAggregatorWrapper : IEventAggregatorWrapper
    {
        private readonly IEventAggregator _eventAggregator;

        public EventAggregatorWrapper(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void SubscribeOnPublishedThread(object subscriber) =>
           _eventAggregator.SubscribeOnPublishedThread(subscriber);

        public async Task PublishOnCurrentThreadAsync(object message) =>
            await _eventAggregator.PublishOnCurrentThreadAsync(message);
    }
}
