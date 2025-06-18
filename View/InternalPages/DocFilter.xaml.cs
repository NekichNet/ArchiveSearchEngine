using ArchiveSearchEngine.Model.Database;
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
    /// Логика взаимодействия для DocFilter.xaml
    /// </summary>
    public partial class DocFilter : Window
    {
        public DocRegistry _owner;
        public DocFilter(DocRegistry owner, DocumentFilter filter)
        {
            InitializeComponent();
            _owner = owner;

            ObjectIndexGUI.Text = filter.ObjectIndex;
            ObjectNameGUI.Text = filter.ObjectName;
            VolumeNumGUI.Text = filter.VolumeNum;
            BookNumGUI.Text = filter.BookNum;
            ContentQuantityGUI.Text = filter.ContentQuantity;
            ExpiringInGUI.Text = filter.ExpiringIn;
            DocumentsDateGUI.Text = filter.DocumentsDate;

            ExpiringInGUI.ItemsSource = new List<string> { "5", "10", "Постоянно" };
        }

        private void DeclineFilterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptFilterButton_Click(object sender, RoutedEventArgs e)
        {
            _owner.SetFilter(ObjectIndexGUI.Text, ObjectNameGUI.Text, VolumeNumGUI.Text,
                BookNumGUI.Text, ContentQuantityGUI.Text, ExpiringInGUI.Text, DocumentsDateGUI.Text, StructDivisionGUI.Text);
            this.Close();
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {

            ObjectIndexGUI.Text = "";
            ObjectNameGUI.Text = "";
            VolumeNumGUI.Text = "";
            BookNumGUI.Text = "";
            ContentQuantityGUI.Text = "";
            ExpiringInGUI.Text = "";
            DocumentsDateGUI.Text = "";
            StructDivisionGUI.Text = "";
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
            e.Handled = Convert.ToInt32((sender as TextBox).Text) < 0;
        }
    }
}
