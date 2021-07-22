using System.Threading.Tasks;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IGameMenuViewModel : IOptionsMenuViewModel
  {
    bool CanPause { get; set; }

    void CloseApp();

    void PauseOrResumeGame();

    Task PublishPauseOrResumeGameMessage();

    void ShowHighScores();

    void StartNewGame();
  }
}