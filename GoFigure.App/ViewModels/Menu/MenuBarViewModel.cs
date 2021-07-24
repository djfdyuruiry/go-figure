using Caliburn.Micro;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Menu
{
  public class MenuBarViewModel : GameMenuViewModel, IMenuBarViewModel
  {
    public MenuBarViewModel(
      IEventAggregatorWrapper eventAggregator,
      IWindowManager windowManager,
      IMessageBoxManager messageBoxManager,
      ISolutionGenerator generator,
      ISoundEffectPlayer soundEffectPlayer,
      IApplicationManager applicationManager,
      IHighScoresScreenViewModel highScores,
      GameSettings gameSettings
    ) : base(
      eventAggregator,
      windowManager,
      messageBoxManager,
      soundEffectPlayer,
      generator,
      applicationManager,
      highScores,
      gameSettings
    )
    {
    }
  }
}
