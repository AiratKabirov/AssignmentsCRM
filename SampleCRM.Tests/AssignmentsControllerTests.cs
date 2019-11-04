using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SampleCRM.Services;
using SampleCRM.ViewModels;
using SampleCRM.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SampleCRM.Tests
{
    public class AssignmentsControllerTests
    {
        private AssignmentsController assignmentsController;

        private AssignmentViewModel newTestAssignment = new AssignmentViewModel
        {
            Id = "1",
            Name = "Test",
            Description = "TestDescription"
        };

        private List<AssignmentViewModel> testAssignments = new List<AssignmentViewModel>
        {
            new AssignmentViewModel
            {
                Id = "1",
                Name = "Test1",
                Description = "TestDescription1"
            },
            new AssignmentViewModel
            {
                Id = "2",
                Name = "Test2",
                Description = "TestDescription2"
            },
            new AssignmentViewModel
            {
                Id = "3",
                Name = "Test3",
                Description = "TestDescription3"
            }
        };

        public AssignmentsControllerTests()
        {
            var mockedAssignmentsService = new Mock<IAssignmentsService>();
            mockedAssignmentsService.Setup(aS => aS.ListAssignments()).Returns(Task.FromResult(testAssignments.AsEnumerable()));
            mockedAssignmentsService.Setup(aS => aS.GetAssignment(It.IsAny<string>())).Returns(Task.FromResult(testAssignments.First()));
            mockedAssignmentsService.Setup(aS => aS.CreateAssignment(It.IsAny<AssignmentViewModel>())).Returns(Task.FromResult(newTestAssignment));
            mockedAssignmentsService.Setup(aS => aS.UpdateAssignment(It.IsAny<string>(), It.IsAny<AssignmentViewModel>())).Returns(Task.FromResult(newTestAssignment));
            mockedAssignmentsService.Setup(aS => aS.DeleteAssignment(It.IsAny<string>()));

            this.assignmentsController = new AssignmentsController(mockedAssignmentsService.Object);
        }

        [Fact]
        public async void GetAllAssignments()
        {
            var result = await assignmentsController.Get();

            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public async void GetAssignment()
        {
            var result = await assignmentsController.Get("1");

            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public async void CreateAssignment()
        {
            var result = await assignmentsController.Post(newTestAssignment);

            Assert.True(result.Result is CreatedResult);
        }

        [Fact]
        public async void UpdateAssignment()
        {
            var result = await assignmentsController.Put("1", newTestAssignment);

            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public async void DeleteAssignment()
        {
            var result = await assignmentsController.Delete("1");

            Assert.True(result is NoContentResult);
        }
    }
}
