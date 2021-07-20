using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    public class GameViewModel : BaseControlViewModel
    {
        public StatusViewModel Status { get; private set; }

        public SolutionViewModel Solution { get; private set; }

        public ControlsViewModel Controls { get; private set; }

        public LevelMeterViewModel LevelMeter { get; private set; }

        public GameViewModel(
            IEventAggregatorWrapper eventAggregator,
            StatusViewModel status,
            SolutionViewModel solution,
            ControlsViewModel controls,
            LevelMeterViewModel levelMeter
        ) : base(eventAggregator)
        {
            Status = status;
            Solution = solution;
            Controls = controls;
            LevelMeter = levelMeter;
        }
    }
}
