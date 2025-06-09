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
        MainSpace _owner;
        public DocRegistry(MainSpace owner)
        {
            _owner = owner;
            InitializeComponent();
        }
    }
}
