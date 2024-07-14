using FluentAssertions;
using Moq;
using Now.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Tests.Application
{
    public class TasksServiceTests
    {
        [Fact]
        public void GetAllTasks_ListAllTasks_ShouldReturnDtoWithData()
        {
            Mock<ITasksRepository> mock = new Mock<ITasksRepository>();

            var task = new Now.Domain.Entities.Task("Simple task", "With description");

            var expectedTasks = new List<Now.Domain.Entities.Task>()
            {
                task
            };

            mock.Setup(t => t.All()).Returns(expectedTasks);

            var tasksService = new Now.Application.Services.TasksService(mock.Object);

            IEnumerable<Now.Domain.Entities.Task.Dto> tasks = tasksService.GetAllTasks();

            tasks.Should().NotBeNullOrEmpty();
            tasks.Should().ContainSingle();

            Now.Domain.Entities.Task.Dto dto = tasks.Single();

            dto.Id.Should().Be(task.ExternalId);
            dto.Name.Should().Be(task.Name);
            dto.Description.Should().Be(task.Description);
            dto.UpdatedAt.Should().Be(task.UpdatedAt);
            dto.CreatedAt.Should().Be(task.CreatedAt);
            dto.UpdatedAt.Should().Be(task.UpdatedAt);
        }
    }
}
