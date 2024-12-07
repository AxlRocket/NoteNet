using NoteNet.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NoteNet.UI.AppThemes
{
    internal static class Theme
    {
        internal static void SetTheme()
        {
            ResourceDictionary ThemeFile = new ResourceDictionary
            {
                Source = new Uri("/NoteNet;component/UI/AppThemes/" + Settings.Default.Theme + "Theme.xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(ThemeFile);
            Application.Current.Resources.Remove(Application.Current.Resources.MergedDictionaries.OfType<ResourceDictionary>().Select(m => m).Where(j => j.Source.ToString().Contains("Theme")));

            //Application.Current.MainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico"));
        }

        internal static void Replace_Theme(Uri ThemeFileUri)
        {
            ResourceDictionary ThemeFile = new ResourceDictionary
            {
                Source = ThemeFileUri
            };

            Application.Current.Resources.MergedDictionaries.Add(ThemeFile);
            Application.Current.Resources.Remove(Application.Current.Resources.MergedDictionaries.OfType<ResourceDictionary>().Select(m => m).Where(j => j.Source.ToString().Contains("Theme")));

            Application.Current.MainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico"));
        }
    }
}
