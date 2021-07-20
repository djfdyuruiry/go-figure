using System.Threading.Tasks;

namespace GoFigure.App.ViewModels
{
    public interface IEventAggregatorWrapper
    {
        Task PublishOnCurrentThreadAsync(object message);

        void SubscribeOnPublishedThread(object subscriber);
    }
}
