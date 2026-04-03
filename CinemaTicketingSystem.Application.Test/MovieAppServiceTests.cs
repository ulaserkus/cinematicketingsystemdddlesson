using CinemaTicketingSystem.Application.Contracts.Catalog.Movie;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.Application.Test.core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class MovieAppServiceTests : BaseIntegrationTest
{
    private readonly IMovieAppService _movieAppService;

    public MovieAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _movieAppService = GetService<IMovieAppService>();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var request = new CreateMovieRequest(
            Title: "Inception " + Guid.NewGuid(),
            PosterImageUrl: "https://example.com/inception.jpg",
            OriginalTitle: "Inception",
            Description: "A mind-bending thriller",
            Duration: TimeSpan.FromMinutes(148),
            EarliestShowingDate: DateTime.UtcNow.AddDays(7));

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data!.NewMovieId);
    }

    [Fact]
    public async Task CreateAsync_WithMinimalRequest_ShouldReturnSuccess()
    {
        // Arrange
        var request = new CreateMovieRequest(
            Title: "The Matrix " + Guid.NewGuid(),
            PosterImageUrl: "https://example.com/matrix.jpg",
            OriginalTitle: null,
            Description: null,
            Duration: TimeSpan.FromMinutes(136),
            EarliestShowingDate: null);

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data!.NewMovieId);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateTitle_ShouldReturnError()
    {
        // Arrange
        var uniqueTitle = "Interstellar " + Guid.NewGuid();
        var request = new CreateMovieRequest(
            Title: uniqueTitle,
            PosterImageUrl: "https://example.com/interstellar.jpg",
            OriginalTitle: null,
            Description: null,
            Duration: TimeSpan.FromMinutes(169),
            EarliestShowingDate: null);

        await _movieAppService.CreateAsync(request);

        // Act
        var duplicateResult = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(duplicateResult.IsFail);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMovies()
    {
        // Arrange
        var initialCount = (await DbContext.Set<Domain.BoundedContexts.Catalog.Movie>().ToListAsync()).Count;

        // Act
        var result = await _movieAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(initialCount, result.Data.Movies.Count);
    }

    [Fact]
    public async Task CreateAsync_ThenGetAllAsync_ShouldIncludeCreatedMovie()
    {
        // Arrange
        var initialCount = (await DbContext.Set<Domain.BoundedContexts.Catalog.Movie>().ToListAsync()).Count;

        var request = new CreateMovieRequest(
            Title: "The Dark Knight " + Guid.NewGuid(),
            PosterImageUrl: "https://example.com/darkknight.jpg",
            OriginalTitle: "The Dark Knight",
            Description: "A superhero film",
            Duration: TimeSpan.FromMinutes(152),
            EarliestShowingDate: DateTime.UtcNow.AddDays(3));

        await _movieAppService.CreateAsync(request);

        // Act
        var result = await _movieAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialCount + 1, result.Data!.Movies.Count);
    }
}
