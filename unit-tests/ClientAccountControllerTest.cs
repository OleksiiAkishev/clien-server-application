using Microsoft.AspNetCore.Mvc;
using UniversityMgmtSystemClientConsuming.Controllers;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace UnitTestUniversityMgmtSystem
{
    [TestFixture]
    public class Tests
    {
        private AccountController _accountController;

        [SetUp]
        public void Setup()
        {
            _accountController = new AccountController(null);
        }

        [Test]
        public void Login_WithValidCredentials_ReturnsRedirectToActionResult()
        {
            // Arrange
            var user = new LoginVM
            {
                Email = "clark@email.com",
                Password = "&ihi0QshT86A"
            };

            // Act
            var result = _accountController.Login(user) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
    }
}