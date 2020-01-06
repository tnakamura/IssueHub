using System;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;

namespace IssueHub.Pages
{
    public partial class NewCommentPage : ContentPage
    {
        public NewCommentPage(IStore<AppState> store)
        {
            InitializeComponent();

            ViewModel = new NewCommentPageViewModel(
                store: store,
                dialogs: Acr.UserDialogs.UserDialogs.Instance);

            ViewModel.RequestPopModal += HandleRequestPopModal;
        }

        NewCommentPageViewModel ViewModel
        {
            get => BindingContext as NewCommentPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            bodyEditor.Focus();
        }

        async void HandleRequestPopModal(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
