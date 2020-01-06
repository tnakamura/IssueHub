using System;
using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelsPage : ContentPage
    {
        public LabelsPage(IStore<AppState> store)
        {
            InitializeComponent();

            listView.On<iOS>().SetSeparatorStyle(SeparatorStyle.FullWidth);

            ViewModel = new LabelsPageViewModel(store);
        }

        LabelsPageViewModel ViewModel
        {
            get => BindingContext as LabelsPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsLabelsLoaded.Value)
            {
                ViewModel.InitializeCommand.Execute();
            }
        }

        void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            ViewModel.ToggleSelectCommand.Execute(e.SelectedItem);
            var list = (Xamarin.Forms.ListView)sender;
            list.SelectedItem = null;
        }
    }
}