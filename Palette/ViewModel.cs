namespace Palette
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reactive.Disposables;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Gu.Reactive;
    using Palette.Properties;

    public sealed class ViewModel : INotifyPropertyChanged, IDisposable
    {
        // Lazy for designtime to work.
        private readonly Lazy<Repository> repository = new Lazy<Repository>(() => new Repository());
        private readonly StringBuilder xamlBuilder = new StringBuilder();
        private readonly SerialDisposable disposable = new SerialDisposable();

        private PaletteInfo palette;
        private ColuorInfo selectedColour;
        private bool disposed;

        public ViewModel()
        {
#if DEBUG
            this.palette = Newtonsoft.Json.JsonConvert.DeserializeObject<PaletteInfo>(
                Resources.SamplePalette,
                Repository.CreateJsonSettings());
#else
            this.palette = new PaletteInfo();
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
                if (value == null)
                {
                    this.disposable.Disposable = null;
                }
                else
                {
                    this.disposable.Disposable = value.Colours.ObserveItemPropertyChangedSlim(x => x.Brush, signalInitial: false)
                        .Subscribe(_ => this.OnPropertyChanged(nameof(this.Xaml)));
                }

                this.OnPropertyChanged(nameof(this.Xaml));
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

        public string Xaml
        {
            get
            {
                this.xamlBuilder.Clear();
                using (var writer = new StringWriter(this.xamlBuilder))
                {
                    this.WriteXaml(writer);
                    return this.xamlBuilder.ToString();
                }
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
                    this.WriteXaml(writer);
                }

                return;
            }

            this.repository.Value.Save(this.Palette, file);
        }

        private void WriteXaml(TextWriter writer)
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
                writer.WriteLine(
                    $"    <SolidColorBrush x:Key=\"{colour.Name}Brush\" Color=\"{{StaticResource {colour.Name}}}\" />");
            }

            writer.WriteLine("</ResourceDictionary>");
        }

        public void Read(FileInfo file)
        {
            this.Palette = this.repository.Value.Read(file);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.disposable.Dispose();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
