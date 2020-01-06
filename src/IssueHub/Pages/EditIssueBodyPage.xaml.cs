using System;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditIssueBodyPage : ContentPage
    {
        public EditIssueBodyPage(IStore<AppState> store)
        {
            InitializeComponent();

            ViewModel = new EditIssueBodyPageViewModel(store);
            ViewModel.RequestPopModal += HandleRequestPopModal;
        }

        EditIssueBodyPageViewModel ViewModel
        {
            get => BindingContext as EditIssueBodyPageViewModel;
            set => BindingContext = value;
        }

        async void HandleRequestPopModal(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            bodyEditor.Focus();
        }
    }
}