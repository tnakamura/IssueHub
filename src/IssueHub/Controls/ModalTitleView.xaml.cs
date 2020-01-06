using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalTitleView : ContentView
    {
        public ModalTitleView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(ModalTitleView),
                defaultValue: null);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty CloseCommandProperty =
            BindableProperty.Create(
                nameof(CloseCommand),
                typeof(ICommand),
                typeof(ModalTitleView),
                defaultValue: null);

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(ModalTitleView),
                defaultValue: Color.Default);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
    }
}