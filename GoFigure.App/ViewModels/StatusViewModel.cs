using System;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

using static System.TimeSpan;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels
{
    class StatusViewModel : BaseViewModel, IHandle<NewGameStartedMessage>
    {
        private static readonly TimeSpan OneSecond = new TimeSpan(TicksPerSecond);

        private readonly SolutionComputer _computer;

        private int _score;
        private System.Timers.Timer _timer;
        private TimeSpan _currentTime;
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

        public string Time => _currentTime.ToString("mm\\:ss");

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
        }

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            Target = _computer.ResultFor(message.Solution);

            SetupTimer();
        }

        private void SetupTimer()
        {
            _timer?.Stop();
            _timer?.Dispose();

            _currentTime = new TimeSpan();
            _timer = new System.Timers.Timer(1000);

            _timer.Elapsed += IncrementTime;

            NotifyOfPropertyChange(() => Time);
            _timer.Start();
        }

        private void IncrementTime(object _, ElapsedEventArgs __)
        {
            _currentTime = _currentTime.Add(OneSecond);

            NotifyOfPropertyChange(() => Time);
        }
    }
}
