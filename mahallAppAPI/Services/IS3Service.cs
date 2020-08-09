using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using mahallAppAPI.Models;
using System;
using System.Threading.Tasks;

namespace mahallAppAPI.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
    }
}