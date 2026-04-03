using CinemaTicketingSystem.Application.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;
using CinemaTicketingSystem.Application.Test.core;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class CinemaAppServiceTests : BaseIntegrationTest
{
    private readonly ICinemaAppService _cinemaAppService;

    public CinemaAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _cinemaAppService = GetService<ICinemaAppService>();
    }

    [Fact]
    public async Task GetCinemaHallsAsync_WithNonExistentCinema_ShouldReturnError()
    {
        // Arrange
        var nonExistentCinemaId = Guid.NewGuid();

        // Act
        var result = await _cinemaAppService.GetCinemaHallsAsync(nonExistentCinemaId);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task GetAsync_WithNonExistentCinema_ShouldReturnError()
    {
        // Arrange
        var nonExistentCinemaId = Guid.NewGuid();

        // Act
        var result = await _cinemaAppService.GetAsync(nonExistentCinemaId);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCinemas()
    {
        // Arrange
        var initialCount = (await DbContext.Cinemas.ToListAsync()).Count;

        // Act
        var result = await _cinemaAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(initialCount, result.Data.Count);
    }
}
