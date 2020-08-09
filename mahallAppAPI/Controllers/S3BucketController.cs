using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mahallAppAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
    [Route("api/v1/S3Bucket")]
    [ApiController]
    public class S3BucketController : ControllerBase
    {
        private readonly IS3Service _service;

        public S3BucketController(IS3Service service)
        {
            _service = service;
        }

        [HttpPost("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketname)
        {
            var response = await _service.CreateBucketAsync(bucketname);
            return Ok();
        }

        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketname)
        {
            await _service.UploadFileAsync(bucketname);

            return Ok();
        }

        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName)
        {
            await _service.GetObjectFromS3Async(bucketName);

            return Ok();
        }
    }
}
