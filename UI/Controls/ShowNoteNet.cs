using System;
using System.Windows.Input;
using System.Windows;

namespace NoteNet.UI.Controls
{
    internal class ShowNoteNet : ICommand
    {
        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Console.WriteLine("OK ?");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
