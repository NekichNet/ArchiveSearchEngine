using ArchiveSearchEngine.Model.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        }

        private void InventGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Файл формата (*.docx)| *.docx";
            openFileDialog.DefaultDirectory = Directory.GetCurrentDirectory();
            openFileDialog.ShowDialog();
            documentTable_.ExportToWord(openFileDialog.FileName, InventoryNumberGUI.Text, TermGUI.Text, 
                YearPickerGUI.Text, Int32.Parse(FromGUI.Text), Int32.Parse(UpToGUI.Text));
            MessageBox.Show($"Опись сгенерирована по пути: {openFileDialog.FileName}");
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string tempText = e.Text;
            e.Handled = !(int.TryParse(e.Text, out _) && e.Text.Replace(" ", "") == tempText);
        }

        private void Number_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space || ((e.Key == Key.Delete || e.Key == Key.Back) && (sender as TextBox).Text.Length == 1);
        }

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = Convert.ToInt32((sender as TextBox).Text) < 0 || Convert.ToInt32((sender as TextBox).Text) > 9999;
        }
    }
}
