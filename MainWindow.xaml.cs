using NoteNet.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*Console.WriteLine("Height : " + this.Height);
            Console.WriteLine("Width : " + this.Width);
            Console.WriteLine("Top : " + this.Top);
            Console.WriteLine("Left : " + this.Left);*/
        }

        private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine("Height : " + this.Height);
            Console.WriteLine("Width : " + this.Width);
            Console.WriteLine("Top : " + this.Top);
            Console.WriteLine("Left : " + this.Left);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNote AN = new AddNote(this.Width - 50, this.Height - 50, this.Left, this.Top);
            AN.ShowDialog();
        }
    }
}
