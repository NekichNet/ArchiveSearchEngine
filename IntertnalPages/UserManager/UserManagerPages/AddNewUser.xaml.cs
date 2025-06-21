using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.IntertnalPages.NonUserDirectory.NonUserDirectoryPages;
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
    /// Логика взаимодействия для AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {
        UserTable userTable_;
        UserFinder owner_;
        bool isAdmin = false;
        NonUserTable _nonUserTable;
        NonUser _nonUser;
        public AddNewUser(UserFinder owner, UserTable userTable)
        {
            InitializeComponent();
            this.userTable_ = userTable;
            owner_ = owner;
        }

        public AddNewUser(UserTable userTable, NonUser nonUser, NonUserTable nonUserTable)
        {
            InitializeComponent();
            this.userTable_ = userTable;
            FullnameChange.Text = nonUser.Fullname;
            PostChange.Text = nonUser.Post;
            StructDivisionChange.Text = nonUser.StructDivision;
            _nonUserTable = nonUserTable;
            _nonUser = nonUser;
            ShowDeleteOnMake_GUI.Visibility = Visibility.Collapsed;
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginChange.Text.Trim().Length > 0)
            {
                if (!userTable_.UserExists(LoginChange.Text))
                {
                    if(PasswordChange.Password.Trim().Length >= 5)
                    {

                        try
                        {
                            User user = new User(LoginChange.Text, FullnameChange.Text, PostChange.Text, StructDivisionChange.Text, isAdmin);

                            userTable_.NewUser(user, PasswordChange.Password);
                            if (AddUserOnMaking_GUI.IsChecked == true)
                            {
                                _nonUserTable.NewUnit(user.Fullname, user.Post, user.StructDivision);
                            }
                            if (owner_ != null)
                            {
                                owner_.UsersFoundDisplay.ItemsSource = userTable_.GetUsers();
                                owner_.UsersFoundDisplay.Items.Refresh();
                                MessageBox.Show("Пользователь был успешно добавлен");
                            }
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Произошла непредвиденененененная ошибка");
                            this.Close();
                        }
                    }
                    else
                    {

                        MessageBox.Show("Поле \"Пароль\" должно содержать хотя бы пять символов");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                }
            }
            else
            {
                MessageBox.Show("Ошибка: поле \"Логин\" должно содержать хотя бы один символ");
            }
            
            
            
        }

    }
}
