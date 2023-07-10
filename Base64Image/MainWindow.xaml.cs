using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls;

namespace Base64Image {

    public partial class MainWindow : MetroWindow {

        private readonly Image oImage;
        private System.IO.MemoryStream msPNG = new();
        private System.IO.MemoryStream msJPG = new();
        private System.IO.MemoryStream msGIF = new();

        public MainWindow() {
            InitializeComponent();

            Title = $"{Title} {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major}.{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor}.{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build}";;

            oImage = new Image {
                Status = "No image in clipboard",
                Text = ""
            };

            if (Clipboard.ContainsImage()) { GetImageFromClipboard(); }

            DataContext = oImage;

        }

        private void GetImageFromClipboard() {

            IDataObject dataObject = Clipboard.GetDataObject();

            string[] formats = dataObject.GetFormats(true);

            bool containsPng = false;
            foreach (var format in formats) {
                if (format.Contains("PNG")) {
                    containsPng = true;
                    break;
                }
            }

            if (containsPng) {

                using System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObject.GetData("PNG");
                ms.Position = 0;


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
                oImage.Status = $"Clipboard Image: {imageSource.Format} {imageSource.PixelHeight}x{imageSource.PixelWidth} {ms.Length / 1024} kb";
            }

        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(new ProcessStartInfo {
                FileName = "https://github.com/pagebox/Base64Image",
                UseShellExecute = true
            });
        }

        private void RbPNG_Checked(object sender, RoutedEventArgs e) {
            oImage.Text = $"data:image/png;base64,{Convert.ToBase64String(msPNG.ToArray())}";
        }

        private void RbJPG_Checked(object sender, RoutedEventArgs e) {
            oImage.Text = $"data:image/jpeg;base64,{Convert.ToBase64String(msJPG.ToArray())}";
        }

        private void RbGIF_Checked(object sender, RoutedEventArgs e) {
            oImage.Text = $"data:image/gif;base64,{Convert.ToBase64String(msGIF.ToArray())}";
        }

        private void BtnData_Click(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnImg_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText(oImage.Text);
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnMD_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText($"<img src='{oImage.Text}' style='border:1px solid black; box-shadow: 5px 5px 5px grey;' />");
            System.Windows.Application.Current.Shutdown();
        }

    }
}
