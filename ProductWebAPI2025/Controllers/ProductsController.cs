using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductModel;
using ProductWebAPI2025.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Sales Manager")]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct<Product> _repository;
        public ProductsController(IProduct<Product> repository)
        {
            _repository = repository; 
        }
        // Must decorate for swagger
        [HttpGet]
        //[Authorize]
        public IEnumerable<Product> Get()
        {
            return _repository.GetAll();
        }

        [HttpGet("values/id/{id:int}/name/{name}")]
        public dynamic Values(int id, string name)
        {
            return new { id, name };
        }

        [HttpPost("AddProduct/Reorder")]
        //[Authorize]

        public dynamic AddProductReorderLevel(ProductReorderViewModel vm)
        {
            if(vm == null)
            {
                return new { Message = "No data" };
            }
            var product = _repository.UpdateReorderLevel(vm.ProductID,vm.ReorderLevel);
            if(product == null)
            {
                return new { Message = "Product not found" };
            }
            return new { Message = "Reorder Level Updated", Product = product };
        }

        [HttpPost("AddProduct/New")]
        //[Authorize]

        public dynamic AddProduct(Product P)
        {
            if (P == null)
            {
                return new { Message = "No data" };
            }
            _repository.Add(P);
            return(P);
        }

    }


}
