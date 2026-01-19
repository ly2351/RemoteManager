using RemoteManager.ViewModels;
using RemoteManager.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RemoteManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm &&
                vm.SelectedServer != null &&
                vm.ConnectCommand.CanExecute(vm.SelectedServer))
            {
                vm.ConnectCommand.Execute(vm.SelectedServer);
            }
        }
        private void OpenJson_Click(object sender, RoutedEventArgs e)
        {
            var win = new JsonEditorWindow
            {
                Owner = this
            };

            if (win.ShowDialog() == true &&
                DataContext is MainViewModel vm)
            {
                vm.Reload();
            }
        }
    }
}