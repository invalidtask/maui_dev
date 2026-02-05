using FluentAssertions;
using VaughnLive.Services;
using Xunit;

namespace VaughnLive.Tests.Services;

public class RtmpServiceTests
{
    private readonly RtmpService _rtmpService;

    public RtmpServiceTests()
    {
        _rtmpService = new RtmpService();
    }

    [Fact]
    public void IsStreaming_DefaultsToFalse()
    {
        _rtmpService.IsStreaming.Should().BeFalse();
    }

    [Fact]
    public async Task StopBroadcastAsync_WhenNotStreaming_DoesNotThrow()
    {
        // Act
        var action = async () => await _rtmpService.StopBroadcastAsync();

        // Assert
        await action.Should().NotThrowAsync();
    }
}
