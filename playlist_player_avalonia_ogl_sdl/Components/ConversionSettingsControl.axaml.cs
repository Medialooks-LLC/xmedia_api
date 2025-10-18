using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace OpenGLAvalonia.Components
{
    public partial class ConversionSettingsControl : UserControl
    {
        private ConversionSettingsViewModel _viewModel = new();

        public ConversionSettingsControl()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void OnGenerateJsonClicked(object? sender, RoutedEventArgs e)
        {
            JsonOutput.Text = _viewModel.GenerateJson();
        }
    }
}