using mahallAppAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imagerepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imagerepository = imageRepository;
        }

        public string DeleteImageByName(string imageName)
        {
            _imagerepository.DeleteImageByName(imageName);
            return "dönelim birşeyler";
        }
    }
}
