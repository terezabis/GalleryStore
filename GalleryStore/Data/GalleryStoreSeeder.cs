using GalleryStore.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryStore.Data
{
    public class GalleryStoreSeeder
    {
        private readonly GalleryStoreContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IServiceProvider _serviceProvider;

        public GalleryStoreSeeder(GalleryStoreContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager, IServiceProvider serviceProvider)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();
            

            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = _serviceProvider.GetRequiredService<UserManager<StoreUser>>();
            string[] roleNames = { "Admin", "Customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 2
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new StoreUser
            {
                UserName = "Admin",
                Email = "admin@store.com"
            };

            string userPWD = "Admin2@017!";

            var adminUser = await _userManager.FindByEmailAsync("admin@store.com");
            if (adminUser == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role : Question 3
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }

            var user = await _userManager.FindByEmailAsync("test@store.com");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Pesho",
                    LastName = "Peshov",
                    UserName = "test@store.com",
                    Email = "test@store.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result.Succeeded)
                {
                    //here we tie the new user to the role : Question 3
                    await UserManager.AddToRoleAsync(user, "Customer");

                }
                else if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_ctx.Products.Any())
            {
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "12345",
                    User = user,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product=products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _ctx.Orders.Add(order);

                _ctx.SaveChanges();

            }
        }
    }
}
