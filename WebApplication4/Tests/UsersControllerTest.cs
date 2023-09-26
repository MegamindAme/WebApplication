using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication4.Controllers;
using WebApplication4.DataDB;
using Xunit;
using Ts = System.Threading.Tasks;

namespace WebApplication4.Tests
{
    public class UsersControllerTest
    {
            public UsersControllerTest()
            {

            }

            [Fact]
            public async Ts.Task Getusers_ReturnsOkResultWithusers()
            {
                // Arrange
                var users = new List<Models.User> { new Models.User { ID = 1, Username = "name 1", Email =  "emali@gmail.com", Password = "password" } };
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users).Returns(MockDbSetHelper.CreateMockDbSet(users));
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.GetUsers();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var model = Assert.IsType<List<Models.User>>(okResult.Value);
                Assert.Single(model);
            }

            [Fact]
            public async Ts.Task GetTask_WithValidId_ReturnsUser()
            {
                // Arrange
                var userId = 1;
                var task = new Models.User { ID = userId, Username = "name 1", Email =  "emali@gmail.com", Password = "password" };
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users.FindAsync(userId)).ReturnsAsync(task);
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.GetUser(userId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var model = Assert.IsType<Models.User>(okResult.Value);
                Assert.Equal(userId, model.ID);
            }

            [Fact]
            public async Ts.Task GetTask_WithInvalidId_ReturnsNotFound()
            {
                // Arrange
                var userId = 1;
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users.FindAsync(userId)).ReturnsAsync((Models.User)null);
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.GetUser(userId);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }

            //[Fact]
            public async Ts.Task PutTask_WithValidId_ReturnsNoContent()
            {
                // Arrange
                var userId = 1;
                var task = new Models.User { ID = userId, Username = "name 1", Email =  "emali@gmail.com", Password = "password" };
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users.FindAsync(userId)).ReturnsAsync(task);
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.PutUser(userId, task);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Ts.Task PutTask_WithInvalidId_ReturnsBadRequest()
            {
                // Arrange
                var userId = 1;
                var task = new Models.User { ID = userId, Username = "name 1", Email =  "emali@gmail.com", Password = "password" };
                var mockContext = new Mock<TestApiContext>();
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.PutUser(userId + 1, task);

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }

            [Fact]
            public async Ts.Task PostTask_ValidTask_ReturnsCreatedAtActionResult()
            {
                // Arrange
                var task = new Models.User { ID = 1, Username = "name 1", Email =  "emali@gmail.com", Password = "password" };
                var mockContext = new Mock<TestApiContext>();
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.PostUser(task);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                Assert.Equal("GetTask", createdAtActionResult.ActionName);
            }

            [Fact]
            public async Ts.Task DeleteTask_WithValidId_ReturnsNoContent()
            {
                // Arrange
                var userId = 1;
                var task = new Models.User { ID = userId, Username = "name 1", Email =  "emali@gmail.com", Password = "password" };
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users.FindAsync(userId)).ReturnsAsync(task);
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.DeleteUser(userId);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Ts.Task DeleteTask_WithInvalidId_ReturnsNotFound()
            {
                // Arrange
                var userId = 1;
                var mockContext = new Mock<TestApiContext>();
                mockContext.Setup(c => c.Users.FindAsync(userId)).ReturnsAsync((Models.User)null);
                var controller = new UsersController(mockContext.Object);

                // Act
                var result = await controller.DeleteUser(userId);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
}
