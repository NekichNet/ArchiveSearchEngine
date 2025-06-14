using ArchiveSearchEngine.Database;
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
    /// Логика взаимодействия для EntrySpace.xaml
    /// </summary>
    public partial class EntrySpace : Page
    {
        MainWindow _owner;
        public EntrySpace(MainWindow owner, UserTable userTable)
        {
            InitializeComponent();

            _owner = owner;
            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    ReOpen();
                }
            };
            LoginSearchGUI.ItemsSource = userTable.GetUsers().Select(x=>x.Username);
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            _owner.ToSignUp();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                _owner.TrySigningIn(LoginSearchGUI.Text, PasswordGUI.Password);
            }
            catch (Exception ex)
            {
                ErrorOut(ex.Message);
            }
        }
        private void ErrorOut(string message)
        {
            ErrorGui.Text = message;
        }
        private void ReOpen() {
            LoginSearchGUI.Text = "";
            ErrorGui.Text = "";
        }
    }
}
