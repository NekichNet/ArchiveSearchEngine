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

namespace ArchiveSearchEngine.IntertnalPages.NonUserDirectory.NonUserDirectoryPages
{
    /// <summary>
    /// Логика взаимодействия для UserFinder.xaml
    /// </summary>
    public partial class NonUserFinder : Page
    {
        NonUserDirectory owner_;
        NonUserTable nonUserTable_;
        

        public NonUserFinder(NonUserDirectory owner, NonUserTable nonUserTable)
        {
            InitializeComponent();
            owner_ = owner;
            nonUserTable_ = nonUserTable;
            UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits();

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits();
                    UsersFoundDisplay.Items.Refresh();
                }
            };
        }
        

        private void SelectUserToChange(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                owner_.ToChangeUsers(UsersFoundDisplay.SelectedIndex);
            }
            catch (Exception ex) { }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PromptLine.Text.Trim().Length == 0)
            {
                UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits();
                UsersFoundDisplay.Items.Refresh();
            }
            else
            {
                UsersFoundDisplay.ItemsSource = nonUserTable_.GetUnits(PromptLine.Text);
                UsersFoundDisplay.Items.Refresh();
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddNewNonUser(this, nonUserTable_);
            addUserWindow.ShowDialog();
        }
    }
}
