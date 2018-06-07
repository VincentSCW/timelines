using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

		[HttpPost("upload")]
		public async Task<IActionResult> UploadImage()
		{
			try
			{
				var file = HttpContext.Request.Form.Files["file"];

				var localFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "uploadedImage",
					localFileName);

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

				if (new FileInfo(filePath).Length > 300000)
				{
					var resized = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "uploadedImage",
						Guid.NewGuid() + Path.GetExtension(file.FileName));
					GetThumImage(filePath, 20000, 2, resized);

					System.IO.File.Delete(filePath);
					filePath = resized;
				}

				await _blobStorage.UploadImageAsync(filePath);

				System.IO.File.Delete(filePath);

				return Ok(new {url = "http://test"});
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
