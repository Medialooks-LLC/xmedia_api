using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace OpenGLAvalonia
{
    public partial class ConversionSettingsDialog : Window
    {
        public string? ResultJson { get; private set; }
        public ConversionSettingsDialog()
        {
            InitializeComponent();
        }

        private void OnOkClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Получаем JSON из контрола
            var viewModel = SettingsControl.DataContext as ConversionSettingsViewModel;
            ResultJson = viewModel?.GenerateJson();
            Close(true);
        }

        private void OnCancelClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ResultJson = null;
            Close(false);
        }

        // Асинхронный метод для показа диалога
        public static async Task<string?> ShowDialog(Window owner, string? initialJson = null, bool isPlaying = false)
        {
            var dialog = new ConversionSettingsDialog();
            var viewModel = string.IsNullOrEmpty(initialJson)
                ? new ConversionSettingsViewModel()
                : ConversionSettingsViewModel.FromJson(initialJson);

            viewModel.IsPlaying = isPlaying;
            dialog.SettingsControl.DataContext = viewModel;
            var result = await dialog.ShowDialog<bool>(owner);

            return result ? dialog.ResultJson : null;
        }
    }
}