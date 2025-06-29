﻿using ArchiveSearchEngine.Database;
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
        public User LoggedUser { get; set; }

        private DocumentTable _documentTable;
        private UserTable _userTable;
        private NonUserTable _nonUserTable;

        private List<string> images = new List<string>
        {
            "/sources/background.png",
            "/sources/background1.png",
            "/sources/background2.png",
        };

        public List<Page> pages = new List<Page>();
        public MainWindow()
        {
            InitializeComponent();

            // Initializing database

            bool db_exists = File.Exists("archive.db");

            SqliteConnection Connection = new SqliteConnection("Data Source=archive.db");
            Connection.Open();

            _userTable = new UserTable(Connection, !db_exists);
            _documentTable = new DocumentTable(Connection);
            _nonUserTable = new NonUserTable(Connection);

            Document.Table = _documentTable;

            pages.Add(new EntrySpace(this, _userTable));
            pages.Add(new RegistrationSpace(this));
            pages.Add(new MainSpace(this, _userTable, _documentTable, _nonUserTable));
            EntryFrame.Navigate(pages[0]);

            //_documentTable.ExportToWord("1", "дел, документов постоянного хранения", "2025", "И.В. Богородский");
        }

        public void TrySigningIn(string username, string password)
        {
            if (username.Trim().Length == 0)
            {
                throw new Exception("Поле ввода \"Имя\" пусто");
            }
            if (password.Trim().Length == 0)
            {
                throw new Exception("Поле ввода \"Пароль\" пусто");
            }

            if (_userTable.CheckUser(username, password))
            {
                LoggedUser = _userTable.GetUser(username);
                ToSystem();
            }
            else
            {
                throw new Exception("Пользователя с таким именем и паролем не существует");
            }
        }

        public void TrySignUp(string username, string fullname, string post, string struct_division, string password, string passwordRepeat, bool AddToRegistry, bool is_admin = false)
        {
            if (username.Trim().Length > 0)
            {
                if (!_userTable.UserExists(username))
                {
                    if (password.Trim().Length >= 5)
                    {
                        if (password == passwordRepeat)
                        {
                            _userTable.NewUser(new User(username, fullname, post, struct_division, is_admin), password);
                            ToSignIn();
                        }
                        else
                        {
                            throw new Exception("Пароли не совпадают");
                        }
                    }
                    else
                    {
                        throw new Exception("Поле \"Пароль\" должно содержать хотя бы пять символов");
                    }
                }
                else
                {
                    throw new Exception("Пользователь с таким именем уже существует");
                }
            }
            else
            {
                throw new Exception("Поле имени пусто");
            }
        }




        public void ToSignIn()
        {
            EntryFrame.Navigate(pages[0]);
        }
        public void ToSignUp()
        {
            EntryFrame.Navigate(pages[1]);
        }
        public void ToSystem()
        {
            EntryFrame.Navigate(pages[2]);
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