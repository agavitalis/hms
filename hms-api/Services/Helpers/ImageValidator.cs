using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Helpers
{
    public static class ImageValidator
    {
      
        public static bool FileSize(IConfiguration config, long fileSize)
        {
            if(fileSize > config.GetValue<long>("FileSizeLimit"))
            {
                return false;
            }

            return true;
        }

        public static bool Filetype(string fileExtension)
        {
            string[] options = new string[5];
            options[0] = ".txt";
            options[1] = ".pdf";
            options[2] = ".png";
            options[3] = ".jpg";
            options[4] = ".jpeg";

            var ext = fileExtension;
            if (string.IsNullOrEmpty(ext) || !options.Contains(ext))
            {
                return false;
            }
            return true;
        }


    }
}
