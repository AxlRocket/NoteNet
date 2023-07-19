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
            if (rtbTest != null)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("null");
            }
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

        // Test

        public static readonly DependencyProperty rtbTestProperty =
        DependencyProperty.Register(
        "rtbTest", typeof(FlowDocument), typeof(Note),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FlowDocument rtbTest
        {
            get => (FlowDocument)GetValue(rtbTestProperty);
            set => SetValue(rtbTestProperty, value);
        }
    }
}
