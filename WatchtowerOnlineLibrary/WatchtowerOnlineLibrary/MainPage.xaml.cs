using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WatchtowerOnlineLibrary
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        const string siteUrl = "http://m.wol.jw.org";

        private void webBrowserWOL_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowserWOL.Navigate(new Uri(siteUrl, UriKind.Absolute));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This program requires data connection.", "Notice", MessageBoxButton.OK);
        }
    }
}