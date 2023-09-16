using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Project_API_NH.Models;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_API_NH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly SalesNhContext _context;
        private readonly IConfiguration _configuration;

        public ProductController (SalesNhContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(int? limit, int? page)
        {
            //Thiết lập mặc định cho số sản phẩm ở trang một
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page -1)*limit);

            //Truy vấn danh sách product từ database
            var products = _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Typeproduct)
                .OrderBy("p => p.Id DESC")
                .Skip(offset)
                .Take((int)limit)
                .ToArray();

            return Ok();

        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult GetSingleProduct(int id)
        {
            //Truy vấn sản phẩm dựa trệ id kèm theo nhà cung cấp và loại sản phẩm
            var product = _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Supplier)
                .Include(p => p.Typeproduct)
                .First();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateAProduct(Product p)
        {
            //Thêm sản phẩm mới và lưu thay đổi
            _context.Products.Add(p);
            _context.SaveChanges();
            //Trả về sản phẩm vừa tạo
            return Created($"/get-by-id?id={p.Id}",p);
        }

        //update sản phẩm
        [HttpPut]
        public IActionResult UpdateProduct(Product p)
        {
            _context.Products.Update(p);
            _context.SaveChanges();

            return Ok("This product has been updated");
        }

        //Xóa sản phẩm theo ID
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            //Tìm sản phẩm dựa trên id
            var p = _context.Products.Find(id);

            if (p == null)
            {
                return NotFound();
            }
            //Xóa product khỏi databasse
            _context.Products.Remove(p);
            _context.SaveChanges();

            return Ok("This product has been removed");
        }
        //Tìm sản phẩm theo tên hoặc loại
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string? name, int? limit, int? page)
        {
            //Thiết lập mặc định cho số sản phẩm ở trang một
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) * limit);

            //Truy vấn sản phẩm dựa trên tên hoặc loại sản phẩm và thực hiện phân trang
            var searchProduct = _context.Products
                .Where(p => p.Name.Contains(name) || p.Typeproduct.Name.Contains(name))
                .Include(p => p.Supplier)
                .Include(p => p.Typeproduct)
                .OrderBy("p => p.Id DESC")
                .Skip(offset)
                .Take((int)limit)
                .ToArray();
            return Ok(searchProduct);
        }

    }
   
}

