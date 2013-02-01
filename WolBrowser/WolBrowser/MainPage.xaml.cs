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

using Microsoft.Phone.Net.NetworkInformation;
using WolBrowser.Controls;
using Coding4Fun.Phone.Controls;

namespace WolBrowser
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //webBrowserWOL.Navigate(new Uri(siteUrl, UriKind.Absolute));
        }

        const string siteUrl = "http://m.wol.jw.org";

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
                Message = "This program requires data connection." + Environment.NewLine + "Check your data connection again." + Environment.NewLine,
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
                Message = "This program requires data connection." + Environment.NewLine + "You will increase your mobile data or WiFi usage." + Environment.NewLine,
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                MillisecondsUntilHidden = 2000,
                //ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            };
            toastPromptDataNotice.Completed += toastPromptDataNotice_Completed;
            toastPromptDataNotice.Show();
        }

        public void toastPromptDataNotice_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            webBrowserWOL.Navigate(siteUrl);
            //webBrowserWOL.Navigate(new Uri(siteUrl, UriKind.Absolute));
        }

        private void appBarIconButtonBack_Click(object sender, EventArgs e)
        {
            webBrowserWOL.NavigateBack();
        }

        ////Untested workaround taken from http://blog.jerrynixon.com/2011/11/mango-sample-exit-application.html page.
        //private void Exit()
        //{
        //    while (NavigationService.BackStack.Any())
        //        NavigationService.RemoveBackEntry();

        //    this.IsHitTestVisible = this.IsEnabled = false;

        //    if (this.ApplicationBar != null)
        //        foreach (var item in this.ApplicationBar.Buttons
        //            .OfType<ApplicationBarIconButton>())
        //            item.IsEnabled = false;
        //}

        private void appBarIconButtonRefresh_Click(object sender, EventArgs e)
        {
            webBrowserWOL.RefreshBrowser();
        }

        private void appBarIconButtonForward_Click(object sender, EventArgs e)
        {
            webBrowserWOL.NavigateForward();
        }

        private void appBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void ShowAboutDialog()
        {
            //About dialog by YLAD from http://ylad.codeplex.com/ page.
            //Recommended way of installing the library is to use NuGet extension.
            //Install-Package YLAD
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //An option to use the device's physical back button to navigate webpages. But, it is redundant since the browser control has a built-in back button too.
            //if (webBrowserWOL.CanNavigateBack)
            //{
            //    e.Cancel = true;
            //    webBrowserWOL.NavigateBack();
            //}
            //else

            //Give user a choice to close program at anytime.
            if (MessageBox.Show("Close program?", "Exit", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                e.Cancel = true;
            base.OnBackKeyPress(e);
        }
    }
}