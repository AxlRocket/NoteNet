using System;
using System.Linq;
using System.Windows;

namespace NoteNet.UI.AppThemes
{
    internal static class Theme
    {
        internal static void SetTheme()
        {
            ResourceDictionary ThemeFile = new ResourceDictionary
            {
                Source = new Uri("/NoteNet;component/UI/AppThemes/" + Properties.Settings.Default.Theme + "Theme.xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(ThemeFile);
            Application.Current.Resources.Remove(Application.Current.Resources.MergedDictionaries.OfType<ResourceDictionary>().Select(m => m).Where(j => j.Source.ToString().Contains("Theme")));
        }
    }
}
