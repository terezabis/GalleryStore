using GalleryStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryStore.Data
{
    public class GalleryStoreRepository : IGalleryStoreRepository
    {
        private readonly GalleryStoreContext _ctx;
        private readonly ILogger<GalleryStoreRepository> _logger;

        public GalleryStoreRepository(GalleryStoreContext ctx, ILogger<GalleryStoreRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public void UpdateEntity(object model)
        {
            _ctx.Update(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Include(u => u.User)
                    .ToList();
            }
            else
            {
                return _ctx.Orders
                    .Include(u => u.User)
                    .ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                    .Where(o => o.User.UserName == username)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Include(u => u.User)
                    .ToList();
            }
            else
            {
                return _ctx.Orders
                    .Where(o => o.User.UserName == username)
                    .Include(u => u.User)
                    .ToList();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");

                return _ctx.Products
                    .OrderBy(p => p.Size)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id && o.User.UserName == username)
                .FirstOrDefault();
        }

        public Order GetOrder(int id)
        {
            return _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public Product GetProduct(int id)
        {
            return _ctx.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }
    
        public IEnumerable<Product> GetProductsByArtistId(string artistId)
        {
            return _ctx.Products
                .Where(a => a.ArtId == artistId)
                .ToList();
        }


        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
