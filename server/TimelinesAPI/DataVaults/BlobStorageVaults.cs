using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public class BlobStorageVaults : StorageVaultsBase
    {
	    public const string IMAGE_CONTAINER = "pictureblobs";

		public BlobStorageVaults(StorageAccountSettings settings) : base(settings)
	    {
	    }

	    private async Task<CloudBlobContainer> GetBlobContainerAsync(string name)
	    {
		    var account = GetAccount();
		    var client = account.CreateCloudBlobClient();

		    var container = client.GetContainerReference(name);
		    container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

		    BlobContainerPermissions permissions = await container.GetPermissionsAsync();
		    // Container 中的所有 Blob 都能被访问
		    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
		    await container.SetPermissionsAsync(permissions);

		    return container;
	    }

		public async Task<string> UploadImageAsync(string toBeUploaded)
		{
			var container = await GetBlobContainerAsync(IMAGE_CONTAINER);
			
			try
			{
				var fileName = Path.GetFileName(toBeUploaded);

				CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);
				await cloudBlockBlob.UploadFromFileAsync(toBeUploaded);

				return cloudBlockBlob.Uri.AbsoluteUri;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<Uri>> GetImageListAsync()
		{
			var list = new List<Uri>();
			var container = await GetBlobContainerAsync(IMAGE_CONTAINER);
			BlobContinuationToken blobContinuationToken = null;
			do
			{
				var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
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
