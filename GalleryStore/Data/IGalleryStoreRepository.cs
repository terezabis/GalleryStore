using GalleryStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryStore.Data
{
    public interface IGalleryStoreRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        IEnumerable<Product> GetProductsByArtistId(string artistID);
        Product GetProduct(int id);

        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(string username, int id);
        Order GetOrder(int id);
        void AddEntity(object model);
        void UpdateEntity(object model);

        bool SaveAll();
    }
}
