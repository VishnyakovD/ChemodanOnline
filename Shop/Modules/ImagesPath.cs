using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Modules
{
    public interface IImagesPath
    {
        string GetImagesPath();
    }

    public class ImagesPath : IImagesPath
    {
        private readonly string _imagesPath;
        public ImagesPath(string imagePath)
        {
            _imagesPath = imagePath;
        }

        public string GetImagesPath()
        {
            return _imagesPath;
        }
    }
}