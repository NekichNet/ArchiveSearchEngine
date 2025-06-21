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

namespace ArchiveSearchEngine.IntertnalPages
{
    /// <summary>
    /// Логика взаимодействия для UserAccountPage.xaml
    /// </summary>
    public partial class UserAccountPage : Page
    {
        MainSpace owner_;
        public UserAccountPage(MainSpace owner, User LoggedUser)
        {
            InitializeComponent();
            this.owner_ = owner;
            FullnameDisplay.Text = LoggedUser.Fullname;
            UserNameDisplay.Text = LoggedUser.Username;
            PostDisplay.Text = LoggedUser.Post;
            StructDivisionDisplay.Text = LoggedUser.StructDivision;
            if (LoggedUser.IsAdmin)
            {
                isAdminDisplay.Text = "Администратор";
            }
            else
            {
                isAdminDisplay.Text = "Пользователь";
            }
            
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            owner_.Owner.ToSignIn();
        }
    }
}
