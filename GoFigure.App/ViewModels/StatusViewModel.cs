using System.Threading.Tasks;
using System.Threading;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels
{
    class StatusViewModel : BaseViewModel, IHandle<NewGameStartedMessage>
    {
        private readonly SolutionComputer _computer;

        private int _score;
        private string _time;
        private int _target;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;

                NotifyOfPropertyChange(() => Score);
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                _time = value;

                NotifyOfPropertyChange(() => Time);
            }
        }

        public int Target
        {
            get => _target;
            set
            {
                _target = value;

                NotifyOfPropertyChange(() => Target);
            }
        }

        public StatusViewModel(IEventAggregator eventAggregator, SolutionComputer computer) : base(eventAggregator)
        {
            _computer = computer;

            Time = "00:00";
        }

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _) =>
            Target = _computer.ResultFor(message.Solution);
    }
}
