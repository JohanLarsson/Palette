namespace Palette
{
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        private readonly ViewModel viewModel = new ViewModel();
        private FileInfo file;
        private static readonly string Filter = "Palettes (*.palette)|*.palette|All files (*.*)|*.*";

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.viewModel;
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

            this.file = new FileInfo(dialog.FileName);

            this.viewModel.Save(this.file);
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
