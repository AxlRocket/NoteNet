using System;
using System.Windows;
using System.Windows.Input;

namespace NoteNet.UI.Controls
{
    internal class ShowNoteNet : ICommand
    {
        public void Execute(object parameter)
        {
            if (!CheckWindow.IsWindowOpen<Window>("Main"))
            {
                MainWindow MW = new MainWindow();
                MW.Show();
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
