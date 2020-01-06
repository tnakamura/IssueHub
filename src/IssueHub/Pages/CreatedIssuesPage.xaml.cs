using System;
using IssueHub.Actions;
using IssueHub.Converters;
using IssueHub.Models;
using IssueHub.States;
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
    public partial class CreatedIssuesPage : ContentPage
    {
        public CreatedIssuesPage(IStore<AppState> store)
            : base()
        {
            InitializeComponent();

            ViewModel = new CreatedIssuesPageViewModel(store);

            SetupToolbarItems();

            listView.On<iOS>()
                .SetSeparatorStyle(SeparatorStyle.FullWidth);
        }

        CreatedIssuesPageViewModel ViewModel
        {
            get => BindingContext as CreatedIssuesPageViewModel;
            set => BindingContext = value;
        }

        void SetupToolbarItems()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var actionItem = new ToolbarItem
                {
                    Order = ToolbarItemOrder.Primary,
                    IconImageSource = Utils.Octicons.GetImageSource(
                        Utils.Octicons.Glyph.KebabHorizontal,
                        20),
                };
                actionItem.Clicked += HandleActionButtonClicked;
                ToolbarItems.Add(actionItem);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var showClosedIssuesItem = new ToolbarItem
                {
                    Order = ToolbarItemOrder.Secondary,
                    Command = ViewModel.ToggleClosedIssuesCommand,
                };
                showClosedIssuesItem.SetBinding(
                    ToolbarItem.TextProperty,
                    new Binding
                    {
                        Path = nameof(ViewModel.ShowClosedIssues.Value),
                        Source = ViewModel.ShowClosedIssues,
                        Converter = new BooleanTextConverter
                        {
                            TrueText = R.HideClosedIssues,
                            FalseText = R.ShowClosedIssues,
                        },
                    });
                ToolbarItems.Add(showClosedIssuesItem);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsInitialized.Value)
            {
                ViewModel.InitializeCommand.Execute();
            }
        }

        async void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var issue = (Issue)e.SelectedItem;
            ViewModel.Store.Dispatch(new SelectCreatedIssue(issue));
            await Navigation.PushAsync(
                new IssueCommentsPage(ViewModel.Store));
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        async void HandleAddButtonClicked(object sender, EventArgs e)
        {
            ViewModel.Store.Dispatch(new Actions.NewIssue());
            await Navigation.PushModalWithNavigationPageAsync(
                new EditIssuePage(ViewModel.Store));
        }

        async void HandleActionButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet(
                null,
                R.Cancel,
                null,
                ViewModel.ShowClosedIssues.Value ? R.HideClosedIssues : R.ShowClosedIssues);

            if (result == R.ShowClosedIssues || result == R.HideClosedIssues)
            {
                ViewModel.ToggleClosedIssuesCommand.Execute();
            }
        }
    }
}