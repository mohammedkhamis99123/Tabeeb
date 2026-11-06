using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Infrastructure.Services
{
    public  class FileService : IFileService
    {
        private readonly IWebHostEnvironment webHost;

        public FileService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }
        public  string UploadFile(IFormFile File, string folderName)
        {
            var path = Path.Combine(webHost.WebRootPath,"Uploads","Images", folderName);

            var FileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
            var finalPath = Path.Combine(path, FileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }

            return folderName+"/"+FileName;
        }

       

        public void DeleteFile(string Path)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
    }
}
