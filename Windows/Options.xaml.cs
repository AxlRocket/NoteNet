using System;
using System.Windows;

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

            Console.WriteLine("Options");
        }
    }
}
