using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Project_API_NH.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_API_NH.Controllers
{
    [ApiController]
    [Route("api/orderitems")]
    public class OrderItemController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SalesNhContext _context;
        public OrderItemController(IConfiguration configuration, SalesNhContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //Gọi tất cả các Orderitems đơn hàng
        [HttpGet]
        public IActionResult GetOrderItemsAll(int? limit, int? page)
        {
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) + limit);

            //Truy vấn danh sách từ database
            var ordersItem = _context.Orderitems
                .OrderBy("p => p.Id DESC")
                .Skip(offset)
                .Take((int)limit)
                .ToArray();

            return Ok(ordersItem);
        }

        // Lấy OrderItem theo id
        [HttpGet]
        [Route("get-by-id")]
        public IActionResult GetSingleOrderItems(int id)
        {
            var orderItem = _context.Orderitems
                .Where(p => p.Id == id)
                .First();
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        // Thêm mới một Orderitems
        [HttpPost]
        public IActionResult CreateOrderItems(Orderitem orderItem)
        {
            _context.Orderitems.Add(orderItem);
            _context.SaveChanges();

            //Trả về thông tin vừa tạo
            return Created($"get-by-id?id = {orderItem.Id}", orderItem);
        }

        //Cập nhật sản phẩm
        [HttpPut]
        public IActionResult UpdateOrderItems(Orderitem order)
        {
            _context.Orderitems.Update(order);
            _context.SaveChanges();


            return NoContent();
        }

        //Xóa đơn hàng
        [HttpDelete]
        public IActionResult DropOrderItems(int id)
        {
            //Tìm sản phẩm theo id
            var order = _context.Orderitems.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            //Xóa và lưu thay đổi
            _context.Orderitems.Remove(order);
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

