using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OpenGLAvalonia.Components
{
    public partial class PlaylistTable : UserControl
    {
        private PlaylistTableViewModel? ViewModel => DataContext as PlaylistTableViewModel;

        public PlaylistTable()
        {
            InitializeComponent();
            Debug.WriteLine("PlaylistTable constructor called");

            var viewModel = new PlaylistTableViewModel();
            DataContext = viewModel;
        }

        private bool _isReadOnly = false;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                PlaylistDataGrid.IsReadOnly = value;
                AddFileButton.IsEnabled = !value;
                DelFileButton.IsEnabled = !value;
                MoveUpButton.IsEnabled = !value;
                MoveDownButton.IsEnabled = !value;
                SetBackgroundButton.IsEnabled = !value;
            }
        }

        private async void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var file = await OpenFilePickerAsync();
            if (file == null) return;

            var viewModel = ViewModel;
            if (viewModel == null) return;

            viewModel.PlaylistItems.Add(new PlaylistItem
            {
                Pos = viewModel.PlaylistItems.Count,
                OpenUrl = file.Path.AbsolutePath,
                FixedPosSec = null,
                InSec = null,
                OutSec = null,
            });
        }

        private void AddUrl_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddLive_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddRef_Click(object sender, RoutedEventArgs e)
        {
        }
        private void AddCmd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;
            if (viewModel?.PlaylistItems == null) return;

            int index = PlaylistDataGrid.SelectedIndex;
            if (index >= 0 && index < viewModel.PlaylistItems.Count)
            {
                viewModel.PlaylistItems.RemoveAt(index);
            }
        }

        private void MoveItem(int direction)
        {
            var viewModel = ViewModel;
            if (viewModel?.PlaylistItems == null) return;

            int index = PlaylistDataGrid.SelectedIndex;
            if (index < 0 || index >= viewModel.PlaylistItems.Count) return;

            int newIndex = index + direction;
            if (newIndex < 0 || newIndex >= viewModel.PlaylistItems.Count) return;

            var item = viewModel.PlaylistItems[index];
            viewModel.PlaylistItems.RemoveAt(index);
            viewModel.PlaylistItems.Insert(newIndex, item);
            PlaylistDataGrid.SelectedIndex = newIndex;
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) => MoveItem(-1);
        private void MoveDown_Click(object sender, RoutedEventArgs e) => MoveItem(1);

        private async void SetBackground_Click(object sender, RoutedEventArgs e)
        {
            var file = await OpenFilePickerAsync();
            if (file == null) return;

            var viewModel = DataContext as PlaylistTableViewModel;
            if (viewModel != null)
                viewModel.PlaylistBackground = file.Path.AbsolutePath;
        }

        private async Task<IStorageFile?> OpenFilePickerAsync()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return null;

            var mediaType = new FilePickerFileType("Media")
            {
                Patterns = ["*.*"]
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Media File",
                AllowMultiple = false,
                FileTypeFilter = [mediaType]
            });

            return files.Count > 0 ? files[0] : null;
        }
    }
}