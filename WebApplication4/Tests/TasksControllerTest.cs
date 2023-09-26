using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplication4.Controllers;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using WebApplication4.DataDB;
using Moq;
using WebApplication4.Models;
using Xunit;
using Microsoft.Build.Framework;
using Ts = System.Threading.Tasks;
using NuGet.Packaging.Signing;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Tests
{
    public class TasksControllerTest
    {
        public TasksControllerTest()
        {

        }

        [Fact]
        public async Ts.Task GetTasks_ReturnsOkResultWithTasks()
        {
            // Arrange
            var tasks = new List<Models.Task> { new Models.Task { ID = 1, Title = "Task 1" } };
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks).Returns(MockDbSetHelper.CreateMockDbSet(tasks));
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<List<Models.Task>>(okResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Ts.Task GetTask_WithValidId_ReturnsTask()
        {
            // Arrange
            var taskId = 1;
            var task = new Models.Task { ID = taskId, Title = "Task 1" };
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.GetTask(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<Models.Task>(okResult.Value);
            Assert.Equal(taskId, model.ID);
        }

        [Fact]
        public async Ts.Task GetTask_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 1;
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync((Models.Task)null);
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.GetTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //[Fact]
        public async Ts.Task PutTask_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var taskId = 1;
            var task = new Models.Task { ID = taskId, Title = "Task 1" };
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.PutTask(taskId, task);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Ts.Task PutTask_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var taskId = 1;
            var task = new Models.Task { ID = taskId, Title = "Task 1" };
            var mockContext = new Mock<TestApiContext>();
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.PutTask(taskId + 1, task);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Ts.Task PostTask_ValidTask_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var task = new Models.Task { ID = 1, Title = "Task 1" };
            var mockContext = new Mock<TestApiContext>();
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.PostTask(task);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTask", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Ts.Task DeleteTask_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var taskId = 1;
            var task = new Models.Task { ID = taskId, Title = "Task 1" };
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Ts.Task DeleteTask_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 1;
            var mockContext = new Mock<TestApiContext>();
            mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync((Models.Task)null);
            var controller = new TasksController(mockContext.Object);

            // Act
            var result = await controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    public static class MockDbSetHelper
    {
        public static DbSet<T> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockDbSet.Object;
        }
    }
}
