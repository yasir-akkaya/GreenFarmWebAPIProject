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
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public WishlistController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: api/Wishlist
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Product>>> GetWishlistProductsByUserId()
        {

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            int userId = db.Users.FirstOrDefault(x => x.Email == userEmail).Id;

            int wishlistId = Convert.ToInt32(db.Wishlists.FirstOrDefault(x => x.UserId == userId)?.Id);

            if (wishlistId == 0)
            {
                return NotFound("Wishlist not found for the user");
            }

            var productIds = db.WishlistProducts.Where(wp => wp.WishlistId == wishlistId).Select(wp => wp.ProductId).ToList();

            var products = await db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound("No products found in the wishlist");
            }

            return Ok(products);
        }

        // POST: api/Wishlist
        [HttpPost]
        public async Task<bool> AddWishlistProduct([FromBody] WishlistProductRequestBody request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            int userId = db.Users.FirstOrDefault(x => x.Email == userEmail).Id;

            int wishlistId = Convert.ToInt32(db.Wishlists.FirstOrDefault(x => x.UserId == userId)?.Id);

            if (wishlistId == 0)
            {
                Wishlist entity = new Wishlist()
                {
                    UserId = userId
                };

                db.Wishlists.Add(entity);
                db.SaveChanges();
                WishlistProduct wp = new WishlistProduct()
                {
                    ProductId = request.ProductId,
                    WishlistId = request.WishlistId
                };
                
                db.WishlistProducts.Add(wp);
                db.SaveChanges();
                return true;
            }
            else
            {
                WishlistProduct wp = new WishlistProduct()
                {
                    ProductId = request.ProductId,
                    WishlistId = wishlistId
                };

                db.WishlistProducts.Add(wp);
                db.SaveChanges();
                return true;
            }
        }
        // DELETE: api/Wishlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlistProduct(int id)
        {
            if (db.WishlistProducts == null)
            {
                return NotFound();
            }
            var wishlistProduct = await db.WishlistProducts.Where(x => x.WishlistId == id).ToListAsync();
            if (wishlistProduct.Count == 0)
            {
                return NotFound();
            }
            else
            {

                db.WishlistProducts.RemoveRange(wishlistProduct);
                db.Wishlists.Remove(db.Wishlists.Find(id));
                await db.SaveChangesAsync();
            }

            return NoContent();
        }

    }
}
