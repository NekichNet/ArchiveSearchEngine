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
        private int page_;
        public DocRegistry(MainSpace owner, DocumentTable documentTable)
        {
            _owner = owner;
            InitializeComponent();
            documents_ = documentTable.GetDocuments(0);
            DocGrid.ItemsSource = documents_; 
            documentTable_ = documentTable;

            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    DocGrid.Items.Refresh();
                }
            };
        }
        private void NextPage(object sender, RoutedEventArgs e)
        {
            CounterBox.Text = Convert.ToString(page_ + 1);
        }

        private void PrevPage(object sender, RoutedEventArgs e)
        {
            CounterBox.Text = Convert.ToString(page_ - 1);
        }

        private void CounterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string tempText = e.Text;
            e.Handled = !(int.TryParse(e.Text, out _) && e.Text.Replace(" ", "") == tempText);
        }

        private void CounterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = Convert.ToInt32((sender as TextBox).Text);
            if (value > 0) page_ = value;
            else (sender as TextBox).Text = Convert.ToString(page_);
        }

        private void CounterBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space || ((e.Key == Key.Delete || e.Key == Key.Back) && (sender as TextBox).Text.Length == 1);
        }

        private void DocGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ChangeDockWin = new ChangeDoc(this, documentTable_, documents_[DocGrid.SelectedIndex]);
            ChangeDockWin.ShowDialog();
        }
    }
}
