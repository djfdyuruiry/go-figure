using System.Threading.Tasks;
using System.Windows.Input;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IAppScreenViewModel
  {
    IMenuBarViewModel MenuBar { get; }

    IGameViewModel Game { get; }

    Task KeyPressed(KeyEventArgs e);

    void SaveCurrentSettings();
  }
}
