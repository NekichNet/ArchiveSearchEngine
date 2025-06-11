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
    /// Логика взаимодействия для RegistrationSpace.xaml
    /// </summary>
    public partial class RegistrationSpace : Page
    {
        MainWindow _owner;
        public RegistrationSpace(MainWindow owner)
        {
            _owner = owner;
            InitializeComponent();
        }

        private void BackToSignIn(object sender, RoutedEventArgs e)
        {

            _owner.ToSignIn();
        }

        private void SignUpANewUser(object sender, RoutedEventArgs e)
        {
            try
            {
                _owner.TrySignUp(LoginGUI.Text, NameGui.Text, PostGUI.Text, 
                    StructDivisionGUI.Text, PasswordGUI.Password, PasswordRepeatGUI.Password);
                LoginGUI.Text = "";
                NameGui.Text = "";
                PostGUI.Text = "";
                StructDivisionGUI.Text = "";

            }
            catch (Exception ex) 
            {
                ErrorOut(ex.Message);
            }
        }

        private void ErrorOut(string message) {
            ErrorGui.Text = message;
        }
    }
}
