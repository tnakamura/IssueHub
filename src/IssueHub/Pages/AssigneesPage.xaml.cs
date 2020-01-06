using IssueHub.States;
using IssueHub.ViewModels;
using ReduxSharp;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace IssueHub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssigneesPage : ContentPage
    {
        public AssigneesPage(IStore<AppState> store)
        {
            InitializeComponent();

            listView.On<iOS>().SetSeparatorStyle(SeparatorStyle.FullWidth);

            ViewModel = new AssigneesPageViewModel(store);
        }

        AssigneesPageViewModel ViewModel
        {
            get => BindingContext as AssigneesPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsAssigneesLoaded.Value)
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