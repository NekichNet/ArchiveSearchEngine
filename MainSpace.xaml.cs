using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.IntertnalPages;
using ArchiveSearchEngine.IntertnalPages.UserManager;
using System;
using System.Collections.Generic;
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

        public List<UserSpace> Spaces = new List<UserSpace>();

        UserTable userTable_;

        public MainSpace(MainWindow owner, UserTable userTable, DocumentTable _documentTable, HistoryTable historyTable)
        {
            InitializeComponent();
            _owner = owner;

            Spaces.Add(new UserSpace("Электронный реестр", new DocRegistry(this, _documentTable, historyTable)));
            Spaces.Add(new UserSpace("Создание документа", new AddDocs(this, _documentTable)));
            Spaces.Add(new UserSpace("Добавление документов", new DocumentCreation(this)));

            userTable_ = userTable;

            SpacesListBox.ItemsSource = Spaces;
            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    RefreshUser();
                }
                else
                {
                    RefreshUser();
                }
            };
        }

        private void SpacesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                DisplayFrame.Navigate(Spaces[SpacesListBox.SelectedIndex].Page);
            } catch { }
        }

        private void RefreshUser()
        {
            if (Spaces.Find(x=>x.Title == "Аккаунт") != null)
            {
                Spaces.RemoveAt(Spaces.Count - 1);
            }

            if (Spaces.Find(x => x.Title == "Управление пользователями") != null)
            {
                Spaces.RemoveAt(Spaces.Count - 1);
            }
            try
            {
                if (_owner.LoggedUser.IsAdmin)
                {
                    Spaces.Add(new UserSpace("Управление пользователями", new UserManager(this, userTable_)));
                }
            }
            catch { }
            try
            {
                Spaces.Add(new UserSpace("Аккаунт", new UserAccountPage(this, _owner.LoggedUser)));
            }catch { }
            SpacesListBox.Items.Refresh();
            DisplayFrame.Navigate(Spaces[0].Page);

        }
    }
}
