using Xamarin.Forms;

namespace IssueHub.Controls
{
    public class LabelCell : TextCell
    {
        public LabelCell()
            : base()
        {
        }

        public static readonly BindableProperty AccessoryProperty = BindableProperty.Create(
            nameof(Accessory),
            typeof(CellAccessory),
            typeof(LabelCell),
            defaultValue: CellAccessory.None);

        public CellAccessory Accessory
        {
            get => (CellAccessory)GetValue(AccessoryProperty);
            set => SetValue(AccessoryProperty, value);
        }
    }

    public enum CellAccessory
    {
        None = 0,
        DisclosureIndicator = 1,
        DetailDisclosureButton = 2,
        Checkmark = 3,
        DetailButton = 4
    }
}
