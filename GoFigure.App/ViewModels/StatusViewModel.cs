using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    class StatusViewModel : BaseViewModel
    {
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

        public StatusViewModel(IEventAggregator eventAggregator) : base(eventAggregator) =>
            Time = "00:00";
    }
}
