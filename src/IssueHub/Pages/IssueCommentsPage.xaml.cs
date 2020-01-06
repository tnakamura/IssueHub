using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IssueHub.Actions;
using IssueHub.States;
using IssueHub.Utils;
using IssueHub.ViewModels;
using Octokit;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using R = IssueHub.Properties.Resources;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IssueCommentsPage : ContentPage
    {
        public IssueCommentsPage(IStore<AppState> store)
            : base()
        {
            InitializeComponent();

            listView.On<iOS>().SetSeparatorStyle(SeparatorStyle.FullWidth);

            ViewModel = new IssueCommentsPageViewModel(
                store,
                UserDialogs.Instance);

            if (Device.RuntimePlatform == Device.iOS)
            {
                var actionButton = new ToolbarItem
                {
                    IconImageSource = Octicons.GetImageSource(Octicons.Glyph.KebabHorizontal, 20),
                    Order = ToolbarItemOrder.Primary,
                };
                actionButton.Clicked += HandleActionButtonClicked;
                ToolbarItems.Add(actionButton);
            }
            else
            {
                var editButton = new ToolbarItem
                {
                    Text = R.EditIssue,
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 1,
                };
                editButton.Clicked += HandleEditButtonClicked;
                ToolbarItems.Add(editButton);
                if (ViewModel.Issue.State == ItemState.Open)
                {
                    ToolbarItems.Add(new ToolbarItem
                    {
                        Text = R.CloseIssue,
                        Order = ToolbarItemOrder.Secondary,
                        Priority = 2,
                        Command = ViewModel.CloseIssueCommand,
                    });
                }
                else
                {
                    ToolbarItems.Add(new ToolbarItem
                    {
                        Text = R.ReopenIssue,
                        Order = ToolbarItemOrder.Secondary,
                        Priority = 2,
                        Command = ViewModel.ReopenIssueCommand,
                    });
                }
            }
        }

        IssueCommentsPageViewModel ViewModel
        {
            get => BindingContext as IssueCommentsPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsInitialized.Value)
            {
                ViewModel.InitializeCommand.Execute();
            }
        }

        async void HandleAddCommentButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalWithNavigationPageAsync(
                new NewCommentPage(ViewModel.Store));
        }

        async void HandleActionButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet(
                null,
                R.Cancel,
                ViewModel.Issue.State == ItemState.Open ? R.CloseIssue : R.ReopenIssue,
                R.EditIssue);
            if (result == R.CloseIssue)
            {
                CloseIssue();
            }
            else if (result == R.ReopenIssue)
            {
                ReopenIssue();
            }
            else if (result == R.EditIssue)
            {
                await DisplayEditIssuePageAsync();
            }
        }

        async void HandleEditButtonClicked(object sender, EventArgs e)
        {
            await DisplayEditIssuePageAsync();
        }

        void CloseIssue()
        {
            ViewModel.CloseIssueCommand.Execute();
        }

        void ReopenIssue()
        {
            ViewModel.ReopenIssueCommand.Execute();
        }

        async Task DisplayEditIssuePageAsync()
        {
            ViewModel.Store.Dispatch(
                new EditIssue(ViewModel.Store.State.IssueComments.Issue));
            await Navigation.PushModalWithNavigationPageAsync(
                new EditIssuePage(ViewModel.Store));
        }

        void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            var list = (Xamarin.Forms.ListView)sender;
            list.SelectedItem = null;
        }
    }
}