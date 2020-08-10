using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mahallAppAPI.Interfaces;
using mahallAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
    [Route("api/v1/S3Bucket")]
    [ApiController]
    public class S3BucketController : ControllerBase
    {
        private readonly IS3Service _service;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;

        public S3BucketController(IS3Service service, IUserRepository userRepository, IImageService imageService)
        {
            _service = service;
            _userRepository = userRepository;
            _imageService = imageService;
        }

        [HttpPost("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketname)
        {
            var response = await _service.CreateBucketAsync(bucketname);
            return Ok();
        }

        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName)
        {
            await _service.GetObjectFromS3Async(bucketName);

            return Ok();
        }

        [HttpPost]
        [CustomAuthorization]
        [Route("DeleteFile")]
        public async Task<IActionResult> DeleteFileFromS3Async([FromBody] DeleteFileRequest request)
        {

            //TODO: Kullanıcının silmek istediği ID kendisine ait mi?

            //TODO: Delete From Database
            _imageService.DeleteImageByName(request.filename);

            string filename = request.filename;
            await _service.DeleteFileFroms3Async(filename, "hadibeoglum");
            return Ok();
        }
    }
}
