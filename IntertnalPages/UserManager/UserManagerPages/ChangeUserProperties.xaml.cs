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
using System.Windows.Shapes;

namespace ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeUserProperties.xaml
    /// </summary>
    public partial class ChangeUserProperties : Window
    {
        User user_;
        public UserChanger owner_;
        bool UserIsAdmin;
        public ChangeUserProperties(UserChanger owner, User user)
        {
            InitializeComponent();
            FullnameChange.Text = user.Fullname;
            PostChange.Text = user.Post;
            StructDivisionChange.Text = user.StructDivision;
            LoginChange.Text = user.Username;
            ChangeIsAdmin.IsChecked = user.IsAdmin;
            UserIsAdmin = user.IsAdmin;
            owner_ = owner;
            if(owner.owner_.owner_.Owner.LoggedUser.Username == user.Username)
            {
                ShowChangeAdmin.Visibility = Visibility.Hidden;
            }
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            owner_.ChangeUser(FullnameChange.Text, PostChange.Text, StructDivisionChange.Text, LoginChange.Text, PasswordChange.Password, UserIsAdmin);
            
            this.Close();
        }

        private void ChangeIsAdmin_Checked(object sender, RoutedEventArgs e)
        {
            UserIsAdmin = true;
        }

        private void ChangeIsAdmin_Unchecked(object sender, RoutedEventArgs e)
        {
            UserIsAdmin = false;
        }
    }
}
