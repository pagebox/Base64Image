
using System.ComponentModel;

namespace Base64Image {
    internal class Image : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public System.Windows.Media.Imaging.BitmapSource Source { get; set; }
        public string Status { get; set; }


        private string text;
        public string Text {
            get => text;
            set {
                text = value;
                OnPropertyChanged("Text");
            }
        }



        private string format;

        public string Format {
            get { return format; }
            set {
                format = value;
                OnPropertyChanged("Format");
            }
        }


    }
}
