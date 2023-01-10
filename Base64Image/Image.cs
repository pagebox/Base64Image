
using System.ComponentModel;

namespace Base64Image {
    internal class Image : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private System.Windows.Media.Imaging.BitmapImage source;
        private string status;
        private string format;
        private string text;


        public System.Windows.Media.Imaging.BitmapImage Source {
            get => source;
            set {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        public string Status {
            get => status;
            set {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


        public string Text {
            get => text;
            set {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }



        public string Format {
            get { return format; }
            set {
                format = value;
                OnPropertyChanged(nameof(Format));
            }
        }


    }
}
