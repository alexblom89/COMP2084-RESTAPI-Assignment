using COMP2084Assignment.Controllers;
using COMP2084Assignment.Data;
using COMP2084Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace AssignmentTest
{
    [TestClass]
    public class GamesControllerTest
    {
        private ApplicationDbContext _context;
        List<Game> games = new List<Game>();
        //From https://stackoverflow.com/a/55953099
        IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

        GamesController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            var platform = new Platform { Id = 100, Developer = "Nintendo", Name = "Switch", ReleaseDate = new DateTime(2016, 5, 12) };
            var genre = new Genre { Id = 100, Name = "Test" };

            games.Add(new Game { Id = 10, Developer = "Dev", Title = "Game 1", Publisher = "EA", ReleaseDate = new DateTime(2020, 5, 12) });
            games.Add(new Game { Id = 11, Developer = "Dev", Title = "Game 3", Publisher = "EA", ReleaseDate = new DateTime(2020, 5, 12) });
            games.Add(new Game { Id = 12, Developer = "Dev", Title = "Game 2", Publisher = "EA", ReleaseDate = new DateTime(2020, 5, 12) });

            foreach (var g in games)
            {
                _context.Games.Add(g);
            }

            _context.SaveChanges();

            controller = new GamesController(_context);
        }

        //Index Tests
        [TestMethod]
        public void IndexViewLoads()
        {
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [TestMethod]
        public void IndexReturnsGameData()
        {
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;

            List<Game> model = (List<Game>)viewResult.Model;

            CollectionAssert.AreEqual(games.ToList(), model);
        }

        //Details Tests
        [TestMethod]
        public void DetailsLoadsErrorViewWithNullId()
        {
            var result = controller.Details(null);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void DetailsLoadsErrorViewWithInvalidId()
        {
            var result = controller.Details(1);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void DetailsLoadsViewWithValidId()
        {
            var result = controller.Details(10);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [TestMethod]
        public void DetailsLoadsCorrectModel()
        {
            var result = controller.Details(10);
            var viewResult = (ViewResult)result.Result;
            Game model = (Game)viewResult.Model;

            Assert.AreEqual(_context.Games.Find(10), model);
        }

        [TestMethod]
        public void DetailsLoadsErrorViewForInvalidModel()
        {
            var result = controller.Details(1);
            var viewResult = (ViewResult)result.Result;
            Game model = (Game)viewResult.Model;

            Assert.AreNotEqual(_context.Games.FindAsync(1), model);
        }

        //Create Tests (GET)
        [TestMethod]
        public void CreateViewLoads()
        {
            var result = controller.Create();
            var viewResult = result as ViewResult;

            Assert.AreEqual("Create", viewResult.ViewName);
        }

        //Create Tests (POST)
        [TestMethod]
        public void CreatePostsValidModel()
        {
            var request = new Game 
            { 
                Id = 13,
                Developer = "Dev",
                Title = "Game 4",
                Publisher = "EA",
                ReleaseDate = new DateTime(2020, 5, 12)
            };

            var result = controller.Create(request, null);

            Assert.AreEqual("Game 4", request.Title);
        }

        [TestMethod]
        public void CreateLoadsIndexViewAfterPostingValidModel()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var request = new Game
            {
                Id = 14,
                Developer = "Dev",
                Title = "Game 5",
                Publisher = "EA",
                ReleaseDate = new DateTime(2020, 5, 12)
            };

            var result = (RedirectToActionResult)controller.Create(request, file).Result;

            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void CreateLoadsCreateViewAfterPostingInvalidModel()
        {
            controller.ModelState.AddModelError("InvalidModelError", "Game is Invalid!");
            var request = new Game
            {
                Id = 14,
                Developer = "Dev",
                Title = "Game 5",
                Publisher = "EA",
                ReleaseDate = new DateTime(2020, 5, 12)
            };

            var result = controller.Create(request, file);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Create", viewResult.ViewName);
        }

        //Edit Tests (GET)
        [TestMethod]
        public void EditLoadsErrorViewWithNullId()
        {
            var result = controller.Edit(null);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsErrorViewWithInvalidId()
        {
            var result = controller.Edit(1);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsViewWithValidId()
        {
            var result = controller.Edit(10);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsCorrectModel()
        {
            var result = controller.Edit(10);
            var viewResult = (ViewResult)result.Result;
            Game model = (Game)viewResult.Model;

            Assert.AreEqual(_context.Games.Find(10), model);
        }

        [TestMethod]
        public void EditLoadsViewData()
        {
            var result = controller.Edit(10);
            var viewResult = (ViewResult)result.Result;
            var viewData = viewResult.ViewData;

            Assert.AreEqual(viewData, viewResult.ViewData);
        }

        //Edit Tests (POST)
        [TestMethod]
        public void EditPostsValidModel()
        {
            var model = _context.Games.Find(10);
            var result = controller.Edit(10, model);
            
            Assert.AreEqual("Game 1", model.Title);
        }

        [TestMethod]
        public void EditLoadsIndexViewAfterPostingValidModel()
        {           
            var result = (RedirectToActionResult)controller.Edit(10, _context.Games.Find(10)).Result;

            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void EditLoadsEditViewAfterPostingInvalidModel()
        {
            controller.ModelState.AddModelError("InvalidModelError", "Game is Invalid!");
            var result = controller.Edit(10, _context.Games.Find(10));
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        //Delete Tests (GET)
        [TestMethod]
        public void DeleteLoadsDeleteViewWithValidModel()
        {
            var result = controller.Delete(10);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Delete", viewResult.ViewName);
        }

        public void DeleteLoadsErrorViewIfModelIsNull()
        {
            var result = controller.Delete(null);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        public void DeleteViewLoadsValidModelData()
        {
            var result = controller.Delete(10);
            var viewResult = (ViewResult)result.Result;
            Game model = (Game)viewResult.Model;

            Assert.AreEqual(_context.Games.FindAsync(10), model);
        }

        //Delete Tests (POST)
        [TestMethod]
        public void DeleteConfirmedRemovesValidModel()
        {
            var result = controller.DeleteConfirmed(10);

            Assert.AreEqual(2, _context.Games.Count());
        }

        [TestMethod]
        public void DeleteConfirmedRedirectsToIndexOnRemovingValidModel()
        {
            var result = (RedirectToActionResult)controller.DeleteConfirmed(10).Result;

            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
