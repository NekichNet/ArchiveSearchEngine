using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.ViewModel;
using Microsoft.Data.Sqlite;
using System.IO;
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

namespace ArchiveSearchEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel;

        public User LoggedUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel(this);
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы собираетесь закрыть программу", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
    }
}