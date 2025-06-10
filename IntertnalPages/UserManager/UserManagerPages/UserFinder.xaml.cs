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
        public UserFinder(UserManager owner)
        {
            InitializeComponent();
            owner_ = owner;
        }

        private void SelectUserToChange(object sender, MouseButtonEventArgs e)
        {
            try
            {
                owner_.ToChangeUsers();
            }
        }
    }
}
