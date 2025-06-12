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
        List<Document> documents_;
        UserTable userTable_;
        public DocRegistry(MainSpace owner, DocumentTable documentTable, UserTable userTable)
        {
            _owner = owner;
            InitializeComponent();
            documents_ = documentTable.GetDocuments(0);
            DocGrid.ItemsSource = documents_; 
            documentTable_ = documentTable;
            userTable_ = userTable;

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    DocGrid.Items.Refresh();
                }
            };
        }

        private void ChangePage(int page)
        {
            documents_ = documentTable_.GetDocuments(page);
        }

        private void DocGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ChangeDockWin = new ChangeDoc(this, documentTable_, documents_[DocGrid.SelectedIndex], userTable_); // , historyTable_
            ChangeDockWin.ShowDialog();
        }
    }
}
