using PlantApp.Application.AzureStorage;
using PlantApp.Core.CosmoDbServices;
using PlantApp.Core.Photos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlantApp.Application.Photos
{
    public class PhotoManager : IPhotoManager
    {
        private ICosmosDbService _service;
        private IBlobStorageService _blobStorage;

        public PhotoManager(ICosmosDbService service, IBlobStorageService blobStorage)
        {
            _service = service;
            _blobStorage = blobStorage;
        }

        public async Task<string> AddPhoto(PhotoDto photos)
        {
            try
            {
                Photo photo = new Photo();
                photo.Id = Guid.NewGuid().ToString();
                List<PhotoMetaData> photoMetaDataList = new List<PhotoMetaData>();
                foreach (var item in photos.Photos)
                {                 
                    var url = await _blobStorage.UploadFileToBlobAsync(item);
                    PhotoMetaData photoMetaData = new PhotoMetaData();
                    photoMetaData.Url = url;
                    photoMetaDataList.Add(photoMetaData);
                }
                photo.PhotoMetaData = photoMetaDataList;
                return await _service.AddItemAsync(photo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Task<Photo> GetPhoto(string id)
        {
            var result = _service.GetItemAsync(id);
            return result;
        }

        public Task<IEnumerable<Photo>> GetPhotos()
        {
            var result = _service.GetItemsAsync("SELECT * FROM c");
            return result;
        }
    }
}
