using ArchiveSearchEngine.IntertnalPages.UserManager.UserManagerPages;
using ArchiveSearchEngine.Model.Database;
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
    /// Логика взаимодействия для AddNewUser.xaml
    /// </summary>
    public partial class AddNewNonUser : Window
    {
        NonUserTable nonUserTable_;
        NonUserFinder owner_;
        public AddNewNonUser(NonUserFinder owner, NonUserTable nonUserTable)
        {
            InitializeComponent();
            this.nonUserTable_ = nonUserTable;
            owner_ = owner;
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {

        try
        {
        nonUserTable_.NewUnit(FullnameChange.Text, PostChange.Text, StructDivisionChange.Text);
        owner_.UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits();
        owner_.UsersFoundDisplay.Items.Refresh();
        MessageBox.Show("Человек был успешно добавлен");
        this.Close();
        }
        catch
        {
        owner_.UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits();
        owner_.UsersFoundDisplay.Items.Refresh();
        MessageBox.Show("Произошла непредвиденененененная ошибка");
        this.Close();
        }
                    

        }
    }
}
