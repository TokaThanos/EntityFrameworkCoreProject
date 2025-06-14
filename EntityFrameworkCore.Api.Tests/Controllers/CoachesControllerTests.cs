using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.Controllers
{
    public class CoachesControllerTests
    {
        private readonly CoachesController _coachesController;
        private readonly Mock<ICoachService> _coachServiceMock;

        public CoachesControllerTests()
        {
            _coachServiceMock = new Mock<ICoachService>();
            _coachesController = new CoachesController(_coachServiceMock.Object);
        }

        [Fact]
        public async Task CoachesController_GetCoaches_ReturnsExpectedResult()
        {
            // Arrange
            var expectedOutput = new List<CoachReadDto>
            {
                new CoachReadDto { Id = 1, Name = "Coach A" },
                new CoachReadDto { Id = 2, Name = "Coach B" }
            };

            _coachServiceMock.Setup(service => service.GetAllCoachesAsync())
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.GetCoaches();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var responseValue = okResult.Value as IEnumerable<CoachReadDto>;
            responseValue.Should().NotBeNull();
            responseValue.Should().HaveCount(2);
            responseValue.Should().BeAssignableTo<IEnumerable<CoachReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task CoachesController_GetCoach_ReturnsExpectedResult(int id)
        {
            // Arrange
            var expectedOutput = id > 0 ? new CoachReadInfoDto { CoachName = "Coach", TeamName = "Team" } : null;

            _coachServiceMock.Setup(service => service.GetCoachByIdAsync(id))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.GetCoach(id);

            // Assert
            if (id > 0)
            {
                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.Value.Should().BeOfType<CoachReadInfoDto>();
            }
            else
            {
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task CoachesController_PostCoach_ReturnsExpectedResult()
        {
            // Arrange
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };

            var expectedOutput = new CoachReadDto
            {
                Id = 1,
                Name = "Coach"
            };

            _coachServiceMock.Setup(service => service.AddCoachAsync(requestInput))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.PostCoach(requestInput);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(_coachesController.GetCoach));
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task CoachesController_DeleteCoach_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _coachServiceMock.Setup(service => service.DeleteCoachByIdAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _coachesController.DeleteCoach(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _coachServiceMock.Verify(service => service.DeleteCoachByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task CoachesController_PutCoach_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };

            _coachServiceMock.Setup(service => service.UpdateCoachAsync(id, requestInput))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _coachesController.PutCoach(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _coachServiceMock.Verify(service => service.UpdateCoachAsync(id, requestInput), Times.Once);
        }
    }
}
