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
using static System.Net.Mime.MediaTypeNames;

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
        private int page_;
        UserTable userTable_;
        private DocumentFilter filter_;
        private bool initialized_ = false;
        public DocRegistry(MainSpace owner, DocumentTable documentTable, UserTable userTable)
        {
            _owner = owner;
            InitializeComponent();
            filter_ = new DocumentFilter();
            documents_ = documentTable.GetDocuments(0, filter_);
            DocGrid.ItemsSource = documents_; 
            documentTable_ = documentTable;
            userTable_ = userTable;
            page_ = 1;

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    documents_ = documentTable_.GetDocuments(page_ - 1, filter_);
                    DocGrid.ItemsSource = documents_;
                    DocGrid.Items.Refresh();
                }
            };
        }

        // Preprocessing pagechanging only by Counterbox and '<-' '->' buttons.
        // If it's going to become empty, it returns false
        //private bool ChangePage(int newPage)
        //{
        //    try
        //    {
        //        if (newPage == page_) { return true; }
        //        if (newPage < 1) { return false; }
        //        List<Document> newDocuments = documentTable_.GetDocuments(page_ - 1, filter_);
        //        if (newDocuments.Count > 0)
        //        {
        //            page_ = newPage;
        //            documents_ = newDocuments;
        //            DocGrid.ItemsSource = documents_;
        //            DocGrid.Items.Refresh();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (NullReferenceException ex) { return true; }
        //}
        
        // Processing '->' button press
        private void NextPage(object sender, RoutedEventArgs e)
        {
            CounterBox.Text = Convert.ToString(page_ + 1);
        }

        // Processing '<-' button press
        private void PrevPage(object sender, RoutedEventArgs e)
        {
            if (page_ > 1)
            {
                CounterBox.Text = Convert.ToString(page_ - 1);
            }
        }

        private void CounterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string tempText = e.Text;
            e.Handled = !(int.TryParse(e.Text, out _) && e.Text.Replace(" ", "") == tempText);
        }

        private void CounterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // чтобы не вылезала ошибка перед инициализированием documentTable_
            if (!initialized_) { initialized_ = true; return; }
            page_ = Convert.ToInt32((sender as TextBox).Text);
            documents_ = documentTable_.GetDocuments(page_ - 1, filter_);
            DocGrid.ItemsSource = documents_;
            DocGrid.Items.Refresh();
        }

        private void CounterBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space || ((e.Key == Key.Delete || e.Key == Key.Back) && (sender as TextBox).Text.Length == 1);
        }

        private void DocGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var ChangeDockWin = new ChangeDoc(this, documentTable_, documents_[DocGrid.SelectedIndex], userTable_);
                ChangeDockWin.ShowDialog();
            } catch { }
        }

        private void CreateDocByPreset(object sender, RoutedEventArgs e)
        {
            _owner.CreateDocByPreset(documents_[DocGrid.SelectedIndex]);
        }
        
        private void CopyRegistrationNumberToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(documents_[DocGrid.SelectedIndex].RegistrationNum);
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (DocGrid.SelectedItem == null) {
                e.Handled = true;
                return;
            }
        }

        private void OpenFilterMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var filterWindow = new DocFilter(this, filter_);
            filterWindow.ShowDialog();
        }

        public void SetFilter(string ObjectIndex, string ObjectName, string VolumeNum, 
            string BookNum, string ContentQuantity, string ExpiringIn, string DocumentDate, string StructDivisionGUI)
        {
            filter_.ObjectIndex = ObjectIndex;
            filter_.ObjectName = ObjectName;
            filter_.VolumeNum = VolumeNum;
            filter_.BookNum = BookNum;
            filter_.ContentQuantity = ContentQuantity;
            filter_.ExpiringIn = ExpiringIn;
            filter_.DocumentsDate = DocumentDate;
            DocGrid.Items.Refresh();
        }
    }
}
