using System.Windows;

using GoFigure.App.Utils.Interfaces;

namespace GoFigure.App.Utils
{
  public class ApplicationManager : IApplicationManager
  {
    public void Shutdown() =>
      Application.Current.Shutdown();
  }
}
