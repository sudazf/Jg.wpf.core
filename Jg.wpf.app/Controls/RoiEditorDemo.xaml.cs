using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// RoiEditorDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RoiEditorDemo : UserControl
    {
        private int _imageIndicator;
        public RoiEditorDemo()
        {
            InitializeComponent();
            ProvideImage();
        }

        private void BtnShowImage(object sender, System.Windows.RoutedEventArgs e)
        {
            ProvideImage();
        }

        private void BtnHideImage(object sender, System.Windows.RoutedEventArgs e)
        {
            Image.Source = null;
        }

        private void ProvideImage()
        {
            _imageIndicator++;

            if (_imageIndicator % 2 == 0)
            {
                var imageFile = "RoiDemo.bmp";

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{imageFile}");
                var image = (Bitmap)System.Drawing.Image.FromFile(filePath);
                var bitmapData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
                var bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Gray8, null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
                image.UnlockBits(bitmapData);

                Image.Source = bitmapSource;
            }
            else
            {
                var imageFile = "RoiDemo2.jpg";

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Images\\{imageFile}");
                var image = (Bitmap)System.Drawing.Image.FromFile(filePath);
                var bitmapData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
                var bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
                image.UnlockBits(bitmapData);

                Image.Source = bitmapSource;
            }
          
        }
    }
}
