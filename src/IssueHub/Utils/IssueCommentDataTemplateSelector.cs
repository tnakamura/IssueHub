using IssueHub.ViewModels;
using Xamarin.Forms;

namespace IssueHub.Utils
{
    public class IssueCommentDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IssueBodyTemplate { get; set; }

        public DataTemplate IssueCommentTemplate { get; set; }

        public DataTemplate IssueTitleTemplate { get; set; }

        public DataTemplate IssueLabelsTemplate { get; set; }

        public DataTemplate IssueAssigneesTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IssueTitleViewModel)
            {
                return IssueTitleTemplate;
            }
            else if (item is IssueAssigneesViewModel)
            {
                return IssueAssigneesTemplate;
            }
            else if (item is IssueLabelsViewModel)
            {
                return IssueLabelsTemplate;
            }
            else if (item is IssueBodyViewModel)
            {
                return IssueBodyTemplate;
            }
            else
            {
                return IssueCommentTemplate;
            }
        }
    }
}
