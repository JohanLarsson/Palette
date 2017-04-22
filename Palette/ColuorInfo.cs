namespace Palette
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    public class ColuorInfo : INotifyPropertyChanged
    {
        private string name;
        private SolidColorBrush brush = Brushes.Transparent;

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
                this.OnPropertyChanged(nameof(this.Hex));
                this.OnPropertyChanged(nameof(this.R));
                this.OnPropertyChanged(nameof(this.G));
                this.OnPropertyChanged(nameof(this.B));
                this.OnPropertyChanged(nameof(this.Hue));
                this.OnPropertyChanged(nameof(this.Saturation));
                this.OnPropertyChanged(nameof(this.Value));
            }
        }

        public string Hex
        {
            get => this.Brush.Color.ToString();
            set => this.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public byte R
        {
            get => this.Brush.Color.R;
            set => this.Brush = new SolidColorBrush(Color.FromRgb(value, this.G, this.B));
        }

        public byte G
        {
            get => this.Brush.Color.G;
            set => this.Brush = new SolidColorBrush(Color.FromRgb(this.R, value, this.B));
        }

        public byte B
        {
            get => this.Brush.Color.B;
            set => this.Brush = new SolidColorBrush(Color.FromRgb(this.R, this.G, value));
        }

        public double Hue
        {
            get => Hsv.ColorToHSV(this.brush.Color).Hue;
            set => this.Brush = new SolidColorBrush(Hsv.ColorFromHSV(value, this.Saturation, this.Value));
        }

        public double Saturation
        {
            get => Hsv.ColorToHSV(this.brush.Color).Saturation;
            set => this.Brush = new SolidColorBrush(Hsv.ColorFromHSV(this.Hue, value, this.Value));
        }

        public double Value
        {
            get => Hsv.ColorToHSV(this.brush.Color).Value;
            set => this.Brush = new SolidColorBrush(Hsv.ColorFromHSV(this.Hue, this.Saturation, value));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}