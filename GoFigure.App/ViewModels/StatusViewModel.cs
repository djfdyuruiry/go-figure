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
    class StatusViewModel : BaseViewModel,
                            IHandle<NewGameStartedMessage>,
                            IHandle<ZeroDataMessages>
    {
        private static readonly TimeSpan OneSecond = new TimeSpan(TicksPerSecond);

        private readonly SolutionComputer _computer;

        private int _score;
        private bool _timerRunning;
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

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken _)
        {
            if (message != ZeroDataMessages.PauseGame)
            {
                return;
            }

            if (_timerRunning)
            {
                _timer.Stop();
                _timerRunning = false;
            }
            else
            {
                _timer.Start();
                _timerRunning = true;
            }
        }

        private void SetupTimer()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timerRunning = false;

            _currentTime = new TimeSpan();
            _timer = new System.Timers.Timer(1000);

            _timer.Elapsed += IncrementTime;

            NotifyOfPropertyChange(() => Time);

            _timer.Start();
            _timerRunning = true;
        }

        private void IncrementTime(object _, ElapsedEventArgs __)
        {
            try
            {
                _currentTime = _currentTime.Add(OneSecond);

                NotifyOfPropertyChange(() => Time);
            }
            catch
            {
                // ignore timer thread errors
                // TODO: cancel timer on 'quit'
            }
        }
    }
}
