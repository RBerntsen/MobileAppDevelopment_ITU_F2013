using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace _1887.App
{
    public partial class Browser : PhoneApplicationPage
    {
        public Browser()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string url;

            if (this.NavigationContext.QueryString.ContainsKey("urlToVisit"))
            {
                url = this.NavigationContext.QueryString["urlToVisit"];
            }
            else
            {
                url = "http://www.ob.dk";
            }

            System.Uri uri = new System.Uri(url, UriKind.Absolute); ;

            InternalWebrowser.Source = uri;
        }

        private void WebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
        }

        private void abibBack_Click(object sender, EventArgs e)
        {
            if(NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void InternalWebrowser_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}