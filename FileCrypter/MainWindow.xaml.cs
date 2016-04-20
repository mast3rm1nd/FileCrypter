using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

namespace FileCrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string fileToEncDecPath = "";
        static string fileToEncDecWithPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BrowseFileToEncDec_Button_Click(object sender, RoutedEventArgs e)
        {
            var file_opening_dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = true
            };

            if (file_opening_dialog.ShowDialog(this) != true) return;


            fileToEncDecPath = file_opening_dialog.FileName;
            var fileName = file_opening_dialog.SafeFileName;


            FileToEncDecName_Label.Content = fileName.Replace("_", "__");            
        }

        private void BrowseEncDecFile_Button_Click(object sender, RoutedEventArgs e)
        {
            var file_opening_dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = true
            };

            if (file_opening_dialog.ShowDialog(this) != true) return;


            fileToEncDecWithPath = file_opening_dialog.FileName;
            var fileName = file_opening_dialog.SafeFileName;


            EncDecFileName_Label.Content = fileName.Replace("_", "__");
        }

        private void Go_Button_Click(object sender, RoutedEventArgs e)
        {
            if (fileToEncDecPath == "") return; // file to end\dec have not been selected

            byte[] withWhatToXorBytes = new byte[0];

            if ((bool)IsEncDecByFile_RadioButton.IsChecked)
            {
                 if(fileToEncDecWithPath == "") return; // file to enc\dec with not selected

                withWhatToXorBytes = File.ReadAllBytes(fileToEncDecWithPath);
            }

            if ((bool)IsEncDecByPassword_RadioButton.IsChecked)
            {
                if(Password_TextBox.Text == "") return; // password to enc\dec with is empty string

                withWhatToXorBytes = GetBytes(Password_TextBox.Text);
            }

            var whatToXorBytes = File.ReadAllBytes(fileToEncDecPath);

            var resultingBytes = XorBytes(whatToXorBytes, withWhatToXorBytes);

            File.WriteAllBytes(fileToEncDecPath, resultingBytes);

            MessageBox.Show("Готово!");
        }


        static byte[] XorBytes(byte[] whatToXorBytes, byte[] withWhatToXorBytes)
        {
            for(long index = 0; index < whatToXorBytes.LongLength; index++)
            {
                whatToXorBytes[index] ^= withWhatToXorBytes[index % withWhatToXorBytes.LongLength];
            }

            return whatToXorBytes;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
