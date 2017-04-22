namespace Palette
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Palette.Properties;

    public class ViewModel : INotifyPropertyChanged
    {
        // Lazy for designtime to work.
        private readonly Lazy<Repository> repository = new Lazy<Repository>(() => new Repository());

        private PaletteInfo palette = new PaletteInfo();
        private ColuorInfo selectedColour;

        public ViewModel()
        {
#if DEBUG
            this.palette = Newtonsoft.Json.JsonConvert.DeserializeObject<PaletteInfo>(
                Resources.SamplePalette,
                Repository.CreateJsonSettings());
#endif
        }

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
                this.SelectedColour = null;
                this.OnPropertyChanged();
            }
        }

        public ColuorInfo SelectedColour
        {
            get => this.selectedColour;
            set
            {
                if (ReferenceEquals(value, this.selectedColour))
                {
                    return;
                }

                this.selectedColour = value;
                this.OnPropertyChanged();
            }
        }

        public bool CanSave(FileInfo file)
        {
            return file != null &&
                   this.palette != null &&
                   this.repository.Value.CanSave(this.palette, file);
        }

        public void Save(FileInfo file)
        {
            if (string.Equals(file.Extension, ".xaml", StringComparison.OrdinalIgnoreCase))
            {
                using (var writer = new StreamWriter(File.OpenWrite(file.FullName)))
                {
                    writer.WriteLine("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" ");
                    writer.WriteLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
                    foreach (var colour in this.palette.Colours)
                    {
                        writer.WriteLine($"    <Color x:Key=\"{colour.Name}\">{colour.Hex}</Color>");
                    }

                    writer.WriteLine();
                    foreach (var colour in this.palette.Colours)
                    {
                        writer.WriteLine($"    <SolidColorBrush x:Key=\"{colour.Name}Brush\" Color=\"{{StaticResource {colour.Name}}}\" />");
                    }

                    writer.WriteLine("</ResourceDictionary>");
                }

                return;
            }

            this.repository.Value.Save(this.Palette, file);
        }

        public void Read(FileInfo file)
        {
            this.Palette = this.repository.Value.Read(file);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
