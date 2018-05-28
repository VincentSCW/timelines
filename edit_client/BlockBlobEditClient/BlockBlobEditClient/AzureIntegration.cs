using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            if (new FileInfo(imagePath).Length > 300000)
            {
                GetThumImage(imagePath, 20000, 2, sourceFile);
            }
            else
            {
                File.Copy(imagePath, sourceFile);
            }

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

        #region getThumImage  
        /**/
        /// <summary>  
        /// 生成缩略图  
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool GetThumImage(String sourceFile, long quality, int multiple, String outputFile)
        {
            try
            {
                long imageQuality = quality;
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / multiple), (int)(yWidth / multiple));
                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
                g.Dispose();
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion getThumImage  

        /**/
        /// <summary>  
        /// 获取图片编码信息  
        /// </summary>  
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
