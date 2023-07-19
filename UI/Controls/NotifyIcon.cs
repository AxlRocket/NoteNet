using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoteNet.UI.Controls
{
    internal class NotifyIcon : TaskbarIcon
    {
        public NotifyIcon()
        {
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            ToolTipText = "NoteNet";

            DoubleClickCommand = new ShowNoteNet();

            ContextMenu = new ContextMenu();
            MenuItem Open = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Display"]
            },
            Close = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Close"]
            };

            Binding b = new Binding("Parent")
            {
                RelativeSource = RelativeSource.Self
            };

            Open.SetBinding(MenuItem.CommandParameterProperty, b);
            Open.Click += OpenApp_Click;

            Close.SetBinding(MenuItem.CommandParameterProperty, b);
            Close.Click += CloseApp_Click;

            ContextMenu.Items.Add(Open);
            ContextMenu.Items.Add(Close);
        }

        private void OpenApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
            
        }
    }
}
