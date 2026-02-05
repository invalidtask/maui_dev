using FluentAssertions;
using Moq;
using VaughnLive.Services;
using VaughnLive.Models;
using Xunit;
using System.Net.Http;
using Microsoft.Extensions.Http;

namespace VaughnLive.Tests.Services;

public class ApiServiceTests
{
    [Fact]
    public void Constructor_ShouldNotThrow()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient("VaughnLiveApi"))
            .Returns(new HttpClient());

        // Act
        var action = () => new ApiService(httpClientFactoryMock.Object);

        // Assert
        action.Should().NotThrow();
    }
}
