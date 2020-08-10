using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI.Interfaces
{
    public interface IImageService
    {
        public string DeleteImageByName(string imageName);
    }
}
