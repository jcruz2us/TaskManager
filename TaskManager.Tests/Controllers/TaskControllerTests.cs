using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Data;
using Moq;
using TaskManager.Controllers;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace TaskManager.Tests.Controllers
{
    [TestClass]
    public class TaskControllerTests
    {
        Mock<IRepository<Task>> _fake;
        IRepository<Task> _repository;
        TaskController _controller;

        [TestInitialize]
        public void Setup()
        {
            var tasks = new List<Task> { 
                new Task{ Id = 1, Description = "Task 1", IsCompleted = false }, 
                new Task{ Id = 2, Description = "Task 2", IsCompleted = false } 
            };
            _fake = new Mock<IRepository<Task>>();
            _fake.Setup(x => x.GetAll()).Returns(tasks);
            _repository = _fake.Object;
            _controller = new TaskController(_repository);
        }

        [TestMethod]
        public void Index_ReturnView()
        {
            //ARRANGE
            
            //ACT
            ViewResult result = _controller.Index();

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create_ReturnsAPartialView()
        {
            //ARRANGE

            //ACT:
            PartialViewResult result = _controller.Create();

            //ASSERT: Verify Add was called
            Assert.IsNotNull(result);
        }

        
        [TestMethod]
        public void Create_WhenAddingAValidTaskIsSuccessful_ReturnsToTaskIndex()
        {

            //ARRANGE: Create a mock that expectcs a call to Repository.Add
            var newTask = new Task { Id = 1, Description = "Task 1", IsCompleted = false };
            var mock = new Mock<IRepository<Task>>();
            mock.Setup(x => x.Add(newTask));
            var repository = mock.Object;

            var controller = new TaskController(repository);

            //ACT:
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(newTask);

            //ASSERT:
            Assert.IsNotNull(result);
            Assert.AreEqual("Task", result.RouteValues["Controller"]);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void Delete_WhenSuccessful_RedirectsToTasksIndex()
        {
            //ARRANGE
            var controller = new TaskController(_repository);

            //ACT
            var result = (RedirectToRouteResult)controller.Delete(1);

            //ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual("Task", result.RouteValues["Controller"]);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void Edit_ReturnsPartialView()
        {
            //ACT
            var result = (PartialViewResult)_controller.Edit(1);

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_WhenUpdatingIsSuccessful_RedirectsToTasksIndex()
        {
            //ARRANGE
            var task = new Task { Id = 1, Description = "Updated description", IsCompleted = true };

            //ACT
            var result = (RedirectToRouteResult)_controller.Edit(task);

            //ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual("Task", result.RouteValues["Controller"]);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
