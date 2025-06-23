using ArchiveSearchEngine.Database;
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
    /// Логика взаимодействия для DestroyActCreationPage.xaml
    /// </summary>
    public partial class DestroyActCreationPage : Page
    {
        MainSpace _owner;
        DocumentTable _documentTable;
        public DestroyActCreationPage(MainSpace owner, DocumentTable documentTable)
        {
            InitializeComponent();
            _owner = owner;
            TermGUI.ItemsSource = new List<string> { "Дела временного хранения", "Дела долговременного хранения", "Дела по личному составу", "Дела постоянного хранения" };
            TermGUI.SelectedIndex = 0;
            _documentTable = documentTable;
            StructDivisionGUI.ItemsSource = documentTable.GetCellValues("struct_division");
        }

        private void InventGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Файл формата (*.docx)| *.docx";
            openFileDialog.DefaultDirectory = Directory.GetCurrentDirectory();
            openFileDialog.ShowDialog();


            if (StructDivisionGUI.Text.Equals(""))
            {
                List<string> Divisions = _documentTable.GetCellValues("struct_division");
                foreach (string struct_division in Divisions)
                {
                    _documentTable.FormDestroyingAct(
                        openFileDialog.FileName.Replace(".docx", "") + " " +
                        struct_division.Replace("\\", "").Replace("/", "").Replace("|", "").Replace("\"", "").Replace("*", "").Replace(":", "").Replace("?", "") // чтобы винда приняла имя файла
                        + ".docx", DestroyActNumberGUI.Text, TermGUI.Text, YearPickerGUI.Text, struct_division);
                    MessageBox.Show($"Акты сгенерированы по пути: {openFileDialog.FileName}");
                }
            }
            else
            {
                _documentTable.FormDestroyingAct(openFileDialog.FileName, DestroyActNumberGUI.Text, TermGUI.Text, YearPickerGUI.Text, StructDivisionGUI.Text);
                MessageBox.Show($"Акт сгенерирован по пути: {openFileDialog.FileName}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DestroyActNumberGUI.Text = string.Empty;
            StructDivisionGUI.Text = string.Empty;
            TermGUI.SelectedIndex = 0;
            YearPickerGUI.Text = string.Empty;
        }

        private void YearPickerGUI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;

        }

        private void YearPickerGUI_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string tempText = e.Text;
            e.Handled = !(int.TryParse(e.Text, out _) && e.Text.Replace(" ", "") == tempText);
        }

        private void YearPickerGUI_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text != "")
            {
                e.Handled = Convert.ToInt32((sender as TextBox).Text) < 0;
            }
            else { e.Handled = false; }
        }
    }
}
