using System.Windows;
using System.Windows.Controls;

namespace NoteNet.UI.Controls
{
    public class Note : Button
    {
        // TITLE

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
        "Title", typeof(string), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // CONTENT

        public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
        "Content", typeof(string), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        static Note()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Note), new FrameworkPropertyMetadata(typeof(Note)));
        }
    }
}
