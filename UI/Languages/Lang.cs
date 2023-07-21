using NoteNet.Properties;
using System;
using System.Linq;
using System.Windows;

namespace NoteNet.UI.Languages
{
    internal static class Lang
    {
        internal static void SetLanguage()
        {
            ResourceDictionary LangFile = new ResourceDictionary
            {
                Source = new Uri("/NoteNet;component/UI/Languages/Lang_" + Settings.Default.Language + ".xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(LangFile);
            Application.Current.Resources.Remove(Application.Current.Resources.MergedDictionaries.OfType<ResourceDictionary>().Select(m => m).Where(j => j.Source.ToString().Contains("Lang")));
        }

        internal static void Replace_Lang(Uri LangFileUri)
        {
            ResourceDictionary LangFile = new ResourceDictionary
            {
                Source = LangFileUri
            };

            Application.Current.Resources.MergedDictionaries.Add(LangFile);
            Application.Current.Resources.Remove(Application.Current.Resources.MergedDictionaries.OfType<ResourceDictionary>().Select(m => m).Where(j => j.Source.ToString().Contains("Lang")));
        }
    }
}
