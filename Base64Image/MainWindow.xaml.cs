using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace Base64Image {

    public partial class MainWindow : Window {

        private readonly Image oImage;

        public MainWindow() {
            InitializeComponent();

            oImage = new Image {
                Status = "No image in clipboard",
                Text = ""
            };

            if (Clipboard.ContainsText()) { oImage.Text = Clipboard.GetText(); }
            if (Clipboard.ContainsImage()) { GetImageFromClipboard(); }


            DataContext = oImage;

        }

        private void GetImageFromClipboard() {


            IDataObject dataObject = Clipboard.GetDataObject();

            string[] formats = dataObject.GetFormats(true);
            //foreach (var f in formats) oImage.Text += $"- {f}\n";


            if (formats[0].Contains("PNG")) {

                using System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObject.GetData("PNG");
                ms.Position = 0;

                System.IO.MemoryStream msJPG = new();
                System.IO.MemoryStream msPNG = new();
                System.IO.MemoryStream msGIF = new();
                System.Drawing.Bitmap bitmap = new(ms);
                bitmap.Save(msJPG, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Save(msPNG, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Save(msGIF, System.Drawing.Imaging.ImageFormat.Gif);

                rbJPG.Content = $"_JPG ({msJPG.Length / 1024}kb)";
                rbPNG.Content = $"_PNG ({msPNG.Length / 1024}kb)";
                rbGIF.Content = $"_GIF ({msGIF.Length / 1024}kb)";

                rbJPG.IsChecked = msJPG.Length <= Math.Min(msPNG.Length, msGIF.Length);
                rbPNG.IsChecked = msPNG.Length <= Math.Min(msJPG.Length, msGIF.Length);
                rbGIF.IsChecked = msGIF.Length <= Math.Min(msPNG.Length, msJPG.Length);


                System.Windows.Media.Imaging.BitmapImage imageSource = new();
                imageSource.BeginInit();
                imageSource.StreamSource = ms;
                imageSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                imageSource.EndInit();
                imageSource.Freeze();

                oImage.Source = imageSource;
                oImage.Text = Convert.ToBase64String(ms.ToArray());
                oImage.Status = $"Clipboard Image: {imageSource.Format} {imageSource.PixelHeight}x{imageSource.PixelWidth} {ms.Length / 1024} kb";
            }

        }

        public static System.Drawing.Bitmap BitmapSourceToBitmap(System.Windows.Media.Imaging.BitmapSource source) {
            if (source == null) return null;

            var pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;  //Bgr32 equiv default
            if (source.Format == System.Windows.Media.PixelFormats.Bgr24) pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            else if (source.Format == System.Windows.Media.PixelFormats.Pbgra32) pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
            else if (source.Format == System.Windows.Media.PixelFormats.Prgba64) pixelFormat = System.Drawing.Imaging.PixelFormat.Format64bppPArgb;

            var bmp = new System.Drawing.Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                pixelFormat);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),  //Point.Empty
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                pixelFormat);

            source.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height * data.Stride,
                data.Stride);

            bmp.UnlockBits(data);

            return bmp;
        }

        private void BtnData_Click(object sender, RoutedEventArgs e) {

            //oImage.Text = Convert.ToBase64String(GetImageFromClipboard());
            //img.Source = GetImageFromClipboard();
            GetImageFromClipboard();


        }

        private void BtnImg_Click(object sender, RoutedEventArgs e) {

        }

        private void BtnMD_Click(object sender, RoutedEventArgs e) {

        }
    }
}
