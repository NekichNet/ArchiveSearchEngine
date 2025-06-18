using ArchiveSearchEngine.IntertnalPages;
using ArchiveSearchEngine.IntertnalPages.NonUserDirectory;
using ArchiveSearchEngine.IntertnalPages.UserManager;
using ArchiveSearchEngine.Model;
using ArchiveSearchEngine.Model.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArchiveSearchEngine.ViewModel
{
    public class MainSpaceViewModel
    {
        public List<UserSpace> Spaces = new List<UserSpace>();
        private UserTable userTable_;
        private MainSpace view_;

        public MainSpaceViewModel(MainSpace view, UserTable userTable, DocumentTable _documentTable, NonUserTable nonUserTable)
        {
            view_ = view;

            this.userTable_ = userTable;

            Spaces.Add(new UserSpace("Электронный реестр", new DocRegistry(view_, _documentTable, userTable)));
            Spaces.Add(new UserSpace("Добавление документа", new AddDocs(view_, _documentTable, userTable)));
            Spaces.Add(new UserSpace("Генерация описи", new InventoryGeneration(view_, userTable, nonUserTable, _documentTable)));

            string method()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Файл формата (*.csv)| *.csv";
                openFileDialog.DefaultDirectory = Directory.GetCurrentDirectory();
                openFileDialog.ShowDialog();
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


            Spaces.Add(new UserSpace("Справочник сотрудников", new NonUserDirectory(view_, nonUserTable, userTable)));
        }

        public void SpacesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Spaces[view_.SpacesListBox.SelectedIndex].Page != null)
                {
                    view_.DisplayFrame.Navigate(Spaces[view_.SpacesListBox.SelectedIndex].Page);
                }
                else
                {
                    Spaces[view_.SpacesListBox.SelectedIndex].Invoke();
                    view_.SpacesListBox.SelectedIndex = -1;
                }
            }
            catch { }
        }

        public void RefreshUser()
        {
            if (Spaces.Find(x => x.Title == "Аккаунт") != null)
            {
                Spaces.RemoveAt(Spaces.Count - 1);
            }

            if (Spaces.Find(x => x.Title == "Управление пользователями") != null)
            {
                Spaces.RemoveAt(Spaces.Count - 1);
            }

            try
            {
                if (view_.Owner.LoggedUser.IsAdmin)
                {
                    Spaces.Add(new UserSpace("Управление пользователями", new UserManager(view_., userTable_)));
                }
            }
            catch { }
            try
            {
                Spaces.Add(new UserSpace("Аккаунт", new UserAccountPage(view_., view_.Owner.LoggedUser)));
            }
            catch { }
            view_.SpacesListBox.Items.Refresh();
            view_.DisplayFrame.Navigate(Spaces[0].Page);

        }
        public void CreateDocByPreset(Document doc)
        {
            var CreationPage = Spaces.Select(x => x.Page).OfType<AddDocs>().FirstOrDefault();
            if (CreationPage != null)
            {
                view_.DisplayFrame.Navigate(Spaces[1].Page);
                CreationPage.SetPresetValues(doc);
                view_.SpacesListBox.SelectedIndex = 1;
            }
        }
    }
}
