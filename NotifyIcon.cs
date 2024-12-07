using NoteNet.Properties;
using NoteNet.UI.AppThemes;
using NoteNet.UI.Controls;
using NoteNet.UI.Languages;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace NoteNet
{
    internal class NotifyIcon
    {
        public NotifyIcon()
        {
            Lang.SetLanguage();
            Theme.SetTheme();

            ContextMenu CM = new ContextMenu();
            MenuItem Open = new MenuItem
            {
                Text = (string)System.Windows.Application.Current.Resources["ContextMenu.Display"]
            },
            Close = new MenuItem
            {
                Text = (string)System.Windows.Application.Current.Resources["ContextMenu.Close"]
            };

            Open.Click += OpenApp_Click;
            Close.Click += CloseApp_Click;

            CM.MenuItems.Add(Open);
            CM.MenuItems.Add(Close);

            System.Windows.Forms.NotifyIcon NI = new System.Windows.Forms.NotifyIcon
            {
                Text = "NoteNet",
                Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico")).Stream),
                ContextMenu = CM
            };

            NI.MouseDoubleClick += NI_MouseDoubleClick;
            NI.Visible = true;
        }

        private void OpenApp_Click(object sender, EventArgs e)
        {
            if (!CheckWindow.IsWindowOpen<Window>("Main"))
            {
                MainWindow MW = new MainWindow();
                MW.ShowDialog();
            }
        }

        private void CloseApp_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void NI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!CheckWindow.IsWindowOpen<Window>("Main"))
            {
                MainWindow MW = new MainWindow();
                MW.ShowDialog();
            }
        }
    }
}
