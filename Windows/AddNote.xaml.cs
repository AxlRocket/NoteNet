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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoteNet.Windows
{
    /// <summary>
    /// Logique d'interaction pour AddNote.xaml
    /// </summary>
    public partial class AddNote : Window
    {
        public AddNote(double width = 0, double height = 0, double left = 0, double top = 0)
        {
            this.Width = width;
            this.MinWidth = width;
            this.MaxWidth = width;
            this.Height = height;
            this.MinHeight = height;
            this.MaxHeight = height;
            this.Left = left + 25;
            this.Top = top + 25;
            InitializeComponent();
            

            Console.WriteLine("Height : " + this.Height);
            Console.WriteLine("Width : " + this.Width);
            Console.WriteLine("Top : " + this.Top);
            Console.WriteLine("Left : " + this.Left);
        }
    }
}
