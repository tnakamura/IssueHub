using System;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;
using R = IssueHub.Properties.Resources;

namespace IssueHub.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(IStore<AppState> store)
        {
            InitializeComponent();

            ViewModel = new SettingsPageViewModel(store);
        }

        SettingsPageViewModel ViewModel
        {
            get => BindingContext as SettingsPageViewModel;
            set => BindingContext = value;
        }

        async void HandleLogoutTapped(object sender, EventArgs e)
        {
            if (await DisplayAlert(R.Logout, R.DoYouWantToLogout, R.Yes, R.No))
            {
                ViewModel.LogoutCommand.Execute(null);
            }
        }

        async void HandleAcknowledgementsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AcknowledgementsPage());
        }

        async void HandleDoneClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
