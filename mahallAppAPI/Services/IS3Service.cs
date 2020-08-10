using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using mahallAppAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace mahallAppAPI.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task UploadFileAsync(string bucketName, IFormFile file);
        Task GetObjectFromS3Async(string bucketName);
        Task DeleteFileFroms3Async(string fileName, string bucketName);
    }
}