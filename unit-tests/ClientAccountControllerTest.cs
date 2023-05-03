using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using UniversityMgmtSystemClientConsuming.Controllers;
using UniversityMgmtSystemClientConsuming.Models;
using UniversityMgmtSystemClientConsuming.ViewModels;
using Xunit;

namespace UnitTestUniversityMgmtSystem
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsViewWithSuccessMessage()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(mockHttpClient.Object);
            var controller = new AccountController(mockConfig.Object, mockHttpClientFactory.Object);
            var registerVM = new RegisterVM { /* set valid properties */ };

            // Setup mock response
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Created);
            mockHttpClient.Setup(h => h.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(responseMessage);

            // Act
            var result = await controller.Register(registerVM) as ViewResult;
            var viewName = result.ViewName;
            var tempData = controller.TempData["Success"];

            // Assert
            Assert.AreEqual("RegisterCompleted", viewName);
            Assert.AreEqual("Registration was successful! Please log in.", tempData);
        }
    }
}