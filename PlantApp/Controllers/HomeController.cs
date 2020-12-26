using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlantApp.Application.Photos;
using PlantApp.Core.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IPhotoManager _manager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IPhotoManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> GetPhoto(string id)
        {
            var result = await _manager.GetPhoto(id);
            return Ok(result);

        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var result= await _manager.GetPhotos();
            return Ok(result);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> AddPhotos([FromForm]PhotoDto photo)
        {
            await _manager.AddPhoto(photo);
            return Ok();

        }
    }
}
