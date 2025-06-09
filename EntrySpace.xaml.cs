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
        public EntrySpace(MainWindow owner)
        {
            InitializeComponent();

            _owner = owner;
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            _owner.ToSignUp();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                _owner.TrySigningIn(LoginGUI.Text, PasswordGUI.Password);
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
    }
}
