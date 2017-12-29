using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleryStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GalleryStore.Data.Entities;

namespace GalleryStore.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IGalleryStoreRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IGalleryStoreRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()  
        {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }
    }
}