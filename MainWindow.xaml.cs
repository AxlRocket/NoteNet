using NoteNet.Properties;
using NoteNet.UI.AppThemes;
using NoteNet.UI.Controls;
using NoteNet.UI.Languages;
using NoteNet.Windows;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

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

            Lang.SetLanguage();
            Theme.SetTheme();
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
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
                Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

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
        }

        void LoadNote(string _fileName)
        {
            string str = string.Empty;
            FileStream fStream;
            if (File.Exists(_fileName))
            {
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                str = new StreamReader(fStream).ReadToEnd();
                fStream.Close();
            }

            Console.WriteLine(str);
        }

        private void CreateNote(string path)
        {
            LoadNote(path);

            Note nte = new Note
            {
                Width = Width / 2 - 30,
                Margin = new Thickness(10, 0, 0, 10),
                Title = path.Remove(path.Length - 4).Split('-')[1],
                Date = path.Split('-')[0]
            };

            nte.Click += OpenNote;
            nte.ContextMenu = NoteContextMenu();

            AddNoteToPanel(nte);
        }

        private ContextMenu NoteContextMenu()
        {
            ContextMenu CM = new ContextMenu();
            MenuItem Mod = new MenuItem();
            Mod.Header = "Modify";
            MenuItem Del = new MenuItem();
            Del.Header = "Delete";
            MenuItem Shr = new MenuItem();
            Shr.Header = "Share";
            MenuItem Dup = new MenuItem();
            Dup.Header = "Duplicate";

            Binding b = new Binding("Parent");
            b.RelativeSource = RelativeSource.Self;

            Mod.SetBinding(MenuItem.CommandParameterProperty, b);
            Mod.Click += ModifyNote;

            Del.SetBinding(MenuItem.CommandParameterProperty, b);
            Del.Click += DeleteNote;

            Shr.SetBinding(MenuItem.CommandParameterProperty, b);
            Shr.Click += ShareNote;

            Dup.SetBinding(MenuItem.CommandParameterProperty, b);
            Dup.Click += DuplicateNote;

            CM.Items.Add(Mod);
            CM.Items.Add(Del);
            CM.Items.Add(Shr);
            CM.Items.Add(Dup);

            return CM;
        }

        private void ModifyNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            Console.WriteLine("Modifier : " + (CM.PlacementTarget as Note).Title);
        }

        private void DeleteNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            Console.WriteLine("Supprimer : " + (CM.PlacementTarget as Note).Title);
        }

        private void ShareNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            Console.WriteLine("Partager : " + (CM.PlacementTarget as Note).Title);
        }

        private void DuplicateNote(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ContextMenu CM = mnu.Parent as ContextMenu;

            Console.WriteLine("Dupliquer : " + (CM.PlacementTarget as Note).Title);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNote AN = new AddNote(this.Width - 50, this.Height - 50, this.Left, this.Top); //, "Titre test", "HelloWorld!"
            AN.ShowInTaskbar = false;
            AN.Owner = this;
            AN.ShowDialog();
        }

        private void AddNoteToPanel(Note nte)
        {
            WPanel.Children.Insert(0, nte);

            /*if (FirstCol.ActualHeight > SecCol.ActualHeight)
            {
                SecCol.Children.Insert(0, nte);
                return Task.CompletedTask;
            }
            else if (FirstCol.ActualHeight < SecCol.ActualHeight)
            {
                FirstCol.Children.Insert(0, nte);
                return Task.CompletedTask;
            }
            else
            {
                FirstCol.Children.Insert(0, nte);
                return Task.CompletedTask;
            }*/
        }

        private void OpenNote(object sender, RoutedEventArgs e)
        {
            Note nte = (Note)sender;
            Console.WriteLine(nte.Title);

            AddNote AN = new AddNote(this.Width - 50, this.Height - 50, this.Left, this.Top, nte.Date + "-" + nte.Title); //, "Titre test", "HelloWorld!"
            AN.ShowInTaskbar = false;
            AN.Owner = this;
            AN.ShowDialog();
        }

    }
}
