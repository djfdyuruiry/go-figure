using System.Windows;

namespace GoFigure.App.Utils
{
    public interface IMessageBoxManager
    {
        void ShowInformation(string message);
        MessageBoxResult ShowOkCancel(string message);
        void ShowWarning(string message);
    }
}