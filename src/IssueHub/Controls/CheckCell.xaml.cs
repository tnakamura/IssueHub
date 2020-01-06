using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckCell : ViewCell
    {
        public CheckCell()
        {
            InitializeComponent();
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CheckCell),
            defaultValue: null);

        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }


        public static BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(CheckCell),
            defaultValue: false);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
    }
}