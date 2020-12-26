using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Application.AzureStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(IFormFile file);
    }
}
