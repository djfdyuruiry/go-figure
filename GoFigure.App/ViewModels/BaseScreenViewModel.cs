using System.Threading.Tasks;

using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    public class BaseScreenViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        protected BaseScreenViewModel(
            IEventAggregator eventAggregator
        )
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        protected async Task PublishMessage<T>(T message) =>
            await _eventAggregator.PublishOnCurrentThreadAsync(message);
    }
}
