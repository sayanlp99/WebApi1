using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace WebApi1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private const string BucketName = "firmware";

        public FileUploadController(MinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "File is empty" });

                BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
                bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(BucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs);
                }

                PutObjectArgs putObjectArgs = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(file.FileName)
                    .WithStreamData(file.OpenReadStream())
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType);
                await _minioClient.PutObjectAsync(putObjectArgs);

                return Ok(new { message = "File uploaded successfully" });
            }
            catch (MinioException ex)
            {
                return StatusCode(500, new { error = $"Minio error: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
