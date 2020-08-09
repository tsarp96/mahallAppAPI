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
using Amazon.S3.Transfer;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        private const string FilePath = "C:\\Users\\TUNA\\source\\repos\\mahallAppAPI\\mahallAppAPI\\amazon_test.txt";
        private const string UploadWithKeyName = "newFileNameForUpload";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                //Option1
                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                //Option2
                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                //Option3
                using (var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                }

                //Option4
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456, //6 MB
                    Key = AdvancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };

                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Messeage:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered on server. Messeage:'{0}' when writing an object", e.Message);
            }
        }

        public async Task GetObjectFromS3Async(string bucketName)
        {
            const string keyName = "amazon_test.txt";

            try
            {
                var request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                string responseBody;

                using (var response = await _client.GetObjectAsync(request)) 
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-Type"];

                    Console.WriteLine($"Object meta, Title: {title}");
                    Console.WriteLine($"Content type: {contentType}");

                    responseBody = reader.ReadToEnd();
                }

                var pathAndFileName = $"C:\\Users\\TUNA\\Desktop\\{keyName}";

                var createText = responseBody;

                //File.WriteAllText(pathAndFileName, createText);  no need for that ,  just for testing
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Messeage:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered on server. Messeage:'{0}' when writing an object", e.Message);
            }
        }
    }
}
