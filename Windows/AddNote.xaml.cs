using NoteNet.Properties;
using System;
using System.IO;
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
        public AddNote(Window parent, double width = 0, double height = 0, double left = 0, double top = 0, string path = "")
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

        public string FullName
        {
            get { return Created.Text + "-" + Title.Text + ".nte"; }
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
                {
                    this.DialogResult = false;
                    this.Close();
                }
            } 
            else
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && Title.Text != "" && Title.Text.Trim() != "")
            {
                string path = System.IO.Path.Combine(Settings.Default.DefaultFolder, Created.Text + "-" + Title.Text + ".nte");

                if (File.Exists(path))
                {
                    if (Message.Show(this, "AlreadyExists", false))
                    {
                        SaveNote(path);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                else
                {
                    SaveNote(path);
                    this.DialogResult = true;
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
