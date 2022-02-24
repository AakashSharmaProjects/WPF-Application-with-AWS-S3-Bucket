using Amazon;
using Amazon.S3;
using System.Configuration;
using System.Windows;

namespace Aakash_301134994_Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private static AmazonS3Client s3Client;
        public MainWindow()
        {
            InitializeComponent();
            s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["awsAccessKey"], ConfigurationManager.AppSettings["awsSecretKey"], RegionEndpoint.APSouth1);
        }
        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
            CreateBucketWindow createBucketWindow = new CreateBucketWindow(s3Client);
            createBucketWindow.Show();
            Close();
        }

        private void btnObjectLevelOps_Click(object sender, RoutedEventArgs e)
        {
            ObjectLevelOperations operations= new ObjectLevelOperations(s3Client);
            operations.Show();
            Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
