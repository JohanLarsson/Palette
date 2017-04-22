namespace Palette
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    public class ColuorInfo : INotifyPropertyChanged
    {
        private string name;
        private SolidColorBrush brush;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => this.name;

            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public SolidColorBrush Brush
        {
            get => this.brush;

            set
            {
                if (ReferenceEquals(value, this.brush))
                {
                    return;
                }

                this.brush = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Color));
                this.OnPropertyChanged(nameof(this.Hsv));
            }
        }

        public Color Color => this.brush.Color;

        public Hsv Hsv => Hsv.ColorToHSV(this.brush.Color);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}