using Xam.Forms.Markdown;
using Xamarin.Forms;

namespace IssueHub.Controls
{
    public sealed class MarkdownCell : ViewCell
    {
        public MarkdownCell()
            : base()
        {
            var markdownView = new MarkdownView();
            markdownView.SetBinding(
                MarkdownView.MarkdownProperty,
                new Binding()
                {
                    Path = nameof(Markdown),
                    Source = this,
                });
            View = markdownView;
			Height = DefaultCellHeight;
        }

        public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(
            nameof(Markdown),
            typeof(string),
            typeof(MarkdownCell),
            propertyChanged: OnMarkdownPropertyChanged);

        static void OnMarkdownPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var oldMarkdown = oldValue as string;
            var newMarkdown = newValue as string;
            if (newMarkdown != oldMarkdown)
            {
                var cell = (MarkdownCell)bindable;
				if (string.IsNullOrEmpty(newMarkdown) ||
					newMarkdown?.Contains(System.Environment.NewLine) == false)
				{
					cell.Height = DefaultCellHeight;
				}
				else
				{
					cell.Height = -1;
				}
                cell.ForceUpdateSize();
            }
        }

        public string Markdown
        {
            get => (string)GetValue(MarkdownProperty);
            set => SetValue(MarkdownProperty, value);
        }
    }
}
