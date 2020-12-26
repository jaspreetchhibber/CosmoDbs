using PlantApp.Core.Photos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Application.Photos
{
    public interface IPhotoManager
    {
        Task<Photo> GetPhoto(string id);
        Task<IEnumerable<Photo>> GetPhotos();
        Task<string> AddPhoto(PhotoDto photo);
    }
}
