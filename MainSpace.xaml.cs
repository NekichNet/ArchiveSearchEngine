using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.IntertnalPages;
using ArchiveSearchEngine.IntertnalPages.NonUserDirectory;
using ArchiveSearchEngine.IntertnalPages.UserManager;
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
        MainWindow _owner;
        public MainWindow Owner { get { return _owner; } }

        public MainSpaceViewModel ViewModel;

        public MainSpace(MainWindow owner, UserTable userTable, DocumentTable documentTable, NonUserTable nonUserTable)
        {
            InitializeComponent();
            _owner = owner;

            ViewModel = new MainSpaceViewModel(userTable, documentTable, nonUserTable, this);

            SpacesListBox.ItemsSource = ViewModel.Spaces;
            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    ViewModel.RefreshUser();
                }
                else
                {
                    ViewModel.RefreshUser();
                }
            };
        }

        public void SpacesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.Spaces[this.SpacesListBox.SelectedIndex].Page != null)
                {
                    this.DisplayFrame.Navigate(ViewModel.Spaces[this.SpacesListBox.SelectedIndex].Page);
                }
                else
                {
                    ViewModel.Spaces[this.SpacesListBox.SelectedIndex].Invoke();
                    this.SpacesListBox.SelectedIndex = -1;
                }
            }
            catch { }
        }
    }
}
