﻿using System.Windows;

namespace GoFigure.App.Utils
{
    public interface IMessageBoxManager
    {
        void ShowInformation(Window window, string message);
        
        MessageBoxResult ShowOkCancel(Window window, string message);
        
        void ShowWarning(Window window, string message);
    }
}
