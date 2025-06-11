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
        HistoryTable historyTable_;
        int index_;
        Document doc_;

        public ChangeDoc(DocRegistry owner, DocumentTable documentTable, HistoryTable historyTable, int index)
        {
            InitializeComponent();
            owner_ = owner;
            documentTable_ = documentTable;
            historyTable_ = historyTable;
            index_ = index;

            try
            {
                doc_ = documentTable.GetDocument(index+1);

            

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
                StructSubdivisionGUI.Text = doc_.StructDivision;
                PostGUI.Text = doc_.GivedPost;
                FullnameGUI.Text = doc_.GivedFullname;

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
            historyTable_.TakeDocument(owner_._owner.Owner.LoggedUser.Username, index_);
            Refresh();
        }

        private void ReturnDocButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDocButton.Visibility = Visibility.Visible;
            ReturnDocButton.Visibility = Visibility.Collapsed;
            historyTable_.ReturnDocument(index_);
            Refresh();
        }
        private void Refresh()
        {
            if (doc_.Available)
            {
                //MessageBox.Show("Доступен");
                DocStatus.Text = "Доступен";
                
                AccountThatTookPreviewButton.Visibility = Visibility.Collapsed;
                TakeReturnButtonSheet.Visibility = Visibility.Visible;
            }
            else
            {
                DocStatus.Text = "Вне архива, забрал: ";
                AccountThatTookPreviewButton.Content = historyTable_.UserWhoTook(index_);
                AccountThatTookPreviewButton.Visibility = Visibility.Visible;
                if (historyTable_.UserWhoTook(index_) == owner_._owner.Owner.LoggedUser.Username)
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
            MessageBox.Show(historyTable_.UserWhoTook(index_));
        }
    }
}
