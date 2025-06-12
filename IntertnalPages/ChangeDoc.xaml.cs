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

        public ChangeDoc(DocRegistry owner, DocumentTable documentTable, Document doc) // , HistoryTable historyTable
        {
            InitializeComponent();
            owner_ = owner;
            documentTable_ = documentTable;
            //historyTable_ = historyTable;

            try
            {

                doc_ = doc;


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
                StoringTermGUI.Text = doc_.ExpiringIn;
                DocDateGUI.DisplayDate = doc_.DocumentsDate;
                DocDateGUI.Text = doc_.DocumentsDate.ToShortDateString();
                CaseNumberGUI.Text = doc_.CaseNum;
                DestroyActDateGUI.DisplayDate= doc_.DestructActDate;
                DestroyActDateGUI.Text= doc_.DestructActDate.ToShortDateString();
                DestroyActNumberGUI.Text = doc_.DestructActNum;
                StructSubdivisionGUI.Text = doc_.StructDivision;
                PostGUI.Text = doc_.GivedPost;
                FullnameGUI.Text = doc_.GivedFullname;
                AdditionGUI.Text = doc_.Note;

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
                DocStatus.Text = "Вне архива, забрал: ";
                AccountThatTookPreviewButton.Content = documentTable_.UserWhoTook(doc_.Id);
                AccountThatTookPreviewButton.Visibility = Visibility.Visible;
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
            MessageBox.Show(documentTable_.UserWhoTook(doc_.Id));
        }
    }
}
