using NoteNet.Properties;
using System;
using System.IO;
using System.Text.RegularExpressions;
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
        public AddNote(double width = 0, double height = 0, double left = 0, double top = 0, string path = "")
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

            Console.WriteLine(Path.Combine(Settings.Default.DefaultFolder, path + ".nte"));

            if (path != "")
            {
                Created.Text = path.Split('-')[0];
                Title.Text = path.Split('-')[1];
                Title.FontStyle = FontStyles.Normal;
                Title.Opacity = 1;
                LoadNote(Path.Combine(Settings.Default.DefaultFolder, path + ".nte"));
            }
            else
            {
                Created.Text = DateTime.Now.ToString("dd MMM yyyy");
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
            if (Title.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && Title.Text != "" && Title.Text.Trim() != "")
            {
                string path = Path.Combine(Settings.Default.DefaultFolder, Created.Text + "-" + Title.Text + ".nte");

                if (File.Exists(path))
                {
                    if (Message.Show(this, "AlreadyExists", false))
                    {
                        SaveNote(path);
                        this.Close();
                    }
                }
                else
                {
                    SaveNote(path);
                    this.Close();
                }                
            }
            else
            {
                Message.Show(this, "EmptyTitle", true);
            }
        }

        void SaveNote(string _fileName)
        {
            TextRange TR;
            FileStream fStream;
            TR = new TextRange(Content.Document.ContentStart, Content.Document.ContentEnd);
            fStream = new FileStream(_fileName, FileMode.Create);
            TR.Save(fStream, DataFormats.XamlPackage);
            fStream.Close();
        }

        void LoadNote(string _fileName)
        {
            TextRange TR;
            FileStream fStream;
            if (File.Exists(_fileName))
            {
                TR = new TextRange(Content.Document.ContentStart, Content.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                TR.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
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
            if (Title.Text == "" || Title.Text.Trim() == "")
            {
                Title.Text = (string)Application.Current.Resources["AddNoteWindow.Title"];
                Title.FontStyle = FontStyles.Italic;
                Title.Opacity = .35;
            }
        }

        private void Title_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\"" ||
                e.Text == "\\" ||
                e.Text == "/" ||
                e.Text == "?" ||
                e.Text == ":" ||
                e.Text == "*" ||
                e.Text == "<" ||
                e.Text == ">" ||
                e.Text == "|" ||
                e.Text == "-" ||
                e.Text == "_")
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}
