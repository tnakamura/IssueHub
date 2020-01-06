using System;
using Octokit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IssueHub.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepositoryIssueCell : ViewCell
    {
        public RepositoryIssueCell()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(RepositoryIssueCell),
                defaultValue: null);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty NumberProperty =
            BindableProperty.Create(
                nameof(Number),
                typeof(int),
                typeof(RepositoryIssueCell),
                defaultValue: 0);

        public int Number
        {
            get => (int)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }

        public static readonly BindableProperty RepositoryFullNameProperty =
            BindableProperty.Create(
                nameof(RepositoryFullName),
                typeof(string),
                typeof(RepositoryIssueCell),
                defaultValue: null);

        public string RepositoryFullName
        {
            get => (string)GetValue(RepositoryFullNameProperty);
            set => SetValue(RepositoryFullNameProperty, value);
        }

        public static readonly BindableProperty UserLoginProperty =
            BindableProperty.Create(
                nameof(UserLogin),
                typeof(string),
                typeof(RepositoryIssueCell),
                defaultValue: null);

        public string UserLogin
        {
            get => (string)GetValue(UserLoginProperty);
            set => SetValue(UserLoginProperty, value);
        }

        public static readonly BindableProperty CreatedAtProperty =
            BindableProperty.Create(
                nameof(CreatedAt),
                typeof(DateTimeOffset),
                typeof(RepositoryIssueCell),
                defaultValue: DateTimeOffset.MinValue);

        public DateTimeOffset CreatedAt
        {
            get => (DateTimeOffset)GetValue(CreatedAtProperty);
            set => SetValue(CreatedAtProperty, value);
        }

        public static readonly BindableProperty StateProperty =
            BindableProperty.Create(
                nameof(State),
                typeof(ItemState),
                typeof(RepositoryIssueCell),
                defaultValue: ItemState.Open);

        public ItemState State
        {
            get => (ItemState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
    }
}