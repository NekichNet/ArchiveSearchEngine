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

namespace ArchiveSearchEngine.IntertnalPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeDoc.xaml
    /// </summary>
    public partial class ChangeDoc : Window
    {
        DocRegistry owner_;
        DocumentTable documentTable_;
        Document doc_;
        UserTable userTable_;

        public ChangeDoc(DocRegistry owner, DocumentTable documentTable, Document doc, UserTable userTable)
        {
            InitializeComponent();
            owner_ = owner;
            documentTable_ = documentTable;
            userTable_ = userTable;

            try
            {

                doc_ = doc;

                IsPersonnelGUI.IsChecked = doc_.IsPersonnel;
                RegistrationObjectNumberGUI.Text = doc_.RegistrationNum;
                TomNumberGUI.Text = doc_.VolumeNum;
                BookNumberGUI.Text = doc_.BookNum;
                AmountOfSheetsGUI.Text = $"{doc_.ContentQuantity}";
                if (doc_.InventoryDate is not null)
                {
                    InventoryDateGUI.DisplayDate = (DateTime)doc_.InventoryDate;
                    InventoryDateGUI.Text = ((DateTime)doc_.InventoryDate).ToShortDateString();
                }
                InventoryNumberGUI.Text = doc_.InventoryNum;
                DealIndexGUI.Text = doc_.ObjectIndex;
                ObjectNameGUI.Text = doc_.ObjectName;
                StorageGUI.Text = doc_.Storage;
                RackGUI.Text = doc_.Rack;
                ShelfGUI.Text = doc_.Shelf;
                StoringTermComboGUI.Text = doc_.ExpiringIn;
                StoringTermComboGUI.ItemsSource = new List<string> { "5", "10", "Постоянно" };
                DocDateGUI.DisplayDate = doc_.DocumentsDate;
                DocDateGUI.Text = doc_.DocumentsDate.ToShortDateString();
                CaseNumberGUI.Text = $"{doc_.CaseNum}";
                if (doc_.DestructActDate is not null)
                {
                    DestroyActDateGUI.DisplayDate = (DateTime)doc_.DestructActDate;
                    DestroyActDateGUI.Text = ((DateTime)doc_.DestructActDate).ToShortDateString();
                }
                DestroyActNumberGUI.Text = doc_.DestructActNum;
                AdditionGUI.Text = doc_.Note;

                AccountThatFirstAddedPreviewButton.Content = userTable.GetUser(doc_.AchievedUsername).Fullname;

                if (doc_.Available)
                {
                    TakeDocButton.Visibility = Visibility.Visible;
                    ReturnDocButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TakeDocButton.Visibility = Visibility.Collapsed;
                    ReturnDocButton.Visibility = Visibility.Visible;
                }
                Refresh();


            }
            catch {}
        }

        private void AcceptAddition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Document updatedDoc = new Document(RegistrationObjectNumberGUI.Text, TomNumberGUI.Text, BookNumberGUI.Text,
                    Int32.Parse(AmountOfSheetsGUI.Text), InventoryNumberGUI.Text, DealIndexGUI.Text, ObjectNameGUI.Text, StorageGUI.Text, RackGUI.Text,
                    ShelfGUI.Text, StoringTermComboGUI.Text, (DateTime)DocDateGUI.SelectedDate, Int32.Parse(CaseNumberGUI.Text), DestroyActNumberGUI.Text,
                    doc_.StructDivision, doc_.GivedPost, doc_.GivedFullname, doc_.IsPersonnel, doc_.AchievedUsername,
                    AdditionGUI.Text, (DateTime?)InventoryDateGUI.SelectedDate, (DateTime?)DestroyActDateGUI.SelectedDate);

                documentTable_.UpdateDocument(updatedDoc, doc_.Id);
                owner_.RefreshDataGrid();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeclineAddition_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TakeDocButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDocButton.Visibility = Visibility.Collapsed;
            ReturnDocButton.Visibility = Visibility.Visible;
            documentTable_.TakeDocument(doc_.Id, owner_._owner.Owner.LoggedUser.Username);
            Refresh();
        }

        private void ReturnDocButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDocButton.Visibility = Visibility.Visible;
            ReturnDocButton.Visibility = Visibility.Collapsed;

            documentTable_.ReturnDocument(doc_.Id);
            Refresh();
        }
        private void Refresh()
        {
            if (doc_.Available)
            {
                
                DocStatus.Text = "Доступен";
                
                AccountThatTookPreviewButton.Visibility = Visibility.Collapsed;
                TakeReturnButtonSheet.Visibility = Visibility.Visible;
            }
            else
            {
                AccountThatTookPreviewButton.Content = documentTable_.UserWhoTook(doc_.Id);
                if (owner_._owner.Owner.LoggedUser.Username != documentTable_.UserWhoTook(doc_.Id))
                {
                    DocStatus.Text = "Вне архива, забрал: ";
                    AccountThatTookPreviewButton.Visibility = Visibility.Visible;
                }
                else
                {
                    DocStatus.Text = "Вне архива, вы забрали этот документ";
                    AccountThatTookPreviewButton.Visibility = Visibility.Collapsed;
                }

                if (documentTable_.UserWhoTook(doc_.Id) == owner_._owner.Owner.LoggedUser.Username)
                {
                    TakeReturnButtonSheet.Visibility = Visibility.Visible;
                }
                else
                {
                    TakeReturnButtonSheet.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AccountThatTookPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var previewUser = new UserPreview(documentTable_.UserWhoTook(doc_.Id), userTable_);
            previewUser.ShowDialog();
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

        private void AccountThatFirstAddedPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var previewUser = new UserPreview(doc_.AchievedUsername, userTable_);
            previewUser.ShowDialog();
        }
    }
}
