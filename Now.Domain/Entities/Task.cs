using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Now.Domain.Entities.Task;

namespace Now.Domain.Entities
{
    public class Task
    {
        public int Id { get; private set; }
        public string ExternalId { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DoneAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Task(int id, string externalId, string name, string description, DateTime doneAt, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            ExternalId = externalId;
            Name = name;
            Description = description;
            DoneAt = doneAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Task(string name, string description = "")
        {
            ExternalId = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
        }

        public class Dto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? DoneAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public Dto ToDto()
        {
            return new Dto()
            {
                Id = ExternalId,
                Name = Name,
                Description = Description,
                DoneAt = DoneAt == DateTime.MinValue ? null : DoneAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt == DateTime.MinValue ? null : UpdatedAt,
            };
        }
    }
}
