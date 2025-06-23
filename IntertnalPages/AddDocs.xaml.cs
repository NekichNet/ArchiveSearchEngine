using ArchiveSearchEngine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        UserTable _userTable;
        NonUserTable _nonUserTable;
        int selectedUserIndex = 0;
        public AddDocs(MainSpace owner, DocumentTable _documentTable, UserTable userTable, NonUserTable nonUserTable)
        {
            _owner = owner;
            InitializeComponent();
            _table = _documentTable;
            _userTable = userTable;
            _nonUserTable = nonUserTable;
            RefreshFullnameSearchGUI();
            StoringTermComboGUI.ItemsSource = new List<string> { "3", "5", "10", "Постоянно" };
        }

        public void RefreshFullnameSearchGUI()
        {
            FullnameSearchGUI.ItemsSource = _nonUserTable.GetUnits().Select(x => x.Fullname);
        }

        public void AddDock()
        {
            try
            {
                if (!_table.DocumentExists(RegistrationObjectNumberGUI.Text))
                {
                    User user = _userTable.GetUsers()[selectedUserIndex];
                    _table.NewDocument(RegistrationObjectNumberGUI.Text, 
                        TomNumberGUI.Text, BookNumberGUI.Text, 
                        Int32.Parse(AmountOfSheetsGUI.Text), InventoryNumberGUI.Text, 
                        DealIndexGUI.Text, ObjectNameGUI.Text, 
                        StorageGUI.Text, RackGUI.Text, ShelfGUI.Text, 
                        StoringTermComboGUI.Text, (DateTime)DocDateGUI.SelectedDate, 
                        Int32.Parse(CaseNumberGUI.Text), DestroyActNumberGUI.Text, 
                        user.StructDivision, user.Post, 
                        user.Fullname, (bool)IsPersonnelGUI.IsChecked, 
                        _owner.Owner.LoggedUser.Username, AdditionGUI.Text, 
                        (DateTime?)InventoryDateGUI.SelectedDate, 
                        (DateTime?)DestroyActDateGUI.SelectedDate);

                    MessageBox.Show("Документ был успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

                    Clear();
                }
                else
                {
                    MessageBox.Show("Документ с таким регистрационным номером уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Возможно вы внесли не все параметры\nХотите увидеть более подробный отчёт?","Возникла ошибка", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
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
            e.Handled = e.Key == Key.Space;
        }

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text != "")
            {
                e.Handled = Convert.ToInt32((sender as TextBox).Text) < 0;
            }
            else { e.Handled = false; }
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
                if (docAsPreset.InventoryDate is not null)
                {
                    InventoryDateGUI.DisplayDate = (DateTime)docAsPreset.InventoryDate;
                    InventoryDateGUI.Text = ((DateTime)docAsPreset.InventoryDate).ToShortDateString();
                }
                InventoryNumberGUI.Text = docAsPreset.InventoryNum;
                DealIndexGUI.Text = docAsPreset.ObjectIndex;
                ObjectNameGUI.Text = docAsPreset.ObjectName;
                StorageGUI.Text = docAsPreset.Storage;
                RackGUI.Text = docAsPreset.Rack;
                ShelfGUI.Text = docAsPreset.Shelf;
                StoringTermComboGUI.Text = docAsPreset.ExpiringIn;
                DocDateGUI.DisplayDate = docAsPreset.DocumentsDate;
                DocDateGUI.Text = docAsPreset.DocumentsDate.ToShortDateString();
                CaseNumberGUI.Text = $"{docAsPreset.CaseNum}";
                if (docAsPreset.DestructActDate is not null)
                {
                    DestroyActDateGUI.DisplayDate = (DateTime)docAsPreset.DestructActDate;
                    DestroyActDateGUI.Text = ((DateTime)docAsPreset.DestructActDate).ToShortDateString();
                }
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
            AmountOfSheetsGUI.Text = "";
            InventoryNumberGUI.Text = "";
            DealIndexGUI.Text = "";
            ObjectNameGUI.Text = "";
            StorageGUI.Text = "";
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
