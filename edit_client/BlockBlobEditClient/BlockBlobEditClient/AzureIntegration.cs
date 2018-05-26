using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlockBlobEditClient
{
    class AzureIntegration
    {
        public const string IMAGE_CONTAINER = "pictureblobs";

        private CloudStorageAccount GetAccount()
        {
            if (CloudStorageAccount.TryParse(
                "DefaultEndpointsProtocol=https;AccountName=timelines;AccountKey=1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==;EndpointSuffix=core.chinacloudapi.cn",
                out CloudStorageAccount storagetAccount))
            {
                return storagetAccount;
            }
            else
            {
                throw new Exception();
            }
        }
        
        private CloudBlobContainer GetBlobContainer(string name)
        {
            var account = GetAccount();
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(name);
            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            BlobContainerPermissions permissions = container.GetPermissions();
            // Container 中的所有 Blob 都能被访问
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            return container;
        }

        public void UploadImage(string imagePath)
        {
            var container = GetBlobContainer(IMAGE_CONTAINER);
            var localFileName = "EF_" + Guid.NewGuid() + Path.GetExtension(imagePath);
            var localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var sourceFile = Path.Combine(localPath, localFileName);

            File.Copy(imagePath, sourceFile);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(localFileName);
            cloudBlockBlob.UploadFromFile(sourceFile);
        }

        public List<Uri> GetImageList()
        {
            var list = new List<Uri>();
            var container = GetBlobContainer(IMAGE_CONTAINER);
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = container.ListBlobsSegmented(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    list.Add(item.Uri);
                }
            } while (blobContinuationToken != null);

            return list;
        }
    }
}
