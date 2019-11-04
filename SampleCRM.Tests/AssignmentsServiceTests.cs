using Xunit;
using Moq;
using SampleCRM.Utilities;
using System.Collections.Generic;
using SampleCRM.Models;
using System.Threading.Tasks;
using System.Linq;
using SampleCRM.Services;
using SampleCRM.ViewModels;

namespace SampleCRM.Tests
{
    public class AssignmentsServiceTests
    {
        private AssignmentsService assignmentsService;

        private AssignmentViewModel newTestAssignment = new AssignmentViewModel
        {
            Name = "Test",
            Description = "TestDescription"
        };

        private AssignmentViewModel updatedTestAssignment = new AssignmentViewModel
        {
            Id = "1",
            Name = "Test4",
            Description = "TestDescription4"
        };

        private List<Assignment> testAssignments = new List<Assignment>
        {
            new Assignment
            {
                RowKey = "1",
                Name = "Test1",
                Description = "TestDescription1"
            },
            new Assignment
            {
                RowKey = "2",
                Name = "Test2",
                Description = "TestDescription2"
            },
            new Assignment
            {
                RowKey = "3",
                Name = "Test3",
                Description = "TestDescription3"
            }
        };

        public AssignmentsServiceTests()
        {
            var mockedTableClient = new Mock<ITableClient>();
            mockedTableClient.Setup(tC => tC.ListAllEntities()).Returns(Task.FromResult<IEnumerable<Assignment>>(testAssignments));
            mockedTableClient.Setup(tC => tC.GetEntityById("1")).Returns(Task.FromResult(testAssignments.First()));
            mockedTableClient.Setup(tC => tC.GetEntityById("10")).Returns(Task.FromResult<Assignment>(null));
            mockedTableClient.Setup(tC => tC.InsertOrMergeEntityAsync(It.IsAny<Assignment>())).Returns(Task.FromResult(newTestAssignment.GetAssignment()));
            mockedTableClient.Setup(tC => tC.DeleteEntityAsync("1"));

            this.assignmentsService = new AssignmentsService(mockedTableClient.Object);
        }

        [Fact]
        public async void ListTestAssignments()
        {
            var assignments = await this.assignmentsService.ListAssignments();

            Assert.Equal(testAssignments.Count, assignments.Count());
        }

        [Fact]
        public async void GetExistingAssignment()
        {
            var assignment = await this.assignmentsService.GetAssignment("1");

            Assert.NotNull(assignment);
        }

        [Fact]
        public async void GetNonExistingAssignment_ShouldThrow()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => this.assignmentsService.GetAssignment("10"));
        }

        [Fact]
        public async void CreateNewAssignment()
        {
            var createdAssignment = await this.assignmentsService.CreateAssignment(newTestAssignment);

            Assert.NotNull(createdAssignment);
        }

        [Fact]
        public async void CreateAlreadyExistingAssignment_ShouldThrow()
        {
            await Assert.ThrowsAsync<BadRequestException>(() => this.assignmentsService.CreateAssignment(testAssignments.First().GetAssignmentViewModel()));
        }

        [Fact]
        public async void UpdateAssignment()
        {
            var updatedAssignment = await this.assignmentsService.UpdateAssignment("1", updatedTestAssignment);

            Assert.NotNull(updatedAssignment);
        }

        [Fact]
        public async void UpdateAssignmentWithNewId_ShouldThrow()
        {
            var assignment = new AssignmentViewModel
            {
                Id = "100"
            };

            await Assert.ThrowsAsync<BadRequestException>(() => this.assignmentsService.UpdateAssignment("1", assignment));
        }

        [Fact]
        public async void DeleteAssignment()
        {
            await this.assignmentsService.DeleteAssignment("1");
        }
    }
}
