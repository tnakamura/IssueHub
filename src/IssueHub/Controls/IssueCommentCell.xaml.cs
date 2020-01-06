using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IssueCommentCell : ViewCell
    {
        public IssueCommentCell()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty BodyProperty =
            BindableProperty.Create(
                nameof(Body),
                typeof(string),
                typeof(IssueCommentCell),
                defaultValue: null);

        public string Body
        {
            get => (string)GetValue(BodyProperty);
            set => SetValue(BodyProperty, value);
        }

        public static readonly BindableProperty CreatedAtProperty =
            BindableProperty.Create(
                nameof(CreatedAt),
                typeof(DateTimeOffset),
                typeof(IssueCommentCell),
                defaultValue: DateTimeOffset.MinValue);

        public DateTimeOffset CreatedAt
        {
            get => (DateTimeOffset)GetValue(CreatedAtProperty);
            set => SetValue(CreatedAtProperty, value);
        }

        public static readonly BindableProperty LoginProperty =
            BindableProperty.Create(
                nameof(Login),
                typeof(string),
                typeof(IssueCommentCell),
                defaultValue: null);

        public string Login
        {
            get => (string)GetValue(LoginProperty);
            set => SetValue(LoginProperty, value);
        }

        public static readonly BindableProperty AvatarUrlProperty =
            BindableProperty.Create(
                nameof(AvatarUrl),
                typeof(string),
                typeof(IssueCommentCell),
                defaultValue: null);

        public string AvatarUrl
        {
            get => (string)GetValue(AvatarUrlProperty);
            set => SetValue(AvatarUrlProperty, value);
        }
    }
}