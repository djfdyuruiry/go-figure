using System.Threading.Tasks;

using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    public class BaseControlViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        protected BaseControlViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        protected async Task PublishMessage<T>(T message) =>
            await _eventAggregator.PublishOnCurrentThreadAsync(message);
    }
}
