using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    class GameViewModel : BaseViewModel
    {
        private int _score;
        private string _time;
        private int _target;

        public StatusViewModel Status { get; private set; }

        public SolutionViewModel Solution { get; private set; }

        public LevelMeterViewModel LevelMeter { get; private set; }

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

        public GameViewModel(
            IEventAggregator eventAggregator,
            StatusViewModel status,
            SolutionViewModel solution,
            LevelMeterViewModel levelMeter
        ) : base(eventAggregator)
        {
            Status = status;
            Solution = solution;
            LevelMeter = levelMeter;

            Time = "00:00";
        }
    }
}
