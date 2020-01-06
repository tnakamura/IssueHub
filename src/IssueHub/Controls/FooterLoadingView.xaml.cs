using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FooterLoadingView : ContentView
    {
        public FooterLoadingView()
        {
            InitializeComponent();
        }

        public static BindableProperty IsLoadingProperty = BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(FooterLoadingView),
            defaultValue: false);

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
    }
}