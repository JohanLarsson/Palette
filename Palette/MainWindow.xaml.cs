namespace Palette
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        private readonly ViewModel viewModel = new ViewModel();
        private FileInfo file;
        private static readonly string Filter = "Palettes (*.palette)|*.palette|ResourceDictionary (*.xaml)|*.xaml|All files (*.*)|*.*";

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.viewModel;
        }

        private void OnCanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.viewModel.Palette?.Colours?.Any() == true;
            e.Handled = true;
        }

        private void OnNew(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.Palette = new PaletteInfo();
            e.Handled = true;
        }

        private void OnCanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.viewModel.CanSave(this.file);
            e.Handled = true;
        }

        private void OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.file == null)
            {
                var dialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog
                {
                    Filter = Filter
                };

                if (dialog.ShowDialog(this) != true)
                {
                    return;
                }

                this.file = new FileInfo(dialog.FileName);
            }

            this.viewModel.Save(this.file);
            e.Handled = true;
        }

        private void OnSaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog
            {
                Filter = Filter
            };

            if (dialog.ShowDialog(this) != true)
            {
                return;
            }

            var fileInfo = new FileInfo(dialog.FileName);
            if (this.file == null)
            {
                this.file = fileInfo;
            }

            this.viewModel.Save(fileInfo);
            e.Handled = true;
        }

        private void OnOpen(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog
            {
                Filter = Filter
            };

            if (dialog.ShowDialog(this) == true)
            {
                this.file = new FileInfo(dialog.FileName);
                if (string.Equals(this.file.Extension, ".xaml"))
                {
                    throw new NotSupportedException("Reading xaml files is not yet supported");
                }

                this.viewModel.Read(this.file);
            }
        }

        private void OnCanSaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.viewModel.Palette != null;
            e.Handled = true;
        }
    }
}
