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
    /// Логика взаимодействия для DestroyActCreationPage.xaml
    /// </summary>
    public partial class DestroyActCreationPage : Page
    {
        MainSpace _owner;
        public DestroyActCreationPage(MainSpace owner)
        {
            InitializeComponent();
            _owner = owner;
            TermGUI.ItemsSource = new List<string> { "Дела временного хранения", "Дела долговременного хранения", "Дела по личному составу", "Дела постоянного хранения" };
            TermGUI.SelectedIndex = 0;
        }
    }
}
