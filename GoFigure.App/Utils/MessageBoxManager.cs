using System.Windows;

using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    class MessageBoxManager
    {
        public void ShowInformation(string message) =>
            MessageBox.Show(
                message,
                MessageBoxHeader,
                OK,
                Information
            );
    }
}
