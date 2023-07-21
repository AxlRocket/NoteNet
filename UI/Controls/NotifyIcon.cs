using Hardcodet.Wpf.TaskbarNotification;
using NoteNet.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NoteNet.UI.Controls
{
    internal class NotifyIcon : TaskbarIcon
    {
        public NotifyIcon()
        {
            IconSource = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico"));
            ToolTipText = "NoteNet";

            DoubleClickCommand = new ShowNoteNet();

            ContextMenu = new ContextMenu();
            MenuItem Open = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Display"]
            }, 
            Restart = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Restart"]
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

            Restart.SetBinding(MenuItem.CommandParameterProperty, b);
            Restart.Click += RestartApp_Click;

            Close.SetBinding(MenuItem.CommandParameterProperty, b);
            Close.Click += CloseApp_Click;

            ContextMenu.Items.Add(Open);
            ContextMenu.Items.Add(Restart);
            ContextMenu.Items.Add(Close);
        }

        private void OpenApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void RestartApp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
            
        }
    }
}
