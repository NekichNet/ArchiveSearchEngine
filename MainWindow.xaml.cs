using ArchiveSearchEngine.Database;
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

        private List<User> users = new List<User>();

        public User LoggedUser { get; set; }




        private List<string> images = new List<string>
        {
            "/sources/background.png",
            "/sources/background1.png",
            "/sources/background2.png",
        };

        List<Page> pages = new List<Page>();
        public MainWindow()
        {
            InitializeComponent();

            pages.Add(new EntrySpace(this));
            pages.Add(new RegistrationSpace(this));
            pages.Add(new MainSpace(this));

            EntryFrame.Navigate(pages[0]);
            users.Add(new User("admin", "Администратор", "Администратор", "Подразделение", true));

            // Initializing database

            bool db_exists = File.Exists("archive.db");

            SqliteConnection Connection = new SqliteConnection("Data Source=archive.db;Password=htfdkshsrujdt");
            Connection.Open();

            UserTable userTable = new UserTable(Connection, !db_exists);
            DocumentTable documentTable = new DocumentTable(Connection);
            HistoryTable historyTable = new HistoryTable(Connection);
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

            if (users.Find(x => x.Username == username) != null)
            {
                var user = users.Find(x => x.Username == username);
                if () // TODO: Проверка пароля
                {
                    LoggedUser = user;
                    ToSystem();
                }
                else
                {
                    throw new Exception("Пользователя с таким именем и паролем не существует");
                }
            }
            else
            {
                throw new Exception("Пользователя с таким именем и паролем не существует");
            }
        }


        public void TrySignUp(string name, string password, string passwordRepeat)
        {
            if (name.Trim().Length > 0 && password.Trim().Length > 0)
            {
                if (users.Find(x => x.Name == name) == null)
                {
                    if (password == passwordRepeat)
                    {
                        users.Add(new User(name, password));
                        ToSignIn();
                    }
                    else
                    {
                        throw new Exception("Пароли не совпадают");
                    }
                }
                else
                {
                    throw new Exception("Пользователь с таким именем уже существует");
                }
            }
            else
            {
                throw new Exception("Поле имени/пароля пусто");
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

        



    }
}