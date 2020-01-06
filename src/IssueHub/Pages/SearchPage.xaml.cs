using System;
using IssueHub.Actions;
using IssueHub.Models;
using IssueHub.States;
using IssueHub.ViewModels;
using Octokit;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage(IStore<AppState> store)
            : base()
        {
            InitializeComponent();

            ViewModel = new SearchPageViewModel(store);
            ViewModel.RequestPopModal += ViewModel_RequestPopModal;

            listView.On<iOS>()
                .SetSeparatorStyle(SeparatorStyle.FullWidth);
        }

        SearchPageViewModel ViewModel
        {
            get => BindingContext as SearchPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            searchBar.Focus();
        }

        async void ViewModel_RequestPopModal(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void CancelToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var issue = (Issue)e.SelectedItem;
            ViewModel.Store.Dispatch(new SelectSearchedIssue(issue));
            await Navigation.PushAsync(
                new IssueCommentsPage(ViewModel.Store));
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }
    }
}