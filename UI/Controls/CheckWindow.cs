using System.Linq;
using System.Windows;

namespace NoteNet.UI.Controls
{
    public static class CheckWindow
    {
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            if (Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name)))
                return true;
            else
                return false;
        }
    }
}
