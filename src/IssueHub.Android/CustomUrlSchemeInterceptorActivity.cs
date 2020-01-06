using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using IssueHub.Helpers;

namespace IssueHub.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        actions: new[] { Intent.ActionView },
        Categories = new[]
        {
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataSchemes = new[]
        {
            Secrets.AndroidDataScheme,
        },
        DataPaths = new[]
        {
            Secrets.AndroidDataPath,
        }
    )]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri_android = Intent.Data;

#if DEBUG
            var sb = new StringBuilder();
            sb.AppendLine("CustomUrlSchemeInterceptorActivity.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            var uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            AuthenticationState.Authenticator?.OnPageLoading(uri_netfx);

            Finish();
        }
    }
}