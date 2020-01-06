using System.ComponentModel;
using IssueHub.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LabelCell), typeof(IssueHub.iOS.Renderers.LabelCellRenderer))]

namespace IssueHub.iOS.Renderers
{
    public sealed class LabelCellRenderer : CellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
			var nativeCell = reusableCell as NativeLabelCell;
			if (nativeCell == null)
			{
				nativeCell = new NativeLabelCell(item);
			}
			else
			{
				nativeCell.Cell.PropertyChanged -= nativeCell.CellPropertyChanged;
			}

			nativeCell.Cell = item;

			item.PropertyChanged += nativeCell.CellPropertyChanged;

			nativeCell.UpdateCell();

			return nativeCell;
        }
    }

	class NativeLabelCell : CellTableViewCell
	{
		LabelCell LabelCell => Cell as LabelCell;

		public NativeLabelCell(Cell formsCell)
			: base(UITableViewCellStyle.Value1, formsCell.GetType().FullName)
		{
			Cell = formsCell;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LabelCell.PropertyChanged -= CellPropertyChanged;
			}
			base.Dispose(disposing);
		}

		public void UpdateCell()
		{
			UpdateText();
			UpdateDetail();
			UpdateAccessory();
		}

		public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == TextCell.TextProperty.PropertyName)
			{
				UpdateText();
			}
			else if (e.PropertyName == TextCell.DetailProperty.PropertyName)
			{
				UpdateDetail();
			}
			else if (e.PropertyName == LabelCell.AccessoryProperty.PropertyName)
			{
				UpdateAccessory();
			}
		}

		void UpdateText()
		{
			TextLabel.Text = LabelCell.Text;
		}

		void UpdateDetail()
		{
			DetailTextLabel.Text = LabelCell.Detail;
		}

		void UpdateAccessory()
		{
			Accessory = GetNativeAccessory(LabelCell.Accessory);
		}

		UITableViewCellAccessory GetNativeAccessory(CellAccessory accessory)
		{
			switch (accessory)
			{
				case CellAccessory.Checkmark:
					return UITableViewCellAccessory.Checkmark;
				case CellAccessory.DisclosureIndicator:
					return UITableViewCellAccessory.DisclosureIndicator;
				case CellAccessory.DetailDisclosureButton:
					return UITableViewCellAccessory.DetailDisclosureButton;
				case CellAccessory.DetailButton:
					return UITableViewCellAccessory.DetailButton;
				default:
					return UITableViewCellAccessory.None;
			}
		}
	}
}
