using System.Threading;
using System.Threading.Tasks;

using GoFigure.App.Model.Messages;

using Caliburn.Micro;

namespace GoFigure.App.ViewModels.Menu
{
    class OptionsMenuViewModel : SkillMenuViewModel,
                                 IHandle<NewGameStartedMessage>,
                                 IHandle<ZeroDataMessages>
    {
        private bool _hintEnabled;

        public bool HintEnabled
        {
            get => _hintEnabled;
            set
            {
                _hintEnabled = value;

                NotifyOfPropertyChange(() => HintEnabled);
            }
        }

        public OptionsMenuViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public async void ShowSolutionHint() =>
            await PublishMessage(ZeroDataMessages.ShowSolutionHint);

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _) =>
            HintEnabled = true;

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken _)
        {
            if (message != ZeroDataMessages.NoHintsLeft)
            {
                return;
            }

            HintEnabled = false;
        }
    }
}
