using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.IntertnalPages;
using ArchiveSearchEngine.IntertnalPages.NonUserDirectory;
using ArchiveSearchEngine.IntertnalPages.UserManager;
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

        public List<UserSpace> Spaces = new List<UserSpace>();

        UserTable userTable_;

        public MainSpace(MainWindow owner, UserTable userTable, DocumentTable documentTable, NonUserTable nonUserTable)
        {
            InitializeComponent();
            _owner = owner;

            AddDocs addDocPage = new AddDocs(this, documentTable, userTable, nonUserTable);

            Spaces.Add(new UserSpace("Электронный реестр", new DocRegistry(this, documentTable, userTable)));
            Spaces.Add(new UserSpace("Добавление документа", addDocPage));
            Spaces.Add(new UserSpace("Генерация описи", new InventoryGeneration(this, userTable, nonUserTable, documentTable)));
            Spaces.Add(new UserSpace("Генерация акта об уничтожении", new DestroyActCreationPage(this, documentTable)));

            string method() { 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Файл формата (*.csv)| *.csv";
                openFileDialog.DefaultDirectory = Directory.GetCurrentDirectory();
                openFileDialog.ShowDialog();
                nonUserTable.ImportFromCSV(openFileDialog.FileName);

                addDocPage.RefreshFullnameSearchGUI();
                return openFileDialog.FileName;
            }
            Spaces.Add(new UserSpace("Импорт сотрудников (*.csv)", method));


            string method1()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Таблица формата (*.xlsx)| *.xlsx";
                openFileDialog.DefaultDirectory = Directory.GetCurrentDirectory();
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            }
            Spaces.Add(new UserSpace("Импорт документов из excel (*.xlsx)", method1));


            Spaces.Add(new UserSpace("Справочник сотрудников", new NonUserDirectory(this, nonUserTable, userTable)));


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
                if (Spaces[SpacesListBox.SelectedIndex].Page != null)
                {
                    DisplayFrame.Navigate(Spaces[SpacesListBox.SelectedIndex].Page);
                }
                else {
                    Spaces[SpacesListBox.SelectedIndex].Invoke();
                    SpacesListBox.SelectedIndex = -1;
                }
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
        public void CreateDocByPreset(Document doc) {
            var CreationPage = Spaces.Select(x=>x.Page).OfType<AddDocs>().FirstOrDefault();
            if (CreationPage != null) {
                DisplayFrame.Navigate(Spaces[1].Page);
                CreationPage.SetPresetValues(doc);
                SpacesListBox.SelectedIndex = 1;
            }
        }


    }
}
