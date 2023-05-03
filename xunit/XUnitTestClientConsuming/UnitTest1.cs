using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using UniversityMgmtSystemClientConsuming.Controllers;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace XUnitTestClientConsuming
{
    public class UnitTest1
    {

        [Fact]
        public async Task Register_ValidInput_ReturnsViewWithSuccessMessage()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);
            var controller = new AccountController(mockConfig.Object, mockHttpClientFactory.Object);
            var registerVM = new RegisterVM
            {
                FullName = "Bob",
                Email = "bob@email.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            // Setup mock response
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Created);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await controller.Register(registerVM) as ViewResult;
            var viewName = result.ViewName;
            var successMessage = result.ViewData["Success"] as string;

            // Assert
            Assert.Equal("RegisterCompleted", viewName);
            Assert.Equal("Registration was successful! Please log in.", successMessage);
        }

        [Fact]
        public async Task Register_ForbiddenInput_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);
            var controller = new AccountController(mockConfig.Object, mockHttpClientFactory.Object);
            var registerVM = new RegisterVM
            {
                FullName = "Bob",
                Email = "bob@email.com",
                Password = "&ihi0QshT86A",
                ConfirmPassword = "&ihi0QshT86A"
            };

            // Setup mock response
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await controller.Register(registerVM) as ViewResult;
            var viewName = result.ViewName;
            var errorMessage = result.ViewData["Error"] as string;

            // Assert
            Assert.Equal("Register", viewName);
            Assert.Equal("User with these credentials already exist!", errorMessage);
        }

    }
}