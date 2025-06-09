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
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ArchiveSearchEngine
{
  
    public partial class EntryAnimWindow : Window
    {
        private readonly string[] _imageFiles =
        {
            "/sources/background1.png",
            "/sources/background2.png",
            "/sources/background3.png",
        };

        private int _currentIndex = 0;
        private readonly DispatcherTimer _timer;
        private int currentIndex = 0;
        public EntryAnimWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            _timer.Start();
            ChangeBackground();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ChangeBackground();
        }


        private void ChangeBackground()
        {
            if (currentIndex < _imageFiles.Length)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(_imageFiles[currentIndex], UriKind.Relative));
                BackgroundImage.Source = bitmap;
                currentIndex++;
            }
            else
            {
                _timer.Stop();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
