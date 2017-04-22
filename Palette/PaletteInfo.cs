namespace Palette
{
    using System.Collections.ObjectModel;

    public class PaletteInfo
    {
        public ObservableCollection<ColuorInfo> Colours { get; } = new ObservableCollection<ColuorInfo>();
    }
}