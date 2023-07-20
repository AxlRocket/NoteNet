using NoteNet.Properties;
using NoteNet.UI.AppThemes;
using NoteNet.UI.Languages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NoteNet.Windows
{
    /// <summary>
    /// Logique d'interaction pour Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options(Window parent, double width = 0, double height = 0, double left = 0, double top = 0)
        {
            InitializeComponent();

            Owner = parent;
            Width = width;
            MinWidth = width;
            MaxWidth = width;
            Height = height;
            MinHeight = height;
            MaxHeight = height;
            Left = left + 25;
            Top = top + 25;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch(Settings.Default.Language)
            {
                case "fr-FR":
                    fr.IsSelected = true;
                    break;
                case "en-GB":
                    en.IsSelected = true;
                    break;
                default:
                    en.IsSelected = true;
                    break;
            }

            switch(Settings.Default.Theme)
            {
                case "Light":
                    light.IsSelected = true;
                    break;
                case "Dark":
                    dark.IsSelected = true;
                    break;
                default:
                    light.IsSelected = true;
                    break;
            }

            if (Settings.Default.Showbubble)
                BubbleCheck.IsChecked = true;

            if (Settings.Default.AtStartup)
                StartCheck.IsChecked = true;

            DefaultFolder.Text = Settings.Default.DefaultFolder;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LanguageSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem LanguageComboBox = (ComboBoxItem)LanguageSelection.SelectedItem;

            Uri LangFileUri;

            switch (LanguageComboBox.Name.ToString())
            {
                case "en":
                    LangFileUri = new Uri("/NoteNet;component/UI/Languages/Lang_en-GB.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Language = "en-GB";
                    break;
                case "fr":
                    LangFileUri = new Uri("/NoteNet;component/UI/Languages/Lang_fr-FR.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Language = "fr-FR";
                    break;
                default:
                    LangFileUri = new Uri("/NoteNet;component/UI/Languages/Lang_en-GB.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Language = "en-GB";
                    break;
            }

            Lang.Replace_Lang(LangFileUri);
        }

        private void ThemeSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem ThemeComboBox = (ComboBoxItem)ThemeSelection.SelectedItem;

            Uri ThemeFileUri;

            switch (ThemeComboBox.Name.ToString())
            {
                case "light":
                    ThemeFileUri = new Uri("/NoteNet;component/UI/AppThemes/LightTheme.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Theme = "Light";
                    break;
                case "dark":
                    ThemeFileUri = new Uri("/NoteNet;component/UI/AppThemes/DarkTheme.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Theme = "Dark";
                    break;
                default:
                    ThemeFileUri = new Uri("/NoteNet;component/UI/AppThemes/LightTheme.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Theme = "Light";
                    break;
            }

            Theme.Replace_Theme(ThemeFileUri);
            //Application.Current.MainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico"));
        }

        private void BubbleCheck_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Showbubble = true;
        }

        private void BubbleCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Showbubble = false;
        }

        private void StartCheck_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AtStartup = true;
        }

        private void StartCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AtStartup = false;
        }
    }
}
