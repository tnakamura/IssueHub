using System.Threading.Tasks;
using IssueHub.Pages;
using Xamarin.Forms;

namespace IssueHub
{
    public static class NavigationEx
    {
        public static async Task PushModalWithNavigationPageAsync(this INavigation navigation, Page page)
        {
            await navigation.PushModalAsync(page.WrapNavigationPage());
        }

        public static NavigationPage WrapNavigationPage(this Page page) =>
             new AppNavigationPage(page);
    }
}
