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

using Coding4Fun.Phone.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;

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
        Uri lastUri;

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();
#if DEBUG
            //isConnected = false;
#endif

            if (isConnected)
            {
                ShowToastDataNotice();
            }
            else
            {
                ShowToastNetworkNotice();
            }
        }

        private void ShowToastNetworkNotice()
        {
            //MessageBox replaced with Toast Prompt from http://coding4fun.codeplex.com/ page.
            var toastPromptNetworkNotice = new ToastPrompt
            {
                Title = "No Data Connection",
                Message = "This program requires data connection." + Environment.NewLine + "Check your data connection." + Environment.NewLine,
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                //ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            };
            toastPromptNetworkNotice.Completed += toastPromptNetworkNotice_Completed;
            toastPromptNetworkNotice.Show();
        }

        public void toastPromptNetworkNotice_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }

        private void ShowToastDataNotice()
        {
            //MessageBox replaced with Toast Prompt from http://coding4fun.codeplex.com/ page.
            var toastPromptDataNotice = new ToastPrompt
            {
                Background = new SolidColorBrush(Colors.Cyan),
                Foreground = new SolidColorBrush(Colors.Black),
                Title = "Data Usage Notice",
                Message = "This program requires data connection." + Environment.NewLine + "You might increase your mobile data usage." + Environment.NewLine,
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                //ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            };
            toastPromptDataNotice.Completed += toastPromptDataNotice_Completed;
            toastPromptDataNotice.Show();
        }

        public void toastPromptDataNotice_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            webBrowserWOL.Navigate(new Uri(siteUrl, UriKind.Absolute));

            ////Opens link in default browser.
            //WebBrowserTask webBrowserTask = new WebBrowserTask();
            //webBrowserTask.Uri = new Uri(siteUrl, UriKind.Absolute);
            //webBrowserTask.Show();
        }

        private void appBarIconButtonRefresh_Click(object sender, EventArgs e)
        {
            webBrowserWOL.Navigate(lastUri);
        }

        private void appBarIconButtonAbout_Click(object sender, EventArgs e)
        {
            //About dialog by YLAD from http://ylad.codeplex.com/ page.
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }

        private void webBrowserWOL_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            lastUri = e.Uri;
        }
    }
}