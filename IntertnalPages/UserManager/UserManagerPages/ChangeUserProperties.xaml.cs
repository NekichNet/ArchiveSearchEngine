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
        UserChanger owner_;
        public ChangeUserProperties(UserChanger owner, User user)
        {
            InitializeComponent();
            FullnameChange.Text = user.Fullname;
            PostChange.Text = user.Post;
            StructDivisionChange.Text = user.StructDivision;
            LoginChange.Text = user.Username;
            owner_ = owner;
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            owner_.ChangeUser(FullnameChange.Text, PostChange.Text, StructDivisionChange.Text, LoginChange.Text, PasswordChange.Text);
            this.Close();
        }
    }
}
