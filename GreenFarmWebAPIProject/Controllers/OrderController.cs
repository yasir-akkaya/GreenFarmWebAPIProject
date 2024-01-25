using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenFarmWebAPIProject.Models;
using GreenFarmWebAPIProject.Models.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

namespace GreenFarmWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public OrderController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Product>>> GetOrderProductsByUserId()
        {

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            int userId = db.Users.FirstOrDefault(x => x.Email == userEmail).Id;

            int OrderId = Convert.ToInt32(db.Orders.FirstOrDefault(x => x.UserId == userId)?.Id);

            if (OrderId == 0)
            {
                return NotFound("Order not found for the user");
            }

            var productIds = db.OrderProducts.Where(wp => wp.OrderId == OrderId).Select(wp => wp.ProductId).ToList();

            var products = await db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            

            return Ok(products);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<bool> AddOrderProduct([FromBody] OrderProductRequestBody request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            int userId = db.Users.FirstOrDefault(x => x.Email == userEmail).Id;

            int OrderId = Convert.ToInt32(db.Orders.FirstOrDefault(x => x.UserId == userId)?.Id);

            if (OrderId == 0)
            {
                Order entity = new Order()
                {
                    UserId = userId,
                    CreationDate= DateTime.Now
                };

                db.Orders.Add(entity);
                db.SaveChanges();
                OrderProduct wp = new OrderProduct()
                {
                    ProductId = request.ProductId,
                    OrderId = request.OrderId
                };

                db.OrderProducts.Add(wp);
                db.SaveChanges();
                return true;
            }
            else
            {
                OrderProduct wp = new OrderProduct()
                {
                    ProductId = request.ProductId,
                    OrderId = OrderId

                };

                db.OrderProducts.Add(wp);
                db.SaveChanges();
                return true;
            }

        }

        

    }
}
