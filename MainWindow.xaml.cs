using NoteNet.UI.AppThemes;
using NoteNet.UI.Controls;
using NoteNet.UI.Languages;
using NoteNet.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoteNet
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Lang.SetLanguage();
            Theme.SetTheme();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            double workAreaHeight = SystemParameters.WorkArea.Height;
            double workAreaWidth = SystemParameters.WorkArea.Width;

            double heightDiff = height - workAreaHeight;
            double widthDiff = width - workAreaWidth;

            double widthRatio = 0.25;
            double heightRatio = 0.6;

            int offset = 10;

            this.Left = width - (width * widthRatio) - widthDiff - offset;
            this.Top = height - (height * heightRatio) - heightDiff - offset;

            Console.WriteLine(height + " " + width);
            Console.WriteLine(workAreaHeight + " " + workAreaWidth);
            this.MaxWidth = width * widthRatio;
            this.MinWidth = width * widthRatio;
            this.Width = width * widthRatio;
            this.MaxHeight = height * heightRatio;
            this.MinHeight = height * heightRatio;
            this.Height = height * heightRatio;

            // TEST

            Note nte = new Note{
                Title = "Courses",
                Content = "Lait, oeufs, etc"
            };

            nte.MouseDown += DeleteNote;
            nte.Click += ModifyNote;

            FirstCol.Children.Add(nte);
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNote AN = new AddNote(this.Width - 50, this.Height - 50, this.Left, this.Top); //, "Titre test", "HelloWorld!"
            AN.ShowInTaskbar = false;
            AN.Owner = this;
            AN.ShowDialog();
        }

        private void DeleteNote(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                Console.WriteLine("Delete");
        }

        private void ModifyNote(object sender, RoutedEventArgs e)
        {
            Note nte = (Note)sender;

            AddNote AN = new AddNote(this.Width - 50, this.Height - 50, this.Left, this.Top, nte.Title, nte.Content); //, "Titre test", "HelloWorld!"
            AN.ShowInTaskbar = false;
            AN.Owner = this;
            AN.ShowDialog();
        }
    }
}
