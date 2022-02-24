using Amazon.S3;
using Amazon.S3.Model;
using System.Windows;

namespace Aakash_301134994_Lab1
{
    /// <summary>
    /// Interaction logic for CreateBucketWindow.xaml
    /// </summary>
    public partial class CreateBucketWindow : Window
    {
        private AmazonS3Client s3Client;
        public CreateBucketWindow()
        {
            InitializeComponent();
        }

        public CreateBucketWindow(AmazonS3Client s3) : this()
        {
            s3Client = s3;
            IntializeDataGrid();
        }

        private async void IntializeDataGrid()
        {
            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            dgBucketList.ItemsSource = response.Buckets;
        }

        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
            BucketCreation();
        }

        public async void BucketCreation()
        {
            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            if (response.Buckets.Exists(x => x.BucketName == txtBucketName.Text))
            {
                MessageBox.Show("Bucket name is already exist, please try with another name!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                await s3Client.PutBucketAsync(txtBucketName.Text);
                MessageBox.Show("New Bucket has been created successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            IntializeDataGrid();
            txtBucketName.Text = string.Empty;
        }
    }
}
