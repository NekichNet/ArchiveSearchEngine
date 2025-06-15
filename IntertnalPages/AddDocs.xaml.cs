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
        int selectedUserIndex = 0;
        UserTable _userTable;
        public AddDocs(MainSpace owner, DocumentTable _documentTable, UserTable userTable)
        {
            _owner = owner;
            InitializeComponent();
            _table = _documentTable;
            FullnameSearchGUI.ItemsSource = userTable.GetUsers().Select(x => x.Fullname);
            _userTable = userTable;
            StoringTermComboGUI.ItemsSource = new List<string> { "5", "10", "ЛС", "Постоянно"};
        }



        public void AddDock()
        {
            try
            {
                User user = _userTable.GetUsers()[selectedUserIndex];
                _table.NewDocument(RegistrationObjectNumberGUI.Text, TomNumberGUI.Text, BookNumberGUI.Text, Int32.Parse(AmountOfSheetsGUI.Text),
                    (DateTime)InventoryDateGUI.SelectedDate, InventoryNumberGUI.Text, DealIndexGUI.Text, ObjectNameGUI.Text, RackGUI.Text,
                    ShelfGUI.Text, StoringTermComboGUI.Text, (DateTime)DocDateGUI.SelectedDate, Int32.Parse(CaseNumberGUI.Text), DestroyActNumberGUI.Text,
                    (DateTime)DestroyActDateGUI.SelectedDate, user.StructDivision, user.Post, user.Fullname, _owner.Owner.LoggedUser.Username,
                    AdditionGUI.Text);

                Clear();
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

        private void FullnameSearchGUI_Selected(object sender, RoutedEventArgs e)
        {
            selectedUserIndex = FullnameSearchGUI.SelectedIndex;
        }

        public void SetPresetValues(Document docAsPreset)
        {
            try
            {
                RegistrationObjectNumberGUI.Text = docAsPreset.RegistrationNum;
                TomNumberGUI.Text = docAsPreset.VolumeNum;
                BookNumberGUI.Text = docAsPreset.BookNum;
                AmountOfSheetsGUI.Text = $"{docAsPreset.ContentQuantity}";
                InventoryDateGUI.DisplayDate = docAsPreset.InventoryDate;
                InventoryDateGUI.Text = docAsPreset.InventoryDate.ToShortDateString();
                InventoryNumberGUI.Text = docAsPreset.InventoryNum;
                DealIndexGUI.Text = docAsPreset.ObjectIndex;
                ObjectNameGUI.Text = docAsPreset.ObjectName;
                RackGUI.Text = docAsPreset.Rack;
                ShelfGUI.Text = docAsPreset.Shelf;
                StoringTermComboGUI.Text = docAsPreset.ExpiringIn;
                DocDateGUI.DisplayDate = docAsPreset.DocumentsDate;
                DocDateGUI.Text = docAsPreset.DocumentsDate.ToShortDateString();
                CaseNumberGUI.Text = $"{docAsPreset.CaseNum}";
                DestroyActDateGUI.DisplayDate = docAsPreset.DestructActDate;
                DestroyActDateGUI.Text = docAsPreset.DestructActDate.ToShortDateString();
                DestroyActNumberGUI.Text = docAsPreset.DestructActNum;
                AdditionGUI.Text = docAsPreset.Note;
                FullnameSearchGUI.Text = docAsPreset.GivedFullname;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {
            RegistrationObjectNumberGUI.Text = "";
            TomNumberGUI.Text = "";
            BookNumberGUI.Text = "";
            AmountOfSheetsGUI.Text = "1";
            InventoryNumberGUI.Text = "";
            DealIndexGUI.Text = "";
            ObjectNameGUI.Text = "";
            RackGUI.Text = "";
            ShelfGUI.Text = "";
            StoringTermComboGUI.Text = "";
            CaseNumberGUI.Text = "";
            DestroyActNumberGUI.Text = "";
            AdditionGUI.Text = "";
            FullnameSearchGUI.Text = "";
            DestroyActDateGUI.DisplayDate = DateTime.Now;
            DestroyActDateGUI.Text = "";
            DocDateGUI.DisplayDate = DateTime.Now;
            DocDateGUI.Text = "";
            InventoryDateGUI.DisplayDate = DateTime.Now;
            InventoryDateGUI.Text = "";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}
