using NoteNet.Properties;
using NoteNet.UI.AppThemes;
using NoteNet.UI.Controls;
using NoteNet.UI.Languages;
using NoteNet.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace NoteNet
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly Bubble bubble;

        public MainWindow()
        {
            InitializeComponent();

            Lang.SetLanguage();
            Theme.SetTheme();

            //new NotifyIcon();

            /*if (Settings.Default.Showbubble)
                bubble = new Bubble();*/

            Debug.WriteLine("Hello MainWindow");
            Debug.WriteLine(this.Name);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                //bubble?.Show();
            }
            else
            {
                //bubble?.Hide();
            }
                
            base.OnStateChanged(e);
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            /*WindowState = WindowState.Minimized;
            Hide();*/

            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            double workAreaHeight = SystemParameters.WorkArea.Height;
            double workAreaWidth = SystemParameters.WorkArea.Width;

            double heightDiff = height - workAreaHeight;
            double widthDiff = width - workAreaWidth;

            double widthRatio = 0.25;
            double heightRatio = 0.6;

            int offset = 20;

            Left = width - (width * widthRatio) - widthDiff - offset;
            Top = height - (height * heightRatio) - heightDiff - offset;

            MaxWidth = width * widthRatio + offset;
            MinWidth = width * widthRatio + offset;
            Width = width * widthRatio + offset;
            MaxHeight = height * heightRatio + offset;
            MinHeight = height * heightRatio + offset;
            Height = height * heightRatio + offset;

            if (Settings.Default.FirstStart)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NoteNet Notes");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Settings.Default.DefaultFolder = path;
                Settings.Default.FirstStart = false;
                Settings.Default.Save();

                Message.Show(this, "ThanksMessage", false, "Thanks");
            }

            if (!IsDirectoryEmpty(Settings.Default.DefaultFolder))
            {
                DirectoryInfo DI = new DirectoryInfo(Settings.Default.DefaultFolder);
                foreach (FileInfo file in DI.GetFiles("*.nte"))
                {
                    CreateNote(file.FullName);
                }
            }

            ReduceImage.Source = (System.Windows.Media.ImageSource)Application.Current.Resources["RightArrow" + Settings.Default.Theme];
            OptionsImage.Source = (System.Windows.Media.ImageSource)Application.Current.Resources["OptionsImage" + Settings.Default.Theme];
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            Options opt = new Options(this, this.Width - 50, this.Height - 50, this.Left, this.Top)
            {
                ShowInTaskbar = false,
                Owner = this
            };

            opt.ShowDialog();
        }

        private void ReduceApp_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private FlowDocument LoadNote(string _fileName)
        {
            if (File.Exists(_fileName))
            {
                FileStream nteFile = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
                FlowDocument FD = XamlReader.Load(nteFile) as FlowDocument;
                nteFile.Close();
                return FD;
            }

            return null;
        }

        private void CreateNote(string path)
        {
            string NoteName = path.Split('\\')[path.Split('\\').Count() - 1];

            Note nte = new Note
            {
                Width = Width / 2 - 30,
                Margin = new Thickness(10, 0, 0, 10),
                Title = NoteName.Remove(NoteName.Length - 4).Split('-')[1],
                RTBContent = LoadNote(path),
                Date = NoteName.Split('-')[0],
                IsTabStop = false
            };

            nte.Click += OpenNote;
            nte.ContextMenu = NoteContextMenu();

            AddNoteToPanel(nte);
        }

        private ContextMenu NoteContextMenu()
        {
            ContextMenu CM = new ContextMenu();
            MenuItem Mod = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Modify"]
            };
            MenuItem Del = new MenuItem
            {
                Header = (string)Application.Current.Resources["ContextMenu.Delete"]
            };

            Binding b = new Binding("Parent")
            {
                RelativeSource = RelativeSource.Self
            };

            Mod.SetBinding(MenuItem.CommandParameterProperty, b);
            Mod.Click += ModifyNote;

            Del.SetBinding(MenuItem.CommandParameterProperty, b);
            Del.Click += DeleteNote;

            CM.Items.Add(Mod);
            CM.Items.Add(Del);

            return CM;
        }

        private void ModifyNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            OpenNote(CM.PlacementTarget as Note, null);
        }

        private void DeleteNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            if (File.Exists(Path.Combine(Settings.Default.DefaultFolder, (CM.PlacementTarget as Note).Date + "-" + (CM.PlacementTarget as Note).Title + ".nte")))
            {
                File.Delete(Path.Combine(Settings.Default.DefaultFolder, (CM.PlacementTarget as Note).Date + "-" + (CM.PlacementTarget as Note).Title + ".nte"));
            }

            NoteContainer.Children.Remove(CM.PlacementTarget as Note);
        }

        public void NewNoteFromBubble(object sender, RoutedEventArgs e)
        {
            NewNote_Click(sender, e);
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            AddNote AddNte = new AddNote(this, this.Width - 50, this.Height - 50, this.Left, this.Top, false)
            {
                ShowInTaskbar = false,
                Owner = this
            };

            if (AddNte.ShowDialog() == true)
            {
                CreateNote(Path.Combine(Settings.Default.DefaultFolder, AddNte.FullName));
            }
        }

        private void NewList_Click(object sender, RoutedEventArgs e)
        {
            AddNote AddNte = new AddNote(this, this.Width - 50, this.Height - 50, this.Left, this.Top, true)
            {
                ShowInTaskbar = false,
                Owner = this
            };

            if (AddNte.ShowDialog() == true)
            {
                CreateNote(Path.Combine(Settings.Default.DefaultFolder, AddNte.FullName));
            }
        }

        private void AddNoteToPanel(Note nte)
        {
            NoteContainer.Children.Insert(0, nte);
        }

        private void OpenNote(object sender, RoutedEventArgs e)
        {
            Note nte = (Note)sender;

            if (File.Exists(Path.Combine(Settings.Default.DefaultFolder, nte.Date + "-" + nte.Title + ".nte")) || true)
            {
                AddNote AddNte = new AddNote(this, this.Width - 50, this.Height - 50, this.Left, this.Top, false, nte.Date + "-" + nte.Title)
                {
                    ShowInTaskbar = false,
                    Owner = this
                };

                if (AddNte.ShowDialog() == true)
                {
                    RefreshNote(nte, AddNte.NewTitle);
                }
            }
            else
            {
                Message.Show(this, "DeletedNote", true);
                
                NoteContainer.Children.Remove(nte);
            }


        }

        private void RefreshNote(Note nte, string newTitle)
        {
            if (File.Exists(Path.Combine(Settings.Default.DefaultFolder, nte.Date + "-" + nte.Title + ".nte")) && nte.Title != newTitle)
            {
                File.Delete(Path.Combine(Settings.Default.DefaultFolder, nte.Date + "-" + nte.Title + ".nte"));
            }

            nte.Title = newTitle;
            nte.RTBContent = LoadNote(Path.Combine(Settings.Default.DefaultFolder, nte.Date + "-" + newTitle + ".nte"));
        }

        private void NoteContainerScrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
            }
        }

        private void Main_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Main_Drop(object sender, DragEventArgs e)
        {
            BorderDragNDrop.Visibility = Visibility.Collapsed;
            BorderNoteContainer.Visibility = Visibility.Visible;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Count() > 1)
            {
                Message.Show(this, "TooMuchFiles", true, "Information");
            }
            else
            {
                if (files[0].Contains(".nte"))
                {
                    AddNote AddNte = new AddNote(this, this.Width - 50, this.Height - 50, this.Left, this.Top, false, files[0])
                    {
                        ShowInTaskbar = false,
                        Owner = this
                    };

                    if (AddNte.ShowDialog() == true)
                    {
                        CreateNote(Path.Combine(Settings.Default.DefaultFolder, AddNte.FullName));
                    }
                }
                else
                {
                    Message.Show(this, "BadFile", true, "Information");
                }
            }
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            BorderNoteContainer.Visibility = Visibility.Collapsed;
            BorderDragNDrop.Visibility = Visibility.Visible;
        }

        private void Main_DragLeave(object sender, DragEventArgs e)
        {
            BorderDragNDrop.Visibility = Visibility.Collapsed;
            BorderNoteContainer.Visibility = Visibility.Visible;
        }
    }
}
