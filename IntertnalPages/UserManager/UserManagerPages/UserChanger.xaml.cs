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
    /// Логика взаимодействия для UserChanger.xaml
    /// </summary>
    public partial class UserChanger : Page
    {
        UserManager owner_;
        public UserChanger(UserManager owner)
        {
            InitializeComponent();
            owner_ = owner;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            owner_.ToSearchUsers();
        }
    }
}
