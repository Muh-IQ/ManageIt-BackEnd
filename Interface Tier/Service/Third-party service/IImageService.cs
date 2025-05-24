using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Interface_Tier.Service
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<IEnumerable<string>> UploadRangeOfImagesAsync(IEnumerable<IFormFile> file);
        Task<string> UpdateImageAsync(IFormFile file, string publicId);
        Task<string> UpdateImageByUrlAsync(IFormFile file, string ImageURL);
    }
}
