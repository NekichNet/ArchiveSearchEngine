using ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages;
using ArchiveSearchEngine.Model.Database;
using ArchiveSearchEngine.ViewModel;
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

namespace ArchiveSearchEngine.IntertnalPages.NonUserDirectory
{
    /// <summary>
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class NonUserDirectory : Page
    {
        List<UserSpace> pages = new List<UserSpace>();
        public MainSpace owner_;
        NonUserTable nonUserTable_;
        UserTable userTable_;
        public NonUserDirectory(MainSpace owner, NonUserTable nonUserTable, UserTable userTable)
        {
            InitializeComponent();
            pages.Add(new UserSpace("Поисковик", new NonUserDirectoryPages.NonUserFinder(this, nonUserTable)));
            UserListManager.Navigate(pages[0].Page);
            owner_ = owner;
            nonUserTable_ = nonUserTable;
            userTable_ = userTable;
        }
        public void ToSearchUsers()
        {
            UserListManager.Navigate(pages[0].Page);
        }
        public void ToChangeUsers(int index)
        {
            try
            {
                pages.RemoveAt(1);
            }
            catch { }
            pages.Add(new UserSpace("Изменятель", new NonUserDirectoryPages.NonUserChanger(this, nonUserTable_, index, userTable_)));
            UserListManager.Navigate(pages[1].Page);
        }


        
    }
}
