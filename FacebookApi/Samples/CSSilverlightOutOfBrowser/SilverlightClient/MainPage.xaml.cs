﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class MainPage : UserControl
    {
        private const string appId = "{your_app_id_here}";

        private string requestedFbPermissions = "user_about_me";

        private const string successUrl = @"http://localhost:18201/LoginSuccessful.htm";

        private const string failedUrl = @"http://localhost:18201/LoginUnsuccessful.htm";

        private bool loggedIn = false;

        Uri loggingInUri;

        private string accessToken;

        private FacebookApp fbApp;

        private void failedLogin()
        {
            // We're on the failed page (we could notify the user that there was an issue here..)
        }

        private void loginSucceeded(NotifyEventArgs e)
        {
            TitleBox.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            fbApp.GetAsync("me", (val) =>
            {
                var result = (IDictionary<string, object>)val.Result;
                Dispatcher.BeginInvoke(() => InfoBox.ItemsSource = result);
            });
        }

        public MainPage()
        {
            InitializeComponent();
            fbApp = new FacebookApp();
        }

        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                LoginToFacebook();
            }
        }

        private void LoginToFacebook()
        {
            TitleBox.Visibility = Visibility.Collapsed;
            FacebookLoginBrowser.Visibility = Visibility.Visible;
            InfoBox.Visibility = Visibility.Collapsed;

            dynamic parms = new System.Dynamic.ExpandoObject();
            parms.display = "popup";
            parms.client_id = appId;
            parms.redirect_uri = successUrl;
            parms.cancel_url = failedUrl;
            parms.scope = requestedFbPermissions;
            parms.type = "user_agent";

            // TODO: figure out why this temporary hack is necessary
            loggingInUri = fbApp.GetLoginUrl(parms);

            FacebookLoginBrowser.Source = (loggingInUri);
        }

        private void FacebookLoginBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value != "Failed")
            {
                FacebookAuthenticationResult authResult;
                if (FacebookAuthenticationResult.TryParse(e.Value, out authResult))
                {
                    fbApp.Session = authResult.ToSession();
                    loggedIn = true;
                    loginSucceeded(e);
                }
            }


            if (fbApp.Session == null)
            {
                failedLogin();
            }
        }
    }
}