using ArchiveSearchEngine.IntertnalPages;
using ArchiveSearchEngine.IntertnalPages.NonUserDirectory;
using ArchiveSearchEngine.IntertnalPages.UserManager;
using ArchiveSearchEngine.Model;
using ArchiveSearchEngine.Model.Database;
using ArchiveSearchEngine.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для MainSpace.xaml
    /// </summary>
    public partial class MainSpace : Page
    {
        private MainWindow _owner;
        public MainWindow Owner { get { return _owner; } }

        private MainSpaceViewModel _viewModel;

        public MainSpace(MainWindow owner, UserTable userTable, DocumentTable documentTable, NonUserTable nonUserTable)
        {
            InitializeComponent();
            _owner = owner;

            _viewModel = new MainSpaceViewModel(this, userTable, documentTable, nonUserTable);

            SpacesListBox.ItemsSource = _viewModel.Spaces;

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    _viewModel.RefreshUser();
                }
                else
                {
                    _viewModel.RefreshUser();
                }
            };
        }
    }
}
