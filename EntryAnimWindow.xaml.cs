using System;
using System.Collections.Generic;
using System.Drawing;
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
        private List<string> _imageFiles = new List<string>
        {
            "/sources/background1.png",
            "/sources/background2.png",
            "/sources/background3.png",
            "/sources/background4.png",
            "/sources/background5.png",
            "/sources/background6.png",
        };

        private readonly DispatcherTimer _timer;
        private int currentIndex = 0;
        private float animProgress = 0.0f;
        private int amountOfImagesBeforeStart = 4;
        private int currentAmountOfImages = 0;

        public EntryAnimWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _timer.Tick += Timer_Tick;

            _timer.Start();

            currentIndex = Random.Shared.Next(0, _imageFiles.Count);
            BitmapImage bitmap1 = new BitmapImage(new Uri(_imageFiles[currentIndex], UriKind.Relative));
            BackgroundImage1.Source = bitmap1;
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
            if ( _imageFiles.Count() > 1 && amountOfImagesBeforeStart > currentAmountOfImages)
            {
                animProgress = -0.5f;

                if (BackgroundImage2.Source != null)
                {
                    BackgroundImage1.Source = BackgroundImage2.Source;
                }
                BackgroundImage2.Opacity = 0.0f;

                try
                {
                _imageFiles.RemoveAt(currentIndex);

                }
                catch( Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }

                do
                {
                    currentIndex = Random.Shared.Next(0, _imageFiles.Count);
                    BitmapImage bitmap2 = new BitmapImage(new Uri(_imageFiles[currentIndex], UriKind.Relative));

                    BackgroundImage2.Source = bitmap2;
                } while (BackgroundImage2.Source == BackgroundImage1.Source);


                currentAmountOfImages++;
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
