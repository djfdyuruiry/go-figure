using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels
{
  public class GameViewModel : BaseControlViewModel, IGameViewModel
  {
    public IStatusViewModel Status { get; private set; }

    public ISolutionViewModel Solution { get; private set; }

    public IControlsViewModel Controls { get; private set; }

    public ILevelMeterViewModel LevelMeter { get; private set; }

    public GameViewModel(
      IEventAggregatorWrapper eventAggregator,
      IStatusViewModel status,
      ISolutionViewModel solution,
      IControlsViewModel controls,
      ILevelMeterViewModel levelMeter
    ) : base(eventAggregator)
    {
      Status = status;
      Solution = solution;
      Controls = controls;
      LevelMeter = levelMeter;
    }
  }
}
