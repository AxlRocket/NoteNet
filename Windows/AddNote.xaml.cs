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
        private string CreationDate;

        private bool Modification;

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
                Modification = true;
                CreationDate = path.Split('-')[0]; //
                Created.Text = DateFormat(path.Split('-')[0]); //path.Split('-')[0].Remove(path.Split('-')[0].Count() - 4); // Donne juste la date
                Title.Text = path.Split('-')[1]; // Donne le titre
                Title.FontStyle = FontStyles.Normal;
                Title.Opacity = 1;
                LoadNote(Path.Combine(Settings.Default.DefaultFolder, path + ".nte"));
                TextRange tr = new TextRange(Content.Document.ContentStart, Content.Document.ContentEnd);
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, Application.Current.Resources["ForegroundColor"]);
            }
            else
            {
                Modification = false;
                CreationDate = DateTime.Now.ToString("ddMMyyyyhhmm");
                Created.Text = DateTime.Now.ToString("dd MMM yyyy");
            }
        }

        public string FullName
        {
            get { return CreationDate + "-" + Title.Text + ".nte"; }
        }

        public string NewTitle
        {
            get { return Title.Text; }
        }

        private string DateFormat(string date)
        {
            string day = date[0].ToString() + date[1].ToString(), 
                month = date[2].ToString() + date[3].ToString(), 
                year = date[4].ToString() + date[5].ToString() + date[6].ToString() + date[7].ToString();

            switch (month)
            {
                case "01":
                    month = (string)Application.Current.Resources["Month.January"];
                    break;
                case "02":
                    month = (string)Application.Current.Resources["Month.February"];
                    break;
                case "03":
                    month = (string)Application.Current.Resources["Month.March"];
                    break;
                case "04":
                    month = (string)Application.Current.Resources["Month.April"];
                    break;
                case "05":
                    month = (string)Application.Current.Resources["Month.May"];
                    break;
                case "06":
                    month = (string)Application.Current.Resources["Month.June"];
                    break;
                case "07":
                    month = (string)Application.Current.Resources["Month.July"];
                    break;
                case "08":
                    month = (string)Application.Current.Resources["Month.August"];
                    break;
                case "09":
                    month = (string)Application.Current.Resources["Month.September"];
                    break;
                case "10":
                    month = (string)Application.Current.Resources["Month.October"];
                    break;
                case "11":
                    month = (string)Application.Current.Resources["Month.November"];
                    break;
                case "12":
                    month = (string)Application.Current.Resources["Month.December"];
                    break;
                default:
                    month = (string)Application.Current.Resources["Month.January"]; ;
                    break;
            }

            return day + " " + month + " " + year;
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
                    DialogResult = false;
                    Close();
                }
            } 
            else
            {
                DialogResult = false;
                Close();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && Title.Text != "" && Title.Text.Trim() != "")
            {
                string path = Path.Combine(Settings.Default.DefaultFolder, CreationDate + "-" + Title.Text + ".nte");

                if (File.Exists(path))
                {
                    if (Modification)
                    {
                        if (Message.Show(this, "SaveModification", false, "Information"))
                        {
                            SaveNote(path);
                            DialogResult = true;
                            Close();
                        }
                    }
                    else
                    {
                        if (Message.Show(this, "AlreadyExists", false))
                        {
                            SaveNote(path);
                            DialogResult = true;
                            Close();
                        }
                    }
                }
                else
                {
                    SaveNote(path);
                    DialogResult = true;
                    Close();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Add_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                Cancel_Click(null, null);
            }
        }
    }
}
