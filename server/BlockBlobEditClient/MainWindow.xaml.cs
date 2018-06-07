using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAzure.Storage.Blob;
using Path = System.IO.Path;

namespace BlockBlobEditClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AzureIntegration _client;
        public MainWindow()
        {
            InitializeComponent();
            _client = new AzureIntegration();
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter =
                    @"All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff"
            };

            if (dlg.ShowDialog() == true)
            {
                var fileName = dlg.FileName;
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("File does not exist.");
                    return;
                }

                TxtFile.Text = fileName;
                DataContext = new { PreviewImgSrc = fileName };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(TxtFile.Text))
            {
                MessageBox.Show("File does not exist.");
                return;
            }

            try
            {
                _client.UploadImage(TxtFile.Text.Trim());
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh()
        {
            var images = _client.GetImageList();
            DataContext = new
            {
                Images = images
            };
        }
    }
}
