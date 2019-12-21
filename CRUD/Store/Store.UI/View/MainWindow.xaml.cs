using System.Windows;

namespace Store.UI.View
{
    using Store.UI.ViewModel;

    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this._viewModel.Load();
        }
    }
}
