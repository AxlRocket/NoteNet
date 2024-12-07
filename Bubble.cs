using NoteNet.Properties;
using NoteNet.UI.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NoteNet
{
    internal class Bubble : Window
    {
        private int baseLeft = 0; //Initial position
        private int bubbleOpenedLeft = 0; //Open position
        private int bubbleHideLeft = 0; //Hidden position
        private bool playInitialAnimation = true;

        public Bubble()
        {
            Width = MinWidth = MaxWidth = 100;
            Height = MinHeight = MaxHeight = 50;

            Icon = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon" + Settings.Default.Theme + ".ico"));
            Title = (string)Application.Current.Resources["Bubble.Title"];

            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            double workAreaHeight = SystemParameters.WorkArea.Height;
            double workAreaWidth = SystemParameters.WorkArea.Width;

            double heightDiff = height - workAreaHeight;
            double widthDiff = width - workAreaWidth;

            Left = width  - widthDiff - Width + 50;
            Top = height - heightDiff - 100;

            baseLeft = (int)Left;
            bubbleOpenedLeft = (int)Left - 50;
            bubbleHideLeft = (int)Left + 50;

            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;

            Background = new SolidColorBrush(Colors.Transparent);

            Border brd = new Border
            {
                Background = (Brush)Application.Current.Resources["Background"],
                CornerRadius = new CornerRadius(10, 0, 0, 10)
            };

            Grid grid = new Grid();

            ColumnDefinition CD1 = new ColumnDefinition(), CD2 = new ColumnDefinition();

            grid.ColumnDefinitions.Add(CD1);
            grid.ColumnDefinitions.Add(CD2);

            Button BtnShowApp = new Button
            {
                Width = 42,
                Height = 42,
                Content = new Image
                {
                    Source = (ImageSource)Application.Current.Resources["Icon" + Settings.Default.Theme]
                },
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["Button"],
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            BtnShowApp.Click += BtnShowApp_Click;
            BtnShowApp.ToolTip = Application.Current.Resources["Bubble.ShowApp"];

            Grid.SetColumn(BtnShowApp, 0);

            Button BtnAddNote = new Button
            {
                Width = 42,
                Height = 42,
                Content = new Image
                {
                    Source = (ImageSource)Application.Current.Resources["Add" + Settings.Default.Theme]
                },
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["Button"],
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            BtnAddNote.Click += BtnAddNote_Click;
            BtnAddNote.ToolTip = Application.Current.Resources["Bubble.AddNote"];

            Grid.SetColumn(BtnAddNote, 1);

            grid.Children.Add(BtnShowApp);
            grid.Children.Add(BtnAddNote);

            brd.Child = grid;

            this.AddChild(brd);

            MouseEnter += Bubble_MouseEnter;
            MouseLeave += Bubble_MouseLeave;
        }

        private void BtnShowApp_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckWindow.IsWindowOpen<Window>("Main"))
            {
                playInitialAnimation = false;
                MainWindow MW = new MainWindow();
                HideAnimation();

                if ((bool)MW.ShowDialog())
                {
                    playInitialAnimation = true;
                    InitialAnimation();
                }
            }
        }

        private void BtnAddNote_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckWindow.IsWindowOpen<Window>("Main"))
            {
                playInitialAnimation = false;
                MainWindow MW = new MainWindow(true);
                HideAnimation();

                bool MVDialogResult = (bool)MW.ShowDialog();

                if (MVDialogResult)
                {
                    playInitialAnimation = true;
                    InitialAnimation();
                }
            }
        }

        private void Bubble_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenAnimation();
            //Left -= 50;
        }

        private void Bubble_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (playInitialAnimation)
                InitialAnimation();
            //Left += 50;
        }

        private void OpenAnimation()
        {
            DoubleAnimation MouseEnterAnimation = new DoubleAnimation(bubbleOpenedLeft, new Duration(TimeSpan.FromSeconds(0.25)))
            {
                AccelerationRatio = 0.75
            };
            this.BeginAnimation(LeftProperty, MouseEnterAnimation);
        }

        private void InitialAnimation()
        {
            DoubleAnimation MouseLeaveAnimation = new DoubleAnimation(baseLeft, new Duration(TimeSpan.FromSeconds(0.2)));
            this.BeginAnimation(LeftProperty, MouseLeaveAnimation);
        }

        private void HideAnimation()
        {
            DoubleAnimation MouseEnterAnimation = new DoubleAnimation(bubbleHideLeft, new Duration(TimeSpan.FromSeconds(0.2)))
            {
                AccelerationRatio = 1
            };
            this.BeginAnimation(LeftProperty, MouseEnterAnimation);
        }
    }
}
