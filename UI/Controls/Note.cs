using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoteNet.UI.Controls
{
    public class Note : Button
    {
        public Note()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Note), new FrameworkPropertyMetadata(typeof(Note)));
        }

        //TITLE

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

        public static readonly DependencyProperty RTBProperty =
        DependencyProperty.Register(
        "RTB", typeof(string), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string RTB
        {
            get => (string)GetValue(RTBProperty);
            set => SetValue(RTBProperty, value);
        }

        // DATE

        public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register(
        "Date", typeof(string), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Date
        {
            get => (string)GetValue(RTBProperty);
            set => SetValue(RTBProperty, value);
        }


    }
}
