using System.Windows;
using System.Windows.Input;

namespace NoteNet.Windows
{
    /// <summary>
    /// Logique d'interaction pour Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        public Message()
        {
            InitializeComponent();
        }

        public static bool Show(Window parent, string message, bool caution = false, string title = "Warning")
        {
            Message msgBox = new Message();
            msgBox.Owner = parent;

            msgBox.Title.Text = (string)Application.Current.Resources["Message." + title];

            msgBox.MessageText.Text = (string)Application.Current.Resources["Message." + message];

            if (caution)
            {
                msgBox.Cancel.Visibility = Visibility.Collapsed;
                msgBox.OK.HorizontalAlignment = HorizontalAlignment.Center;
                msgBox.OK.Margin = new Thickness(0,15,0,10);
                msgBox.ButtonsContainer.ColumnDefinitions.RemoveAt(1);
            }

            return (bool)msgBox.ShowDialog();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OK_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                Cancel_Click(null, null); 
            }
        }
    }
}
