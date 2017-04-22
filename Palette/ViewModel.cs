namespace Palette
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public class ViewModel
    {
        public ViewModel()
        {
            this.Colours = new ObservableCollection<ColuorInfo>
            {
                new ColuorInfo { Name = "Red", Brush = Brushes.OrangeRed },
                new ColuorInfo { Name = "Blue", Brush = Brushes.DodgerBlue },
                new ColuorInfo { Name = "Green", Brush = Brushes.LimeGreen },
                new ColuorInfo { Name = "DisabledGreen", Brush = new SolidColorBrush(Color.FromRgb(175, 212, 161)) },
                new ColuorInfo { Name = "Order1", Brush = Brushes.YellowGreen },
                new ColuorInfo { Name = "Order2", Brush = Brushes.MediumPurple },
                new ColuorInfo { Name = "Order3", Brush = Brushes.LightBlue },
                new ColuorInfo { Name = "Order4", Brush = Brushes.Coral },
            };
        }

        public ObservableCollection<ColuorInfo> Colours { get; } = new ObservableCollection<ColuorInfo>();
    }
}
