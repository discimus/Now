using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Tests.Domain
{
    public class EntitiesTests
    {
        [Fact]
        public void Task_TransformEntityIntoDto_ShouldCreateDtoWithSameData()
        {
            var task1 = new Now.Domain.Entities.Task("First task", "With some description");

            var dto1 = task1.ToDto();

            dto1.Id.Should().Be(task1.ExternalId);
            dto1.Name.Should().Be(task1.Name);
            dto1.Description.Should().Be(task1.Description);
            dto1.UpdatedAt.Should().Be(task1.UpdatedAt);
            dto1.CreatedAt.Should().Be(task1.CreatedAt);
            dto1.UpdatedAt.Should().Be(task1.UpdatedAt);

            var task2 = new Now.Domain.Entities.Task(
                id: 123,
                externalId: "21202c2a-35bd-4e9a-898d-0a8e0d5c4fc9",
                name: "Other task",
                description: "With other description",
                doneAt: DateTime.Now,
                createdAt:  DateTime.Now,
                updatedAt: DateTime.Now);

            var dto2 = task2.ToDto();

            dto2.Id.Should().Be(task2.ExternalId);
            dto2.Name.Should().Be(task2.Name);
            dto2.Description.Should().Be(task2.Description);
            dto2.UpdatedAt.Should().Be(task2.UpdatedAt);
            dto2.CreatedAt.Should().Be(task2.CreatedAt);
            dto2.UpdatedAt.Should().Be(task2.UpdatedAt);
        }
    }
}
