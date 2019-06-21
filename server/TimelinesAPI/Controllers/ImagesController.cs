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

				var localFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "uploadedImage");
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				var filePath = Path.Combine(directory, localFileName);

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

				//if (new FileInfo(filePath).Length > 300000)
				//{
				//	var resized = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "uploadedImage",
				//		Guid.NewGuid() + Path.GetExtension(file.FileName));
				//	GetThumImage(filePath, 20000, 2, resized);

				//	System.IO.File.Delete(filePath);
				//	filePath = resized;
				//}

				var path = await _blobStorage.UploadImageAsync(MockUser.Username, folder, filePath);

				System.IO.File.Delete(filePath);
				if (path == null)
					throw new Exception("Upload to Azure failed.");

				return Ok(new { url = path });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
				using (Bitmap sourceImage = new Bitmap(sourceFile))
				{
					ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
					Encoder myEncoder = Encoder.Quality;
					EncoderParameters myEncoderParameters = new EncoderParameters(1);
					EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
					myEncoderParameters.Param[0] = myEncoderParameter;
					float xWidth = sourceImage.Width;
					float yWidth = sourceImage.Height;
					Bitmap newImage = new Bitmap((int) (xWidth / multiple), (int) (yWidth / multiple));
					using (Graphics g = Graphics.FromImage(newImage))
					{
						g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
					}

					newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
					return true;
				}
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
