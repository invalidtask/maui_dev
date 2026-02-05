using FluentAssertions;
using Moq;
using VaughnLive.Services;
using VaughnLive.ViewModels;
using VaughnLive.Models;
using Xunit;

namespace VaughnLive.Tests.ViewModels;

public class HomeViewModelTests
{
    private readonly Mock<IApiService> _apiServiceMock;
    private readonly HomeViewModel _viewModel;

    public HomeViewModelTests()
    {
        _apiServiceMock = new Mock<IApiService>();
        _viewModel = new HomeViewModel(_apiServiceMock.Object);
    }

    [Fact]
    public async Task InitializeAsync_LoadsAllData()
    {
        // Arrange
        var featuredStreams = new List<Stream>
        {
            new() { Id = "1", Title = "Featured Stream", IsLive = true }
        };
        var liveStreams = new List<Stream>
        {
            new() { Id = "2", Title = "Live Stream", IsLive = true }
        };
        var categories = new List<Category>
        {
            new() { Id = "1", Name = "Gaming" }
        };

        _apiServiceMock.Setup(x => x.GetFeaturedStreamsAsync())
            .ReturnsAsync(featuredStreams);
        _apiServiceMock.Setup(x => x.GetLiveStreamsAsync(null, 1, 20))
            .ReturnsAsync(liveStreams);
        _apiServiceMock.Setup(x => x.GetCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        _viewModel.FeaturedStreams.Should().BeEquivalentTo(featuredStreams);
        _viewModel.LiveStreams.Should().BeEquivalentTo(liveStreams);
        _viewModel.Categories.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public void Title_ShouldBeHome()
    {
        _viewModel.Title.Should().Be("Home");
    }

    [Fact]
    public async Task LoadDataCommand_SetsBusyState()
    {
        // Arrange
        _apiServiceMock.Setup(x => x.GetFeaturedStreamsAsync())
            .ReturnsAsync(new List<Stream>());
        _apiServiceMock.Setup(x => x.GetLiveStreamsAsync(null, 1, 20))
            .ReturnsAsync(new List<Stream>());
        _apiServiceMock.Setup(x => x.GetCategoriesAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.IsBusy.Should().BeFalse();
        _viewModel.IsRefreshing.Should().BeFalse();
    }
}
