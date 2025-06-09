using Accidenta.Application.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Moq;
using Serilog;

namespace Accidenta.Application.Tests
{
    public class CreateIncidentHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IAccountRepository> _accountRepoMock = new();
        private readonly Mock<IContactRepository> _contactRepoMock = new();
        private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
        private readonly Mock<ILogger> _loggerMock = new();
        private readonly CreateIncidentHandler _handler;

        public CreateIncidentHandlerTests()
        {
            _unitOfWorkMock.Setup(u => u.Accounts).Returns(_accountRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Contacts).Returns(_contactRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Incidents).Returns(_incidentRepoMock.Object);

            _handler = new CreateIncidentHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateExistingContact_AndLinkToAccount_AndCreateIncident()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "TestAccount" };
            var contact = new Contact { Id = Guid.NewGuid(), Email = "user@test.com" };

            var request = new CreateIncidentRequest
            {
                AccountName = "TestAccount",
                Email = "user@test.com",
                FirstName = "Updated",
                LastName = "User",
                Description = "Incident occurred"
            };

            _accountRepoMock.Setup(r => r.GetByNameAsync("TestAccount", It.IsAny<CancellationToken>())).ReturnsAsync(account);
            _contactRepoMock.Setup(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(contact);
            _incidentRepoMock.Setup(r => r.AddAsync(It.IsAny<Incident>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(new CreateIncident(request), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _contactRepoMock.Verify(r => r.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>()), Times.Once);
            _incidentRepoMock.Verify(r => r.AddAsync(It.Is<Incident>(i => i.Description == request.Description), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateNewContact_AndLinkToAccount_AndCreateIncident()
        {
            // Arrange
            var account = new Account { Id = Guid.NewGuid(), Name = "TestAccount" };

            var request = new CreateIncidentRequest
            {
                AccountName = "TestAccount",
                Email = "newuser@test.com",
                FirstName = "New",
                LastName = "User",
                Description = "New incident"
            };

            _accountRepoMock.Setup(r => r.GetByNameAsync("TestAccount", It.IsAny<CancellationToken>())).ReturnsAsync(account);
            _contactRepoMock.Setup(r => r.GetByEmailAsync("newuser@test.com", It.IsAny<CancellationToken>())).ReturnsAsync((Contact?)null);
            _contactRepoMock.Setup(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _incidentRepoMock.Setup(r => r.AddAsync(It.IsAny<Incident>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(new CreateIncident(request), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _contactRepoMock.Verify(r => r.AddAsync(It.Is<Contact>(c => c.Email == request.Email), It.IsAny<CancellationToken>()), Times.Once);
            _incidentRepoMock.Verify(r => r.AddAsync(It.IsAny<Incident>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenAccountNotFound()
        {
            // Arrange
            var request = new CreateIncidentRequest
            {
                AccountName = "MissingAccount",
                Email = "user@test.com",
                FirstName = "First",
                LastName = "Last",
                Description = "Issue"
            };

            _accountRepoMock.Setup(r => r.GetByNameAsync("MissingAccount", It.IsAny<CancellationToken>())).ReturnsAsync((Account?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new CreateIncident(request), CancellationToken.None));
        }
    }
}
