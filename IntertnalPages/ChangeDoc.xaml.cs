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

        public ChangeDoc(DocRegistry owner, DocumentTable documentTable, HistoryTable historyTable, int index)
        {
            InitializeComponent();
            owner_ = owner;
            documentTable_ = documentTable;
            historyTable_ = historyTable;
            index_ = index;

            try
            {
            Document doc = documentTable.GetDocument(index+1);

            

            RegistrationObjectNumberGUI.Text = doc.RegistrationNum;
            TomNumberGUI.Text = doc.VolumeNum;
            BookNumberGUI.Text = doc.BookNum;
            AmountOfSheetsGUI.Text = $"{doc.ContentQuantity}";
            InventoryDateGUI.DisplayDate = doc.InventoryDate;
            InventoryDateGUI.Text = doc.InventoryDate.ToShortDateString();
            InventoryNumberGUI.Text = doc.InventoryNum;
            DealIndexGUI.Text = doc.ObjectIndex;
            ObjectNameGUI.Text = doc.ObjectName;
            RackGUI.Text = doc.Rack;
            ShelfGUI.Text = doc.Shelf;
            StoringTermGUI.Text = doc.ExpiringIn;
            DocDateGUI.DisplayDate = doc.DocumentsDate;
            DocDateGUI.Text = doc.DocumentsDate.ToShortDateString();
            CaseNumberGUI.Text = doc.CaseNum;
            DestroyActDateGUI.DisplayDate= doc.DestructActDate;
            DestroyActDateGUI.Text= doc.DestructActDate.ToShortDateString();
            StructSubdivisionGUI.Text = doc.StructDivision;
            PostGUI.Text = doc.GivedPost;
            FullnameGUI.Text = doc.GivedFullname;

            if(doc.Available)

            }catch {}
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
        }

        private void ReturnDocButton_Click(object sender, RoutedEventArgs e)
        {
            TakeDocButton.Visibility = Visibility.Visible;
            ReturnDocButton.Visibility = Visibility.Collapsed;
        }
        private void Refresh()
        {

        }
    }
}
