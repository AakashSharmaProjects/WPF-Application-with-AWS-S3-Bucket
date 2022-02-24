using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Win32;
using System;
using System.Windows;

namespace Aakash_301134994_Lab1
{
    /// <summary>
    /// Interaction logic for ObjectLevelOperations.xaml
    /// </summary>
    public partial class ObjectLevelOperations : Window
    {
        private AmazonS3Client s3Client;
        public ObjectLevelOperations()
        {
            InitializeComponent();
        }

        public ObjectLevelOperations(AmazonS3Client s3) : this()
        {
            s3Client = s3;
            InitializeCombBox();
        }

        private async void InitializeCombBox()
        {
            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            foreach (var bucket in response.Buckets)
            {
                cmbBuckets.Items.Add(bucket.BucketName);
            }
            fetchObjectsInfo();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\";
            // Show save file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                txtFilePath.Text = openFileDialog.FileName;
            }

        }
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
           UploadFileAsync();
        }

        private async void UploadFileAsync()
        {
            try
            {
                if (cmbBuckets.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a valid bucket.");
                    return;

                }
                if (string.IsNullOrEmpty(txtFilePath.Text))
                {
                    MessageBox.Show("Please select a valid file path.");
                    return;
                }

                var fileTransferUtility = new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(txtFilePath.Text, cmbBuckets.SelectedItem.ToString());
                MessageBox.Show("File has been uploaded successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                txtFilePath.Text = string.Empty;
                fetchObjectsInfo();
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        private async void fetchObjectsInfo()
        {
            try
            {
                if (cmbBuckets.SelectedItem != null)
                {
                    var response = await s3Client.ListObjectsAsync(cmbBuckets.SelectedItem.ToString());
                    dgObjectList.ItemsSource = response.S3Objects;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void cmbBuckets_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            fetchObjectsInfo();
        }
    }
}
