using JwtTokenWithIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public ProductsController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult GetAll()
        {
            List<Products> productList = new List<Products>();

            foreach (var item in _context.Products)
            {
                productList.Add(item);
            }

            return Ok(productList);
        }
    }
}
