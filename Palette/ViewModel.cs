namespace Palette
{
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Repository repository = new Repository();
        private PaletteInfo palette = new PaletteInfo();

        ////public ViewModel()
        ////{
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Red", Brush = Brushes.OrangeRed });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Blue", Brush = Brushes.DodgerBlue });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Green", Brush = Brushes.LimeGreen });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "DisabledGreen", Brush = new SolidColorBrush(Color.FromRgb(175, 212, 161)) });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Order1", Brush = Brushes.YellowGreen });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Order2", Brush = Brushes.MediumPurple });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Order3", Brush = Brushes.LightBlue });
        ////    this.Palette.Colours.Add(new ColuorInfo { Name = "Order4", Brush = Brushes.Coral });
        ////}

        public event PropertyChangedEventHandler PropertyChanged;

        public PaletteInfo Palette
        {
            get => this.palette;
            set
            {
                if (ReferenceEquals(value, this.palette))
                {
                    return;
                }

                this.palette = value;
                this.OnPropertyChanged();
            }
        }

        public bool CanSave(FileInfo file)
        {
            return file != null &&
                   this.palette != null &&
                   this.repository.CanSave(this.palette, file);
        }

        public void Save(FileInfo file)
        {
            this.repository.Save(this.Palette, file);
        }

        public void Read(FileInfo file)
        {
            this.Palette = this.repository.Read(file);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
