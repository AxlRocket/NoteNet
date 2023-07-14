using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace NoteNet.Windows
{
    /// <summary>
    /// Logique d'interaction pour AddNote.xaml
    /// </summary>
    public partial class AddNote : Window
    {
        public AddNote(double width = 0, double height = 0, double left = 0, double top = 0, string title = "", string content = "")
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

            if (title != "")
            {
                Title.Text = "";
                Title.FontStyle = FontStyles.Normal;
                Title.Opacity = 1;
                this.Title.Text = title;
            }
            
            if (content != "")
            {
                this.Content.AppendText(content);
            }
        }

        private bool ContentInRTB()
        {
            var start = Content.Document.ContentStart;
            var end = Content.Document.ContentEnd;

            if (start.GetOffsetToPosition(end) == 0 || start.GetOffsetToPosition(end) == 2)
            {
                return false;
            } else
            {
                return true;
            }
    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Created.Text = (string)Application.Current.Resources["AddNoteWindow.Created"] + DateTime.Now.ToString("dd MMM yyyy");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if ((Title.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && Title.Text != "")
                || ContentInRTB())
            {
                //Ask user if he wants to leave
                if (Message.Show(this, "Leave"))
                    this.Close();
            } 
            else
            {
                this.Close();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && Title.Text != "")
            {
                //Save
            }
            else
            {
                Message.Show(this, "EmptyTitle", true);
            }
        }

        private void Title_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Title.Text == (string)Application.Current.Resources["AddNoteWindow.Title"])
            {
                Title.Text = "";
                Title.FontStyle = FontStyles.Normal;
                Title.Opacity = 1;
            }
        }

        private void Title_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Title.Text == "")
            {
                Title.Text = (string)Application.Current.Resources["AddNoteWindow.Title"];
                Title.FontStyle = FontStyles.Italic;
                Title.Opacity = .25;
            }
        }
    }
}
