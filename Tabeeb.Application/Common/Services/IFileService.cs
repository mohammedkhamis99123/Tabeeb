using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Services
{
    public  interface IFileService
    {
        string UploadFile(IFormFile File, string Folder);

        void DeleteFile(string Path);
    }
}
