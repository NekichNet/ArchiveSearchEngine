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
using System.Windows.Shapes;

namespace ArchiveSearchEngine.IntertnalPages
{
    /// <summary>
    /// Логика взаимодействия для UserPreview.xaml
    /// </summary>
    public partial class UserPreview : Window
    {
        public UserPreview(string username, UserTable userTable)
        {
            InitializeComponent();
            User user = userTable.GetUser(username);
            ShowUser(user);

        }
        public UserPreview(User user)
        {
            InitializeComponent();
            ShowUser(user);
        }
        public void ShowUser(User user)
        {

            FullnameDisplay.Text = user.Fullname;
            UsernameDisplay.Text = user.Username;
            PostDisplay.Text = user.Post;
            StructDivisionDisplay.Text = user.StructDivision;
            if (!user.IsAdmin)
            {
                UserIsAdminDisplay.Text = "Аккаунт пользователя";
            }
        }

        private void ButtonCloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
