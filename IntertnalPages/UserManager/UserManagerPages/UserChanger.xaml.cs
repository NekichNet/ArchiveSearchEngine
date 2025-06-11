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
            NameDisplay.Text = userTable_.GetUser(index_).Fullname;
            LoginDisplay.Text = userTable_.GetUser(index_).Username;
            PostDisplay.Text = userTable_.GetUser(index_).Post;
            StructDivisionDisplay.Text = userTable_.GetUser(index_).StructDivision;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            owner_.ToSearchUsers();
        }

       
        public void ChangeUser(string name, string post, string structDivision, string username, string password) {
            try
            {
                userTable_.GetUser(index_).Fullname = name;
                userTable_.GetUser(index_).Post = post;
                userTable_.GetUser(index_).StructDivision = structDivision;
                userTable_.GetUser(index_).Username = username;
                //userTable_.GetUser(index_).password = password;
                RefreshInfo();
            }
            catch { }
        }

    
        private void ChangeNamePostEtc_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChangeUserProperties(this, userTable_.GetUser(index_));
            window.ShowDialog();
        }
    }
}
