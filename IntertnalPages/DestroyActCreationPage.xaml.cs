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
    /// Логика взаимодействия для DestroyActCreationPage.xaml
    /// </summary>
    public partial class DestroyActCreationPage : Page
    {
        MainSpace _owner;
        public DestroyActCreationPage(MainSpace owner, DocumentTable documentTable)
        {
            InitializeComponent();
            _owner = owner;
            TermGUI.ItemsSource = new List<string> { "Дела временного хранения", "Дела долговременного хранения", "Дела по личному составу", "Дела постоянного хранения" };
            TermGUI.SelectedIndex = 0;
        }

        private void InventGenerationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryNumberGUI.Text = string.Empty;
            StructDivisionGUI.Text = string.Empty;
            TermGUI.SelectedIndex = 0;
            YearPickerGUI.Text = string.Empty;
        }

        private void YearPickerGUI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space || ((e.Key == Key.Delete || e.Key == Key.Back) && (sender as TextBox).Text.Length == 1);

        }

        private void YearPickerGUI_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string tempText = e.Text;
            e.Handled = !(int.TryParse(e.Text, out _) && e.Text.Replace(" ", "") == tempText);
        }

        private void YearPickerGUI_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = Convert.ToInt32((sender as TextBox).Text) < 0 || Convert.ToInt32((sender as TextBox).Text) > 9999;
        }
    }
}
