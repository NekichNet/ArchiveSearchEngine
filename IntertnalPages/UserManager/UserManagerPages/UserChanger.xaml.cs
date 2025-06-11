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
using System.Xml.Linq;

namespace ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages
{
    /// <summary>
    /// Логика взаимодействия для UserChanger.xaml
    /// </summary>
    public partial class UserChanger : Page
    {
        UserManager owner_;
        UserTable userTable_;
        int index_;
        public UserChanger(UserManager owner, UserTable userTable, int index)
        {
            InitializeComponent();
            owner_ = owner;
            userTable_ = userTable;
            index_ = index;
            RefreshInfo();
        }


        public void RefreshInfo()
        {
            var user = userTable_.GetUsers()[index_];
            NameDisplay.Text = userTable_.GetUser(user.Username).Fullname;
            LoginDisplay.Text = userTable_.GetUser(user.Username).Username;
            PostDisplay.Text = userTable_.GetUser(user.Username).Post;
            StructDivisionDisplay.Text = userTable_.GetUser(user.Username).StructDivision;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            owner_.ToSearchUsers();
        }

       
        public void ChangeUser(string name, string post, string structDivision, string username, string password) {
                var user = userTable_.GetUsers()[index_];
                user.Username = username;
                user.Fullname = name;
                user.Post = post;
                user.StructDivision = structDivision;

                if (password.Trim().Count() > 0) {
                    userTable_.UpdateUser(user, password);
                }
                else
                {
                    userTable_.UpdateUser(user);
                }

                RefreshInfo();
        }

    
        private void ChangeNamePostEtc_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChangeUserProperties(this, userTable_.GetUser(userTable_.GetUsers()[index_].Username));
            window.ShowDialog();
        }
    }
}
