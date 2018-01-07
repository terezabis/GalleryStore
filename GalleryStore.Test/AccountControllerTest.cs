using GalleryStore.Controllers;
using GalleryStore.Data.Entities;
using GalleryStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace GalleryStore.Test
{
    public class AccountControllerTest
    {
        [Fact]
        public async Task LoginShouldReturnTrue()
        {
            //SignInManager<StoreUser> signInManager = new SignInManager<StoreUser>(null, null,null,null,null,null);
            //var result = await signInManager.PasswordSignInAsync("Admin", "Admin2@017!", false, false);

            //Assert.True(result.Succeeded);

            //LoginViewModel loginVm = new LoginViewModel { Username = "Admin", Password = "Admin2@017!", RememberMe = false };
            //AccountController accController = new AccountController 
        }
    }
}
