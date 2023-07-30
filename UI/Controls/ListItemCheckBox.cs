using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoteNet.UI.Controls
{
    public class ListItemCheckBox : StackPanel
    {
        public ListItemCheckBox() 
        {
            

        }

        private void TB_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            Keyboard.Focus(TB);
        }
    }
}
