namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IGameViewModel
  {
    IStatusViewModel Status { get; }

    ISolutionViewModel Solution { get; }

    ILevelMeterViewModel LevelMeter { get; }

    IControlsViewModel Controls { get; }
  }
}
