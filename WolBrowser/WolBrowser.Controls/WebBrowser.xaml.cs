///This code is offered as is without any warranties.
///For any information or comments pleas contact:
///Francisco Fernandez
///fj.fernandez@live.com
///Developer

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

using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace WolBrowser.Controls
{
    [ContentPropertyAttribute("Content")]
    public partial class WebBrowser : UserControl
    {
        //ctr
        public WebBrowser()
        {
            InitializeComponent();

            //Set the data context for the bindings.
            ContentGrid.DataContext = this;
        }

        #region Fields

        //The navigation urls of the browser.
        private readonly Stack<Uri> _NavigatingUrls = new Stack<Uri>();

        //The history for the browser
        private readonly ObservableCollection<string> _History = 
            new ObservableCollection<string>();

        //Flag to check if the browser is navigating back.
        bool _IsNavigatingBackward = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the History property for the browser.
        /// </summary>
        public ObservableCollection<string> History
        {
            get { return _History; }
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// ShowProgress Dependency Property
        /// </summary>
        public static readonly DependencyProperty ShowProgressProperty =
            DependencyProperty.Register("ShowProgress", typeof(bool), 
            typeof(WebBrowser), new PropertyMetadata((bool)false));

        /// <summary>
        /// Gets or sets the ShowProgress property. This dependency property 
        /// indicates whether to show the progress bar.
        /// </summary>
        public bool ShowProgress
        {
            get { return (bool)GetValue(ShowProgressProperty); }
            set { SetValue(ShowProgressProperty, value); }
        }

        /// <summary>
        /// CanNavigateBack Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanNavigateBackProperty =
            DependencyProperty.Register("CanNavigateBack", typeof(bool), 
            typeof(WebBrowser), new PropertyMetadata((bool)false));

        /// <summary>
        /// Gets or sets the CanNavigateBack property. This dependency property 
        /// indicates whether the browser can go back.
        /// </summary>
        public bool CanNavigateBack
        {
            get { return (bool)GetValue(CanNavigateBackProperty); }
            set { SetValue(CanNavigateBackProperty, value); }
        }

        /// <summary>
        /// InitialUri Dependency Property
        /// </summary>
        public static readonly DependencyProperty InitialUriProperty =
            DependencyProperty.Register("InitialUri", typeof(string), 
            typeof(WebBrowser), new PropertyMetadata((string)String.Empty));

        /// <summary>
        /// Gets or sets the InitialUri property. This dependency property 
        /// indicates the initial uri for the browser.
        /// </summary>
        public string InitialUri
        {
            get { return (string)GetValue(InitialUriProperty); }
            set { SetValue(InitialUriProperty, value); }
        }

        #endregion Dependency Properties

        #region Event Handlers

        void TheWebBrowser_Navigating(object sender, 
            Microsoft.Phone.Controls.NavigatingEventArgs e)
        {
            //We show the progress bar when we start navigating.
            ShowProgress = true;
        }

        void TheWebBrowser_Navigated(object sender, 
            System.Windows.Navigation.NavigationEventArgs e)
        {
            //If we are Navigating Backward and we Can Navigate back, 
            //remove the last uri from the stack.
            if (_IsNavigatingBackward == true && CanNavigateBack)
                _NavigatingUrls.Pop();
            //Else we are navigating forward so we need to add the uri 
            //to the stack.
            else
            {
                _NavigatingUrls.Push(e.Uri);

                //If we do not have the navigated uri in our history 
                //we add it.
                if (!_History.Contains(e.Uri.ToString()))
                    _History.Add(e.Uri.ToString());
            }

            //If there is one address left you can't go back.
            if (_NavigatingUrls.Count > 1)
                CanNavigateBack = true;
            else
                CanNavigateBack = false;

            //Finally we hide the progress bar.
            ShowProgress = false;
        }

        private void TheWebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            //When we load our browser if we specified an initial uri
            //we navigate to it.
            if(!String.IsNullOrEmpty(InitialUri))
                TheWebBrowser.Navigate(new Uri(InitialUri));
        }

        #endregion Event Handlers

        #region Public Methods

        /// <summary>
        /// Used to navigate forward.
        /// </summary>
        public void NavigateForward()
        {
            _IsNavigatingBackward = false;
            TheWebBrowser.InvokeScript("eval", "history.go(1)");
        }

        /// <summary>
        /// Used to refresh the browser.
        /// </summary>
        public void RefreshBrowser()
        {
            TheWebBrowser.InvokeScript("eval", "history.go()");
        }

        /// <summary>
        /// Used to navigate back.
        /// </summary>
        public void NavigateBack()
        {
            _IsNavigatingBackward = true;
            TheWebBrowser.InvokeScript("eval", "history.go(-1)");
        }

        /// <summary>
        /// Used to navigate to a specified url.
        /// </summary>
        /// <param name="Url">The web address.</param>
        public void Navigate(string Url)
        {
            TheWebBrowser.Navigate(new Uri(Url, UriKind.Absolute));
        }

        #endregion
    }
}
