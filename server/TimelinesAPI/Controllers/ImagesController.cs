using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimelinesAPI.DataVaults;

namespace TimelinesAPI.Controllers
{
	[Route("api/[controller]")]
	public class ImagesController : Controller
	{
        private readonly BlobStorageVaults _blobStorage;

		public ImagesController(BlobStorageVaults blobStorage)
		{
			_blobStorage = blobStorage;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetImageList([FromQuery] string timeline)
		{
			try
			{
				var results = await _blobStorage.GetImageListAsync($"{MockUser.Username}/{timeline}");
				return Ok(results.Select(x => x.AbsoluteUri));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("upload")]
		[Authorize]
		[RequestSizeLimit(5_000_000)] // up to 5 mb
		public async Task<IActionResult> Upload([FromQuery] string folder)
		{
			try
			{
				var file = HttpContext.Request.Form.Files["file"];
                if (file == null)
                    file = HttpContext.Request.Form.Files[0];

				var thumbnailDivideBy = GetThumbnailDivideBy(file.Length);

				var localFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "uploadedImage");
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				var filePath = Path.Combine(directory, localFileName);
                var thumbFilePath = Path.ChangeExtension(filePath, "thumb");

                //Deletion exists file  
                if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}

				using (var stream = System.IO.File.OpenWrite(filePath))
				{
					await file.CopyToAsync(stream);
					stream.Close();
				}

                using (var image = Image.FromFile(filePath))
                {
                    // Thumbnail
                    var thumb = image.GetThumbnailImage(image.Width / thumbnailDivideBy, image.Height / thumbnailDivideBy, () => false, IntPtr.Zero);
                    thumb.Save(thumbFilePath);
                }

                var path = await _blobStorage.UploadImageAsync(MockUser.Username, folder, filePath);
                var thumbPath = await _blobStorage.UploadImageAsync(MockUser.Username, "_thumbnail", thumbFilePath);

                System.IO.File.Delete(filePath);
                System.IO.File.Delete(thumbFilePath);
                if (path == null)
					throw new Exception("Upload to Azure failed.");

				return Ok(new { url = path, thumbnail = thumbPath });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		private static int GetThumbnailDivideBy(long size)
		{
			if (size > 2_000_000)
				return 10;

			if (size > 1_000_000)
				return 5;

			if (size > 5_00_000)
				return 2;

			return 1;
		}
	}
}
