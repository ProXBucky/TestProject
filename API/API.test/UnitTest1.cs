using API.Controllers;
using API.Data;
using API.Helper.Factory;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace API.test
{
    //public class MathService
    //{
    //    public int Add(int a, int b) => a + b;
    //    public int Subtract(int a, int b) => a - b;
    //    public int Multiply(int a, int b) => a * b;
    //}

    public class UnitTest1
    {
        //private readonly MathService _mathService;

        //public UnitTest1()
        //{
        //    _mathService = new MathService();
        //}

        //[Fact]
        //public void Add_ShouldReturnCorrectSum()
        //{
        //    // Arrange
        //    int a = 3, b = 5;

        //    // Act
        //    int result = _mathService.Add(a, b);

        //    // Assert
        //    Assert.Equal(8, result);
        //}

        //[Fact]
        //public void Subtract_ShouldReturnCorrectDifference()
        //{
        //    // Arrange
        //    int a = 10, b = 4;

        //    // Act
        //    int result = _mathService.Subtract(a, b);

        //    // Assert
        //    Assert.Equal(6, result);
        //}

        //[Fact]
        //public void Multiply_ShouldReturnCorrectProduct()
        //{
        //    // Arrange
        //    int a = 7, b = 5;

        //    // Act
        //    int result = _mathService.Multiply(a, b);

        //    // Assert
        //    Assert.Equal(42, result);
        //}

        //[Fact]
        //public async Task GetAuthHistory_ReturnsCorrectUser()
        //{
        //    var options = new DbContextOptionsBuilder<DPContext>()
        //                    .UseInMemoryDatabase(databaseName: "TestDb")
        //                    .Options;

        //    using var context = new DPContext(options);
        //    var testUser = new AppUser { Id = "test_user_id", UserName = "TestUser" };
        //    context.AppUsers.Add(testUser);
        //    await context.SaveChangesAsync();

        //    var idField = typeof(AuthController).GetField("id", BindingFlags.Static | BindingFlags.NonPublic);
        //    idField.SetValue(null, testUser.Id);

        //    var jwtOptions = Options.Create(new JwtIssuerOptions
        //    {
        //        ValidFor = TimeSpan.FromMinutes(30)
        //    });

        //    var controller = new AuthController(
        //        userManager: null,
        //        mapper: null,
        //        context: context,
        //        jwtFactory: null,
        //        jwtOptions: jwtOptions
        //    );

        //    // Act: Gọi method GetAuthHistory
        //    var result = await controller.GetAuthHistory();

        //    // Assert: Kiểm tra kết quả trả về
        //    var actionResult = Assert.IsType<ActionResult<AppUser>>(result);
        //    var returnedUser = actionResult.Value;
        //    Assert.NotNull(returnedUser);
        //    Assert.Equal(testUser.Id, returnedUser.Id);
        //    Assert.Equal(testUser.UserName, returnedUser.UserName);
        //}

        [Fact]
        public async Task GetAuthHistory_ReturnsCorrectUser()
        {
            var options = new DbContextOptionsBuilder<DPContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb_ValidUser")
                            .Options;

            using var context = new DPContext(options);
            var testUser = new AppUser { Id = "test_user_id", UserName = "TestUser" };
            context.AppUsers.Add(testUser);
            await context.SaveChangesAsync();

            var idField = typeof(AuthController)
                            .GetField("id", BindingFlags.Static | BindingFlags.NonPublic);
            idField.SetValue(null, testUser.Id);

            var jwtOptions = Options.Create(new JwtIssuerOptions
            {
                ValidFor = TimeSpan.FromMinutes(30)
            });

            var controller = new AuthController(
                userManager: null,
                mapper: null,
                context: context,
                jwtFactory: null,
                jwtOptions: jwtOptions
            );

            var result = await controller.GetAuthHistory();

            var actionResult = Assert.IsType<ActionResult<AppUser>>(result);
            var returnedUser = actionResult.Value;
            Assert.NotNull(returnedUser);
            Assert.Equal(testUser.Id, returnedUser.Id);
            Assert.Equal(testUser.UserName, returnedUser.UserName);
        }

        [Fact]
        public async Task GetAuthHistory_ReturnsNull_WhenIdIsNull()
        {
            var options = new DbContextOptionsBuilder<DPContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb_IdNull")
                            .Options;

            using var context = new DPContext(options);

            var jwtOptions = Options.Create(new JwtIssuerOptions
            {
                ValidFor = TimeSpan.FromMinutes(30)
            });

            var controller = new AuthController(
                userManager: null,
                mapper: null,
                context: context,
                jwtFactory: null,
                jwtOptions: jwtOptions
            );

            var result = await controller.GetAuthHistory();
            Assert.Null(result.Value);
        }
        
        [Fact]
        public async Task GetAuthHistory_ReturnsNull_WhenUserDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<DPContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb_UserNotExist")
                            .Options;

            using var context = new DPContext(options);

            var idField = typeof(AuthController)
                            .GetField("id", BindingFlags.Static | BindingFlags.NonPublic);
            idField.SetValue(null, "non_existent_user_id");

            var jwtOptions = Options.Create(new JwtIssuerOptions
            {
                ValidFor = TimeSpan.FromMinutes(30)
            });

            var controller = new AuthController(
                userManager: null,
                mapper: null,
                context: context,
                jwtFactory: null,
                jwtOptions: jwtOptions
            );

            var result = await controller.GetAuthHistory();
            Assert.Null(result.Value);
        }


    }

}
