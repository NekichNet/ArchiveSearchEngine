using ArchiveSearchEngine.Database;
using NPOI.HSSF.Record;
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

namespace ArchiveSearchEngine.AdditionalElements
{
    /// <summary>
    /// Логика взаимодействия для ProgressBar.xaml
    /// </summary>
   
    public partial class ProgressBar : Window
    {
        public ProgressBar()
        {
            InitializeComponent();
        }
        //public ProgressBar(string filename, string username, DocumentTable documentTable)
        //{
        //    InitializeComponent();
        //    ProgressBarGUI.IsIndeterminate = false;
        //    ProgressBarGUI.Value = 0.0;
        //    Show();
        //    documentTable.ImportFromExcel(filename, username, UpdateValue);
        //    Close();
        //}

        //public void UpdateValue(double val)
        //{
        //    ProgressBarGUI.Value = val;
        //}
    }
}
