using FluentAssertions;
using Moq;
using VaughnLive.Services;
using VaughnLive.ViewModels;
using VaughnLive.Models;
using Xunit;

namespace VaughnLive.Tests.ViewModels;

public class BroadcastViewModelTests
{
    private readonly Mock<IRtmpService> _rtmpServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly BroadcastViewModel _viewModel;

    public BroadcastViewModelTests()
    {
        _rtmpServiceMock = new Mock<IRtmpService>();
        _authServiceMock = new Mock<IAuthService>();
        _viewModel = new BroadcastViewModel(_rtmpServiceMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public void Title_ShouldBeGoLive()
    {
        _viewModel.Title.Should().Be("Go Live");
    }

    [Fact]
    public void DefaultResolution_ShouldBeHD720()
    {
        _viewModel.Resolution.Should().Be(VideoResolution.HD720);
    }

    [Fact]
    public void DefaultCameraPosition_ShouldBeBack()
    {
        _viewModel.CameraPosition.Should().Be(CameraPosition.Back);
    }

    [Fact]
    public void ToggleCameraCommand_SwitchesCamera()
    {
        // Arrange
        var initialPosition = _viewModel.CameraPosition;

        // Act
        _viewModel.ToggleCameraCommand.Execute(null);

        // Assert
        _viewModel.CameraPosition.Should().NotBe(initialPosition);
    }

    [Fact]
    public void ToggleMuteCommand_TogglesMuteState()
    {
        // Arrange
        var initialMuted = _viewModel.IsMuted;

        // Act
        _viewModel.ToggleMuteCommand.Execute(null);

        // Assert
        _viewModel.IsMuted.Should().Be(!initialMuted);
    }
}
