using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    abstract class BaseViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        protected BaseViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.SubscribeOnPublishedThread(this);
        }
    }
}
