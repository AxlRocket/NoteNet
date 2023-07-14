﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        public static bool Show(Window parent, string message, bool caution = false, string title = "")
        {
            Message msgBox = new Message();
            msgBox.Owner = parent;

            if (title != "")
            {
                msgBox.Title.Text = title;
            }
            else
            {
                msgBox.Title.Text = (string)Application.Current.Resources["Message.Warning"];
            }

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
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}