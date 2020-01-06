using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace IssueHub.Pages
{
    public sealed class AppNavigationPage : NavigationPage
    {
        public AppNavigationPage()
            : base()
        {
            Setup();
        }

        public AppNavigationPage(Page root)
            : base(root)
        {
            Setup();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Setup()
        {
            BarBackgroundColor = AppTheme.NavigationBarBackgroundColor;
            BarTextColor = AppTheme.NavigationBarTextColor;
            Popped += AppNavigationPage_Popped;
        }

        void AppNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            (e.Page.BindingContext as IDisposable)?.Dispose();
        }
    }
}
