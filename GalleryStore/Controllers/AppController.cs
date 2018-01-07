using Microsoft.AspNetCore.Mvc;
using GalleryStore.Data;
using GalleryStore.ViewModels;
using GalleryStore.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GalleryStore.Data.Entities;
using AutoMapper;
using System;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleryStore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IGalleryStoreRepository _repository;
        private readonly IMapper _mapper;

        public AppController(IMailService mailService, IGalleryStoreRepository repository
            , IMapper mapper)
        {
            _mailService = mailService;
            _repository = repository;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();

        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactVewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("teresitka_91@abv.bg"
                    , model.Subject
                    , $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            else
            {
                //show error
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }

        [Authorize]
        public IActionResult Product(int productId)
        {
            var result = _repository.GetProduct(productId);
            ViewBag.Product = result;
            return View("Product");

        }

        [Authorize]
        public IActionResult Buy(int productId)
        {
            IEnumerable<Order> orders = _repository.GetAllOrdersByUser(User.Identity.Name, true);
            Product product = _repository.GetProduct(productId);

            if (orders.Count() == 0)
                return RedirectToAction("Shop");

            Order order = orders.First();
            OrderItem orderItem = null;

            foreach (OrderItem currentOrderItem in order.Items)
            {
                if (currentOrderItem.Product.Id == product.Id) {
                    orderItem = currentOrderItem;
                    currentOrderItem.Quantity++;
                }
            }

            if (orderItem == null)
            {
                orderItem = new OrderItem();
                orderItem.Order = order;
                orderItem.Product = product;
                orderItem.Quantity = 1;
                orderItem.UnitPrice = product.Price;
                order.Items.Add(orderItem);
            }

            _repository.UpdateEntity(order);
            if (_repository.SaveAll())
            {
                return RedirectToAction("Order", new { orderId = order.Id });
            }

            return RedirectToAction("Shop");
        }

        [Authorize]
        public IActionResult DeleteProduct(int productId)
        {
            IEnumerable<Order> orders = _repository.GetAllOrdersByUser(User.Identity.Name, true);
            Product product = _repository.GetProduct(productId);

            if (orders.Count() == 0)
                return RedirectToAction("Shop");

            Order order = orders.First();
            OrderItem orderItemToDelete = null;

            foreach (OrderItem currentOrderItem in order.Items)
            {
                if (currentOrderItem.Product.Id == product.Id)
                {
                    orderItemToDelete = currentOrderItem;
                    
                }
            }

            if (orderItemToDelete != null)
                order.Items.Remove(orderItemToDelete);

            _repository.UpdateEntity(order);
            if (_repository.SaveAll())
            {
                return RedirectToAction("Order", new { orderId = order.Id });
            }

            return RedirectToAction("Shop");
        }

        [Authorize]
        public IActionResult Orders(bool includeItems)
        {
            var results = _repository.GetAllOrdersByUser(User.Identity.Name, includeItems); ;
            if (User.IsInRole("Admin"))
            {
                results = _repository.GetAllOrders(includeItems);
            }
            else if (User.IsInRole("Customer"))
            { 
                results = _repository.GetAllOrdersByUser(User.Identity.Name, includeItems);
            }

            return View("Orders", results);
        }

        [Authorize]
        public IActionResult Order(int orderId)
        {
            var result = _repository.GetOrder(orderId);

           // result.User.
               // User.Identity.Name

            ViewBag.Order = result;
            //ViewBag.Total = result.Items.Quantity;

            return View("Order");
        }

        public IActionResult Artist(string artistId)
        {
            var result = _repository.GetProductsByArtistId(artistId);
            
            ViewBag.Order = result;

            return View("Order");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int productId)
        {
            var product = _repository.GetProduct(productId);

            ProductViewModel pvm = _mapper.Map<Product, ProductViewModel>(product);

            return View("ProductEdit", pvm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(ProductViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                //Save
                Product p = _repository.GetProduct(pvm.ProductId);
                p = _mapper.Map<ProductViewModel, Product>(pvm, p);
                _repository.UpdateEntity(p);
                _repository.SaveAll();

                return RedirectToAction("Product", new { productId = p.Id });
            }

            return View("ProductEdit", pvm);
        }
    }
}
