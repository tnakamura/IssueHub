using IssueHub.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelCell), typeof(IssueHub.Droid.Renderers.LabelCellRenderer))]

namespace IssueHub.Droid.Renderers
{
    public class LabelCellRenderer : TextCellRenderer
    {
    }
}