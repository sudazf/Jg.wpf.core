using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// RoiEditorDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RoiEditorDemo : UserControl
    {
        public RoiEditorDemo()
        {
            InitializeComponent();
            ProvideImage();
        }

        private void ProvideImage()
        {
            var filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Images\\RoiDemo.bmp");
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
    }
}
