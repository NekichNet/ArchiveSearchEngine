using ArchiveSearchEngine.Database;
using ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages;
using Microsoft.Win32;
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

namespace ArchiveSearchEngine.IntertnalPages.NonUserDirectory.NonUserDirectoryPages
{
    /// <summary>
    /// Логика взаимодействия для UserChanger.xaml
    /// </summary>
    public partial class NonUserChanger : Page
    {
        public NonUserDirectory owner_;
        NonUserTable nonUserTable_;
        int index_;
        UserTable userTable_;
        public NonUserChanger(NonUserDirectory owner, NonUserTable nonUserTable, int index, UserTable userTable)
        {
            InitializeComponent();
            owner_ = owner;
            nonUserTable_ = nonUserTable;
            index_ = index;
            userTable_ = userTable;
            RefreshInfo();
        }


        public void RefreshInfo()
        {
            var user = nonUserTable_.GetUnits()[index_];
            NameDisplay.Text = nonUserTable_.GetUnit(user.Id).Fullname;
            PostDisplay.Text = nonUserTable_.GetUnit(user.Id).Post;
            StructDivisionDisplay.Text = nonUserTable_.GetUnit(user.Id).StructDivision;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            owner_.ToSearchUsers();
        }

       
        public void ChangeUser(string name, string post, string structDivision) {
            var user = nonUserTable_.GetUnits()[index_];
            user.Fullname = name;
            user.Post = post;
            user.StructDivision = structDivision;

            nonUserTable_.UpdateUnit(user);
            RefreshInfo();
        }

    
        private void ChangeNamePostEtc_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChangeNonUserProperties(this, nonUserTable_.GetUnit(nonUserTable_.GetUnits()[index_].Id));
            window.ShowDialog();
        }
        public void DeleteUserAfterRegistration()
        {
            nonUserTable_.DeleteUnit(nonUserTable_.GetUnits()[index_].Id);
            owner_.ToSearchUsers();
        }
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult notifWindow = MessageBox.Show("Вы собираетесь удалить этого человека из справочника,\nподтвердить удаление?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (notifWindow == MessageBoxResult.Yes)
            {
                nonUserTable_.DeleteUnit(nonUserTable_.GetUnits()[index_].Id);
                owner_.ToSearchUsers();
            }
        }

        private void MakeAnAccount_Click(object sender, RoutedEventArgs e)
        {
            var createAUser = new AddNewUser(userTable_, nonUserTable_.GetUnits()[index_], nonUserTable_);
            createAUser.ShowDialog();
        }
    }
}
