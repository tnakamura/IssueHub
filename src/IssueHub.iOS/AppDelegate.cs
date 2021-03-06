﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using IssueHub.Utils;

namespace IssueHub.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");

            global::Xamarin.Forms.Forms.Init();

            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Logger.LogError(e.ExceptionObject as Exception, "Unhandled exception raised.");
            };

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (url.AbsoluteString.StartsWith(Helpers.Secrets.GitHubAuthorizationCallbackUrl, StringComparison.OrdinalIgnoreCase))
            {
                AuthenticationState.Authenticator?.OnPageLoading(new Uri(url.AbsoluteString));
            }
            return true;
        }
    }
}
