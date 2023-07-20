using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace NoteNet.UI.Controls
{
    public class Note : Button
    {
        public Note()
        {
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

        // DATE

        public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register(
        "Date", typeof(string), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Date
        {
            get => (string)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        // Test

        public static readonly DependencyProperty RTBContentProperty =
        DependencyProperty.Register(
        "RTBContent", typeof(FlowDocument), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FlowDocument RTBContent
        {
            get => (FlowDocument)GetValue(RTBContentProperty);
            set => SetValue(RTBContentProperty, value);
        }
    }
}
