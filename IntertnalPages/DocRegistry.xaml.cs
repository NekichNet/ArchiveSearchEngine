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
    /// Логика взаимодействия для DocRegistry.xaml
    /// </summary>
    public partial class DocRegistry : Page
    {
        public MainSpace _owner;
        DocumentTable documentTable_;
        //HistoryTable historyTable_;
        public DocRegistry(MainSpace owner, DocumentTable documentTable) // , HistoryTable historyTable
        {
            _owner = owner;
            InitializeComponent();
            DocGrid.ItemsSource = documentTable.GetDocuments();
            documentTable_ = documentTable;
            //historyTable_ = historyTable;

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    DocGrid.Items.Refresh();
                }
            };
        }

        private void DocGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ChangeDockWin = new ChangeDoc(this, documentTable_, DocGrid.SelectedIndex); // , historyTable_
            ChangeDockWin.ShowDialog();
        }
    }
}
