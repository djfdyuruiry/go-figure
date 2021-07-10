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
                            IHandle<ZeroDataMessage>
    {
        private const string TimeFormat = @"mm\:ss";
        private const string ScorePlaceholder = "Paused";
        private const string TargetPlaceholder = "???";
        private static readonly TimeSpan OneSecond = new TimeSpan(TicksPerSecond);

        private readonly SolutionComputer _computer;

        private int _gameScore;
        private string _score;
        
        private bool _timerRunning;
        private System.Timers.Timer _timer;
        private TimeSpan _currentTime;

        private int _solutionTarget;
        private string _target;

        public string Score
        {
            get => _score;
            set
            {
                _score = value;

                NotifyOfPropertyChange(() => Score);
            }
        }

        public string Time => _currentTime.ToString(TimeFormat);

        public string Target
        {
            get => _target;
            set
            {
                _target = value;

                NotifyOfPropertyChange(() => Target);
            }
        }

        public StatusViewModel(IEventAggregator eventAggregator, SolutionComputer computer) : base(eventAggregator) =>
            _computer = computer;

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            _solutionTarget = _computer.ResultFor(message.Solution);

            Score = $"{_gameScore}";
            Target = $"{_solutionTarget}";

            SetupTimer();
        }

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
        {
            if (!message.IsOneOf(
                ZeroDataMessage.PauseGame,
                ZeroDataMessage.ResumeGame
            ))
            {
                return;
            }

            if (message is ZeroDataMessage.PauseGame)
            {
                if (_timerRunning)
                {
                    _timer.Stop();
                    _timerRunning = false;
                }

                Score = ScorePlaceholder;
                Target = TargetPlaceholder;

                return;
            }

            Score = $"{_gameScore}";
            Target = $"{_solutionTarget}";

            if (!_timerRunning)
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
            _timer = new System.Timers.Timer(OneSecond.TotalMilliseconds);

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
