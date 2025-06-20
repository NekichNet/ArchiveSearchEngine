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

namespace ArchiveSearchEngine.IntertnalPages.NonUserDirectory.NonUserDirectoryPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeUserProperties.xaml
    /// </summary>
    public partial class ChangeNonUserProperties : Window
    {
        public NonUserChanger owner_;
        public ChangeNonUserProperties(NonUserChanger owner, NonUser user)
        {
            InitializeComponent();
            FullnameChange.Text = user.Fullname;
            PostChange.Text = user.Post;
            StructDivisionChange.Text = user.StructDivision;
            owner_ = owner;
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            owner_.ChangeUser(FullnameChange.Text, PostChange.Text, StructDivisionChange.Text);
            
            this.Close();
        }
    }
}
