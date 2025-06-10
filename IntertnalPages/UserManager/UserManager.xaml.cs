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

namespace ArchiveSearchEngine.IntertnalPages.UserManager
{
    /// <summary>
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class UserManager : Page
    {
        List<UserSpace> pages = new List<UserSpace>();
        MainSpace owner_;
        public UserManager(MainSpace owner)
        {
            InitializeComponent();
            pages.Add(new UserSpace("Поисковик", new UserManagerPages.UserFinder(this)));
            UserListManager.Navigate(pages[0]);
            owner_ = owner;
        }
        public void ToSearchUsers()
        {
            UserListManager.Navigate(pages[0]);
        }
        public void ToChangeUsers()
        {
            if (pages.Find(x => x.Title.Equals("Именятель")) != null)
            {
                pages.Add(new UserSpace("Именятель", new UserManagerPages.UserFinder(this)));
            }
            else
            {
                pages.RemoveAt(pages.Count - 1);
                pages.Add(new UserSpace("Именятель", new UserManagerPages.UserFinder(this)));
            }
            UserListManager.Navigate(pages[1]);
        }
    }
}
