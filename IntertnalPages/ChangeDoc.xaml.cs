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
        //HistoryTable historyTable_;
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
                InventoryDateGUI.DisplayDate = doc_.InventoryDate;
                InventoryDateGUI.Text = doc_.InventoryDate.ToShortDateString();
                InventoryNumberGUI.Text = doc_.InventoryNum;
                DealIndexGUI.Text = doc_.ObjectIndex;
                ObjectNameGUI.Text = doc_.ObjectName;
                RackGUI.Text = doc_.Rack;
                ShelfGUI.Text = doc_.Shelf;
                StoringTermComboGUI.Text = doc_.ExpiringIn;
                StoringTermComboGUI.ItemsSource = new List<string> { "5", "10", "Постоянно" };
                DocDateGUI.DisplayDate = doc_.DocumentsDate;
                DocDateGUI.Text = doc_.DocumentsDate.ToShortDateString();
                CaseNumberGUI.Text = $"{doc_.CaseNum}";
                DestroyActDateGUI.DisplayDate= doc_.DestructActDate;
                DestroyActDateGUI.Text= doc_.DestructActDate.ToShortDateString();
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
                Document updatedDoc = new Document(RegistrationObjectNumberGUI.Text, TomNumberGUI.Text, BookNumberGUI.Text, Int32.Parse(AmountOfSheetsGUI.Text),
                    (DateTime)InventoryDateGUI.SelectedDate, InventoryNumberGUI.Text, DealIndexGUI.Text, ObjectNameGUI.Text, RackGUI.Text,
                    ShelfGUI.Text, StoringTermGUI.Text, (DateTime)DocDateGUI.SelectedDate, Int32.Parse(CaseNumberGUI.Text), DestroyActNumberGUI.Text,
                    (DateTime)DestroyActDateGUI.SelectedDate, doc_.StructDivision, doc_.GivedPost, doc_.GivedFullname, doc_.IsPersonnel, doc_.AchievedUsername,
                    AdditionGUI.Text);

                documentTable_.UpdateDocument(updatedDoc, doc_.RegistrationNum);
            }
            catch (Exception ex) {
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
            documentTable_.TakeDocument(doc_.RegistrationNum, owner_._owner.Owner.LoggedUser.Username);
            Refresh();
        }

        private void ReturnDocButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDocButton.Visibility = Visibility.Visible;
            ReturnDocButton.Visibility = Visibility.Collapsed;

            documentTable_.ReturnDocument(doc_.RegistrationNum);
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
                AccountThatTookPreviewButton.Content = documentTable_.UserWhoTook(doc_.RegistrationNum);
                if (owner_._owner.Owner.LoggedUser.Username != documentTable_.UserWhoTook(doc_.RegistrationNum))
                {
                    DocStatus.Text = "Вне архива, забрал: ";
                    AccountThatTookPreviewButton.Visibility = Visibility.Visible;
                }
                else
                {
                    DocStatus.Text = "Вне архива, вы забрали этот документ";
                    AccountThatTookPreviewButton.Visibility = Visibility.Collapsed;
                }

                if (documentTable_.UserWhoTook(doc_.RegistrationNum) == owner_._owner.Owner.LoggedUser.Username)
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
            var previewUser = new UserPreview(documentTable_.UserWhoTook(doc_.RegistrationNum), userTable_);
            previewUser.ShowDialog();
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

        private void AccountThatFirstAddedPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var previewUser = new UserPreview(doc_.AchievedUsername, userTable_);
            previewUser.ShowDialog();
        }
    }
}
