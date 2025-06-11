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

namespace ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages
{
    /// <summary>
    /// Логика взаимодействия для UserFinder.xaml
    /// </summary>
    public partial class UserFinder : Page
    {
        UserManager owner_;
        UserTable userTable_;
        

        public UserFinder(UserManager owner, UserTable userTable)
        {
            InitializeComponent();
            owner_ = owner;
            userTable_ = userTable;
            UsersFoundDisplay.ItemsSource = userTable_.GetUsers();

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    UsersFoundDisplay.ItemsSource = userTable_.GetUsers();
                    UsersFoundDisplay.Items.Refresh();
                }
            };
        }
        

        private void SelectUserToChange(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                owner_.ToChangeUsers(UsersFoundDisplay.SelectedIndex);
            }
            catch (Exception ex) { }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PromptLine.Text.Trim().Length == 0)
            {
                UsersFoundDisplay.ItemsSource = userTable_.GetUsers();
                UsersFoundDisplay.Items.Refresh();
            }
            else
            {
                UsersFoundDisplay.ItemsSource = userTable_.GetUsers(PromptLine.Text);
                UsersFoundDisplay.Items.Refresh();
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddNewUser(this, userTable_);
            addUserWindow.ShowDialog();
        }
    }
}
