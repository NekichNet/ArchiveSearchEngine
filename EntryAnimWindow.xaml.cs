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
        private float animProgress = 0.0f;

        public EntryAnimWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(30)
            };
            _timer.Tick += Timer_Tick;

            _timer.Start();
            ChangeBackground();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(animProgress >= 1.0)
            {
                ChangeBackground();
            }
            else
            {
                animProgress = animProgress + 0.05f;
                BackgroundImage2.Opacity = Math.Clamp(animProgress, 0.0f, 1.0f);
            }
        }


        private void ChangeBackground()
        {
            if (currentIndex < _imageFiles.Length && currentIndex + 1 < _imageFiles.Count())
            {
                BitmapImage bitmap1 = new BitmapImage(new Uri(_imageFiles[currentIndex], UriKind.Relative));
                BitmapImage bitmap2 = new BitmapImage(new Uri(_imageFiles[currentIndex+1], UriKind.Relative));
                animProgress = -0.5f;

                BackgroundImage1.Source = bitmap1;
                BackgroundImage2.Opacity = 0.0f;
                BackgroundImage2.Source = bitmap2;
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
