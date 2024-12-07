using NoteNet.Properties;
using NoteNet.UI.Controls;
using NoteNet.Windows;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace NoteNet
{
    public partial class MainWindow : Window
    {
        public MainWindow(bool addNoteFromBubble = false)
        {
            InitializeComponent();

            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            double workAreaHeight = SystemParameters.WorkArea.Height;
            double workAreaWidth = SystemParameters.WorkArea.Width;

            double heightDiff = height - workAreaHeight;
            double widthDiff = width - workAreaWidth;

            double widthRatio = 0.25;
            double heightRatio = 0.6;

            int offset = 20;

            MaxWidth = width * widthRatio + offset;
            MinWidth = width * widthRatio + offset;
            Width = width * widthRatio + offset;
            MaxHeight = height * heightRatio + offset;
            MinHeight = height * heightRatio + offset;
            Height = height * heightRatio + offset;

            //Left = width - (width * widthRatio) - widthDiff - offset; 
            Left = width + Width + offset; //Initial position - Closed
            openLeftMainWindow = width - (width * widthRatio) - widthDiff - offset; //Open position
            closeLeftMainWindow = width + Width + offset; //Close position
            Top = height - (height * heightRatio) - heightDiff - offset;

            LocationChanged += new EventHandler(Window_LocationChanged);

            ReduceImage.Source = (System.Windows.Media.ImageSource)Application.Current.Resources["RightArrow" + Settings.Default.Theme];
            OptionsImage.Source = (System.Windows.Media.ImageSource)Application.Current.Resources["OptionsImage" + Settings.Default.Theme];

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

                //Message.Show(this, "ThanksMessage", false, "Thanks");
            }

            if (addNoteFromBubble)
            {
                this.Loaded += (s, e) =>
                {
                    NewNote_Click(s, e);
                };
            }

            OpenAnimation();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            foreach (Window win in this.OwnedWindows)
            {
                win.Top = this.Top + 25;
                win.Left = this.Left + 25;
            }
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private double closeLeftMainWindow = 0;
        private double openLeftMainWindow = 0;
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsDirectoryEmpty(Settings.Default.DefaultFolder))
            {
                DirectoryInfo DI = new DirectoryInfo(Settings.Default.DefaultFolder);
                foreach (FileInfo file in DI.GetFiles("*.nte"))
                {
                    CreateNote(file.FullName);
                }
            }
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            Options opt = new Options(this)
            {
                ShowInTaskbar = false,
                Owner = this
            };

            opt.ShowDialog();
        }

        private void ReduceApp_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation CloseDA = new DoubleAnimation(closeLeftMainWindow, new Duration(TimeSpan.FromSeconds(0.3)))
            {
                AccelerationRatio = 0.8
            };

            CloseDA.Completed += (s, ev) =>
            {
                this.DialogResult = true;
                this.Close();
            };

            this.BeginAnimation(LeftProperty, CloseDA);
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

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            AddNote AddNte = new AddNote(this, false)
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
            AddNote AddNte = new AddNote(this, true)
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
                AddNote AddNte = new AddNote(this, false, nte.Date + "-" + nte.Title)
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
                    AddNote AddNte = new AddNote(this, false, files[0])
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

        private void OpenAnimation()
        {
            Opacity = 1;
            DoubleAnimation OpenDA = new DoubleAnimation(openLeftMainWindow, new Duration(TimeSpan.FromSeconds(0.25)))
            {
                AccelerationRatio = 0.75
            };
            this.BeginAnimation(LeftProperty, OpenDA);
        }
    }
}
