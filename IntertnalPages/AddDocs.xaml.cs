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
    /// Логика взаимодействия для AddDocs.xaml
    /// </summary>
    public partial class AddDocs : Page
    {
        MainSpace _owner;
        DocumentTable _table;
        public AddDocs(MainSpace owner, DocumentTable _documentTable)
        {
            _owner = owner;
            InitializeComponent();
            _table = _documentTable;
        }

        public void AddDock()
        {
            try
            {
                _table.NewDocument(RegistrationObjectNumberGUI.Text, TomNumberGUI.Text, BookNumberGUI.Text, Int32.Parse(AmountOfSheetsGUI.Text),
                    (DateTime)InventoryDateGUI.SelectedDate, InventoryNumberGUI.Text, DealIndexGUI.Text, ObjectNameGUI.Text, RackGUI.Text,
                    ShelfGUI.Text, StoringTermGUI.Text, (DateTime)DocDateGUI.SelectedDate, CaseNumberGUI.Text, DestroyActNumberGUI.Text,
                    (DateTime)DestroyActDateGUI.SelectedDate, StructSubdivisionGUI.Text, PostGUI.Text, FullnameGUI.Text, _owner.Owner.LoggedUser.Username,
                    AdditionGUI.Text);
                MessageBox.Show("Документ был успешно добавлен");
            }
            catch (Exception exception)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Возможно вы внесли не все параметры\nХотите увидеть более подробный отчёт?","Возникла ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    MessageBox.Show(exception.Message, "Ошибка");
                }
            }
        }

        private void AcceptAddition_Click(object sender, RoutedEventArgs e)
        {
            AddDock();
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
            if (Convert.ToInt32((sender as TextBox).Text) < 0) e.Handled = true;
        }
    }
}
