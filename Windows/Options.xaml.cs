﻿using Microsoft.Win32;
using NoteNet.Properties;
using NoteNet.UI.AppThemes;
using NoteNet.UI.Languages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace NoteNet.Windows
{
    public partial class Options : Window
    {
        public Options(Window parent)
        {
            InitializeComponent();

            Opacity = 0;

            Owner = parent;
            Width = MinWidth = MaxWidth = parent.Width - 50;
            Height = MinHeight = MaxHeight = parent.Height - 50;
            Left = parent.Left + 25;
            Top = parent.Top + 25;

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            BeginAnimation(OpacityProperty, opacityAnimation);
        }

        private bool bubblePreviousState;
        private string themePreviousState;

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
                    Light.IsSelected = true;
                    themePreviousState = "Light";
                    break;
                case "Dark":
                    Dark.IsSelected = true;
                    themePreviousState = "Dark";
                    break;
                default:
                    Light.IsSelected = true;
                    themePreviousState = "Light";
                    break;
            }

            if (Settings.Default.Showbubble)
            {
                BubbleCheck.IsChecked = true;
                bubblePreviousState = true;
            }
                
            if (Settings.Default.AtStartup)
                StartCheck.IsChecked = true;

            DefaultFolder.Text = Settings.Default.DefaultFolder;
        }

        private void CloseOptions_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.Showbubble != bubblePreviousState || Settings.Default.Theme != themePreviousState)
            {
                Settings.Default.Save();
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else
            {
                Settings.Default.Save();
                CloseAnimation();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseAnimation();
        }

        private void CloseAnimation()
        {
            DoubleAnimation DA = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            DA.Completed += (s, e) =>
            {
                Close();
            };

            BeginAnimation(OpacityProperty, DA);
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
                case "Light":
                    ThemeFileUri = new Uri("/NoteNet;component/UI/AppThemes/LightTheme.xaml",
                        UriKind.RelativeOrAbsolute);
                    Settings.Default.Theme = "Light";
                    break;
                case "Dark":
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

            var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);

            key.SetValue("NoteNetStartup", AppDomain.CurrentDomain.BaseDirectory + "NoteNet.exe");
        }

        private void StartCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AtStartup = false;

            var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);

            key.DeleteValue("NoteNetStartup", false);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
