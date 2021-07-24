using System.Threading.Tasks;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IGameMenuViewModel : IOptionsMenuViewModel
  {
    bool CanPause { get; set; }

    void CloseApp();

    Task StartNewGame();

    Task PauseOrResumeGame();

    Task ShowHighScores();
  }
}
