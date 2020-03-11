using IssueHub.ViewModels;
using Xamarin.Forms;

namespace IssueHub.Utils
{
    public class RepositoryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PublicTemplate { get; set; }

        public DataTemplate PrivateTemplate { get; set; }

        public DataTemplate PublicFavoriteTemplate { get; set; }

        public DataTemplate PrivateFavoriteTemplate { get; set; }

        public DataTemplate SmartListTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is RepositoryViewModel repository)
            {
                if (repository.Private)
                {
                    return PrivateTemplate;
                }
                else
                {
                    return PublicTemplate;
                }
            }
            else if (item is FavoriteViewModel favorite)
            {
                if (favorite.Private)
                {
                    return PrivateFavoriteTemplate;
                }
                else
                {
                    return PublicFavoriteTemplate;
                }
            }
            else if (item is SmartListViewModel)
            {
                return SmartListTemplate;
            }
            else
            {
                return null;
            }
        }
    }
}
