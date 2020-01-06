using System;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditIssuePage : ContentPage
    {
        public EditIssuePage(IStore<AppState> store)
            : base()
        {
            InitializeComponent();

            ViewModel = new EditIssuePageViewModel(
                store: store,
                dialogs: Acr.UserDialogs.UserDialogs.Instance);
            ViewModel.RequestPopModal += HandleRequestPopModal;
        }

        EditIssuePageViewModel ViewModel
        {
            get => BindingContext as EditIssuePageViewModel;
            set => BindingContext = value;
        }

        async void HandleRequestPopModal(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void HandleLabelsCellTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new LabelsPage(ViewModel.Store));
        }

        async void HandleAssigneeCellTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new AssigneesPage(ViewModel.Store));
        }

        async void HandleIssueBodyCellTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalWithNavigationPageAsync(
                new EditIssueBodyPage(ViewModel.Store));
        }
    }
}