using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Application.Services
{
    public class TasksService
    {
        private readonly Domain.Repositories.ITasksRepository _tasksRepository;

        public TasksService(Domain.Repositories.ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        public IEnumerable<Domain.Entities.Task.Dto> GetAllTasks()
        {
            return _tasksRepository.All().Select(t => t.ToDto());
        }

        public Domain.Entities.Task.Dto? Find(string uuid)
        {
            if (!Guid.TryParse(uuid, out var id))
            {
                return null;
            }

            return _tasksRepository.Get(uuid)?.ToDto();
        }

        public record CreateTaskModel(string name, string descrtption = "");

        public void Create(CreateTaskModel createTaskModel)
        {
            _tasksRepository.Insert(new Domain.Entities.Task(name: createTaskModel.name, description: createTaskModel.descrtption));
        }

        public record UpdateTaskModel(string name, string descritption);

        public void Update(string uuid, UpdateTaskModel updateTaskModel)
        {
            Domain.Entities.Task task1 = _tasksRepository.Get(uuid);

            if (task1 == null)
            {
                throw new InvalidOperationException();
            }

            Domain.Entities.Task task2 = new Domain.Entities.Task(
                id: task1.Id,
                externalId: task1.ExternalId,
                name: updateTaskModel.name,
                description: updateTaskModel.descritption,
                doneAt: task1.DoneAt,
                createdAt: task1.CreatedAt,
                updatedAt: task1.UpdatedAt);

            _tasksRepository.Update(task2);
        }

        public void Done(string uuid)
        {
            Domain.Entities.Task task = _tasksRepository.Get(uuid);

            if (task == null)
            {
                throw new InvalidOperationException();
            }

            _tasksRepository.Done(task);
        }

        public void Undone(string uuid)
        {
            Domain.Entities.Task task = _tasksRepository.Get(uuid);

            if (task == null)
            {
                throw new InvalidOperationException();
            }

            _tasksRepository.Undone(task);
        }

        public void Remove(string uuid)
        {
            Domain.Entities.Task task = _tasksRepository.Get(uuid);

            if (task == null)
            {
                throw new InvalidOperationException();
            }

            _tasksRepository.Remove(task);
        }
    }
}
