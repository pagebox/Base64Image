using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace Base64Image {

    public partial class MainWindow : Window {

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        public MainWindow() {
            InitializeComponent();



            var oImage = new Image {
                Status = "No image in clipboard",
                Text = ""
            };
            //, source = "/marvin.jpg"


            if (Clipboard.ContainsText()) { oImage.Text = Clipboard.GetText(); }
            if (Clipboard.ContainsImage()) {

                var oImg = Clipboard.GetImage();

                //-WindowStyle Minimized -file "%USERPROFILE%\Documents\SyMenu\pageBOX_Data\PowerShell\Scripts\ConvertTo-MdImage.ps1" -GreenShot

                oImage.Status = $"Clipboard contains Image: {oImg.Format} {oImg.PixelHeight}x{oImg.PixelWidth}";



                var dataObject = Clipboard.GetDataObject();

                var formats = dataObject.GetFormats(true);
                foreach (var f in formats) oImage.Text += $"- {f}\n";



                //img.Source = oImg;
                //oImage.Source = oImg;


                //var dataObject = Clipboard.GetDataObject();


                if (formats[0].Contains("PNG")) {

                    using (System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObject.GetData("PNG")) {
                        ms.Position = 0;


                        var imageSource = new System.Windows.Media.Imaging.BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = ms;
                        imageSource.EndInit();


                        img.Source = imageSource;
                        

                        // img.Source = (System.Drawing.Bitmap)new System.Drawing.Bitmap(ms);
                        //return (System.Drawing.Bitmap)new System.Drawing.Bitmap(ms);
                        oImage.Text += Convert.ToBase64String(ms.ToArray());
                    }
                }




            }


            this.DataContext = oImage;

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



    }
}
