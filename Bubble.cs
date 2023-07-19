using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoteNet
{
    internal class Bubble : Window
    {
        private readonly MainWindow MainWindow;

        public Bubble(MainWindow MV)
        {
            MainWindow = MV;

            MinWidth = 100;
            Width = 100;
            MaxWidth = 100;
            MinHeight = 50;
            Height = 50;
            MaxHeight = 50;

            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            double workAreaHeight = SystemParameters.WorkArea.Height;
            double workAreaWidth = SystemParameters.WorkArea.Width;

            double heightDiff = height - workAreaHeight;
            double widthDiff = width - workAreaWidth;

            Left = width  - widthDiff - Width + 50;
            Top = height - heightDiff - 100;

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
                Width = 38,
                Height = 40,
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Icon.png"))
                },
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["Button"]
            };
            BtnShowApp.Click += BtnShowApp_Click;

            Grid.SetColumn(BtnShowApp, 0);

            Button BtnAddNote = new Button
            {
                Width = 38,
                Height = 40,
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/NoteNet;component/UI/Icons/Add.png"))
                },
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["Button"]
            };
            BtnAddNote.Click += BtnAddNote_Click;

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
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void BtnAddNote_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            MainWindow.NewNoteFromBubble(sender, e);
        }

        private void Bubble_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Left = Left - 50;
        }

        private void Bubble_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Left = Left + 50;
        }

        
    }
}
