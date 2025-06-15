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

namespace ArchiveSearchEngine.IntertnalPages
{
    /// <summary>
    /// Логика взаимодействия для InventoryGeneration.xaml
    /// </summary>
    public partial class InventoryGeneration : Page
    {
        MainSpace owner_;
        UserTable userTable_;
        NonUserTable nonUserTable_;
        DocumentTable documentTable_;
        public InventoryGeneration(MainSpace owner, UserTable userTable, NonUserTable nonUserTable, DocumentTable documentTable)
        {
            InitializeComponent();

            TermGUI.ItemsSource = new List<string> { "Дела временного хранения", "Дела долговременного хранения", "Дела по личному составу", "Дела постоянного хранения" };
            TermGUI.SelectedIndex = 0;

            this.owner_ = owner;
            this.userTable_ = userTable;
            this.nonUserTable_ = nonUserTable;
            this.documentTable_ = documentTable;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryNumberGUI.Text = "";
            TermGUI.SelectedIndex = 0;
            YearPickerGUI.Text = "";
            UserPickerGUI.Text = "";
        }

        private void InventGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Генерация описи");
        }
    }
}
