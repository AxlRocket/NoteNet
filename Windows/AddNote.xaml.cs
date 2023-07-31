using NoteNet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace NoteNet.Windows
{
    /// <summary>
    /// Logique d'interaction pour AddNote.xaml
    /// </summary>
    public partial class AddNote : Window
    {
        private string CreationDate;

        private bool Modification;

        private bool isList;

        public string FullName
        {
            get { return CreationDate + "-" + AddNoteTitle.Text + ".nte"; }
        }

        public string NewTitle
        {
            get { return AddNoteTitle.Text; }
        }

        private string DateFormat(string date)
        {
            string day = date[6].ToString() + date[7].ToString(),
                month = date[4].ToString() + date[5].ToString(),
                year = date[0].ToString() + date[1].ToString() + date[2].ToString() + date[3].ToString();

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

        public AddNote(Window parent, double width = 0, double height = 0, double left = 0, double top = 0, bool list = false, string path = "")
        {
            InitializeComponent();

            isList = list;

            Owner = parent;
            Width = width;
            MinWidth = width;
            MaxWidth = width;
            Height = height;
            MinHeight = height;
            MaxHeight = height;
            Left = left + 25;
            Top = top + 25;

            if (!isList)
            {
                AddNoteContent.Document = new FlowDocument();
                AddNoteContent.AcceptsReturn = true;
            }
            else
            {
                AddNoteContent.AcceptsReturn = false;
            }

            if (path != "")
            {
                Modification = true;
                CreationDate = path.Split('-')[0];
                Created.Text = DateFormat(path.Split('-')[0]); //Day note
                AddNoteTitle.Text = path.Split('-')[1]; //Title note
                AddNoteTitle.FontStyle = FontStyles.Normal;
                AddNoteTitle.Opacity = 1;
                LoadNote(Path.Combine(Settings.Default.DefaultFolder, path + ".nte"));
                TextRange tr = new TextRange(AddNoteContent.Document.ContentStart, AddNoteContent.Document.ContentEnd);
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, Application.Current.Resources["ForegroundColor"]);
                if (isList)
                    AddNoteContent.AcceptsReturn = false;
            }
            else
            {
                Modification = false;
                CreationDate = DateTime.Now.ToString("yyyyMMddHHmm");
                Created.Text = DateFormat(DateTime.Now.ToString("yyyyMMdd"));
            }

            if (isList)
            {
                AddNoteContent.CaretBrush = Brushes.Transparent;

                if (((StackPanel)AddNoteContent.Document.FindName("SPContainer")).Children.Count == 0)
                {
                    AddListElement();
                }

                foreach (CheckBox CB in FindWindowChildren<CheckBox>((StackPanel)AddNoteContent.Document.FindName("SPContainer")))
                {
                    CB.Checked += new RoutedEventHandler((send, ee) => CB_Checked(send, ee, (DockPanel)CB.Parent));
                    CB.Unchecked += new RoutedEventHandler((send, ee) => CB_Checked(send, ee, (DockPanel)CB.Parent));
                }
            }
        }

        public static IEnumerable<T> FindWindowChildren<T>(DependencyObject dObj) where T : DependencyObject
        {
            if (dObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
                {
                    DependencyObject ch = VisualTreeHelper.GetChild(dObj, i);
                    if (ch != null && ch is T)
                    {
                        yield return (T)ch;
                    }
                    foreach (T nestedChild in FindWindowChildren<T>(ch))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }

        private bool ContentInRTB()
        {
            var start = AddNoteContent.Document.ContentStart;
            var end = AddNoteContent.Document.ContentEnd;

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
            if ((AddNoteTitle.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && AddNoteTitle.Text.Trim() != "")
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
            if (AddNoteTitle.Text != (string)Application.Current.Resources["AddNoteWindow.Title"] && AddNoteTitle.Text.Trim() != "")
            {
                string path = Path.Combine(Settings.Default.DefaultFolder, CreationDate + "-" + AddNoteTitle.Text + ".nte");

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
            FileStream xamlFile = new FileStream(_fileName, FileMode.Create, FileAccess.ReadWrite);
            XamlWriter.Save(AddNoteContent.Document, xamlFile);
            xamlFile.Close();
        }

        void LoadNote(string _fileName)
        {
            FileStream nteFile = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            FlowDocument FD = XamlReader.Load(nteFile) as FlowDocument;
            AddNoteContent.Document = FD;

            nteFile.Close();

            if ((StackPanel)AddNoteContent.Document.FindName("SPContainer") != null)
                isList = true;
        }

        private void AddNoteTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AddNoteTitle.Text == (string)Application.Current.Resources["AddNoteWindow.Title"])
            {
                AddNoteTitle.Text = "";
                AddNoteTitle.FontStyle = FontStyles.Normal;
                AddNoteTitle.Opacity = 1;
            }
        }

        private void AddNoteTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AddNoteTitle.Text == "" || AddNoteTitle.Text.Trim() == "")
            {
                AddNoteTitle.Text = (string)Application.Current.Resources["AddNoteWindow.Title"];
                AddNoteTitle.FontStyle = FontStyles.Italic;
                AddNoteTitle.Opacity = .35;
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

        private void AddNoteContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && isList)
            { 
                AddListElement();
            }
        }

        private void AddListElement()
        {
            DockPanel SP = new DockPanel
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            CheckBox CB = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(2)
            };
            TextBox TB = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                Focusable = true,
                AcceptsReturn = false,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                BorderBrush = Brushes.Transparent,
                SelectionBrush = (Brush)Application.Current.Resources["SelectionText"],
                TextWrapping = TextWrapping.WrapWithOverflow
            };
            TB.Loaded += TB_Loaded;
            CB.Checked += new RoutedEventHandler((send, ee) => CB_Checked(send, ee, (DockPanel)CB.Parent));
            CB.Unchecked += new RoutedEventHandler((send, ee) => CB_Checked(send, ee, (DockPanel)CB.Parent));

            SP.Children.Add(CB);
            SP.Children.Add(TB);

            FlowDocument FD = AddNoteContent.Document;
            StackPanel SPT = (StackPanel)FD.FindName("SPContainer");

            SPT.Children.Add(SP);
        }

        private void CB_Checked(object sender, RoutedEventArgs e, DockPanel DP)
        {
            CheckBox CB = sender as CheckBox;

            if (CB.IsChecked == true)
            {
                TextBox TB = (TextBox)DP.Children[1];
                TB.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                TextBox TB = (TextBox)DP.Children[1];
                TB.TextDecorations = null;
            }
        }

        private void TB_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            Keyboard.Focus(TB);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && isList)
            {
                if (Keyboard.FocusedElement.GetType() == typeof(TextBox))
                {
                    TextBox TB = (TextBox)Keyboard.FocusedElement;

                    if (TB.Text.Length > 0)
                    {
                        e.Handled = false;
                    }
                    else if (TB.Text.Length < 1)
                    {
                        FlowDocument FD = AddNoteContent.Document;
                        StackPanel SPT = (StackPanel)FD.FindName("SPContainer");

                        if (SPT.Children.Count == 1)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = true;

                            DockPanel SP = (DockPanel)TB.Parent;

                            int index = SPT.Children.IndexOf(SP);

                            SPT.Children.Remove(SP);

                            Keyboard.Focus(((DockPanel)SPT.Children[index - 1]).Children[1]);
                        }
                    }
                }
                else if (Keyboard.FocusedElement.GetType() == typeof(RichTextBox))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else if (Keyboard.FocusedElement.GetType() == typeof(RichTextBox) && isList)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void AddNoteContent_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isList)
            {
                FlowDocument FD = AddNoteContent.Document;
                StackPanel SPT = (StackPanel)FD.FindName("SPContainer");

                Keyboard.Focus(((DockPanel)SPT.Children[SPT.Children.Count - 1]).Children[1]);
            }
        }
    }
}
