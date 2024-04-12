using Minio;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Args;

namespace WebApi1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private const string BucketName = "firmware";

        public FileDownloadController(MinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        [HttpGet("{fileName}")]
        public async Task<ActionResult<string>> GetPresignedUrl(string fileName)
        {
            try
            {
                PresignedGetObjectArgs presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName)
                    .WithExpiry(3600);

                var url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
