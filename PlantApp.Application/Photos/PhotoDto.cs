using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Application.Photos
{
    public class PhotoDto
    {
        public List<IFormFile> Photos { get; set; }

        public string Comments { get; set; }
    }
}
