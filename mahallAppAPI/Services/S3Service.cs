using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using mahallAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace mahallAppAPI.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service()
        {
            var credentials = new BasicAWSCredentials("AKIA6I7DV26CGFLSZK5O", "gHe4Jt6ITGxhJKejyxlnHrnat9sQ5Uaqr5QZwWzp");
            _client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.EUCentral1);
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName) == false)
                {
                    var putBacketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _client.PutBucketAsync(putBacketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }

            return new S3Response
            {
                Message = "Something went wrong!",
                Status = HttpStatusCode.InternalServerError
            };
        }
    }
}
