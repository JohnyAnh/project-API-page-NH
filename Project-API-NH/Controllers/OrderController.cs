using System;
using System.Collections.Generic;
using System.Linq;
using Project_API_NH.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_API_NH.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SalesNhContext _context;


        public OrderController (IConfiguration configuration, SalesNhContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //Gọi tất cả các đơn hàng
        [HttpGet]
        public IActionResult GetOrderAll(int? limit, int? page)
        {
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) + limit);

            //Truy vấn danh sách từ database
            var orders = _context.Orders
                .OrderBy("p => p.Id DESC")
                .Skip(offset)
                .Take((int)limit)
                .ToArray();

            return Ok(orders);
        }

        // Lấy Order theo id
        [HttpGet]
        [Route("get-by-id")]
        public IActionResult GetSingleOrder(int id)
        {
            var order = _context.Orders
                .Where(p => p.Id == id)
                .First();
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // Thêm mới một đơn hàng
        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            //Trả về thông tin vừa tạo
            return Created($"get-by-id?id = {order.Id}", order);
        }

        //Cập nhật sản phẩm
        [HttpPut]
        public IActionResult UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();

            
            return NoContent();
        }

        //Xóa đơn hàng
        [HttpDelete]
        public IActionResult DropOrder(int id)
        {
            //Tìm sản phẩm theo id
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            //Xóa và lưu thay đổi
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }

        //Tìm kiếm đơn hàng
        [HttpGet]
        [Route("search")]
        public IActionResult GetType(int? id, int? limit, int? page)
        {
            //Thiết lập mặc định cho số sản phẩm ở trang một
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) * limit);

            //Truy vấn dựa trên id sản phẩm
            var order = _context.Orders
                .Where(o => o.Id == id)
                .OrderByDescending(o => o.Id)
                .Skip(offset)
                .Take((int)limit)
                .ToArray();


            return Ok(order);
        }

    }
}

