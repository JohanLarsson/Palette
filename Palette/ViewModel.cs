namespace Palette
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Repository repository = new Repository();
        private PaletteInfo palette = new PaletteInfo();

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
            if (string.Equals(file.Extension , ".xaml", StringComparison.OrdinalIgnoreCase))
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
