using Dapper;
using Microsoft.Data.Sqlite;

string connectionString = "Data Source=../../../Database/db.sqlite;";

Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

void ListTasks()
{
    IEnumerable<Now.Domain.Entities.Task> tasks = tasksRepository.All();

    foreach (var task in tasks)
    {
        Console.WriteLine($"{task.Name} {task.CreatedAt} {task.DoneAt}");
    }
}

void InsertTask()
{
    tasksRepository.Insert(new Now.Domain.Entities.Task("aslkdfj alskjdf"));
}

void RemoveTask()
{
    Now.Domain.Entities.Task task = tasksRepository.Get(1);
    tasksRepository.Remove(task);
}

void UpdateTask()
{
    Now.Domain.Entities.Task task = tasksRepository.Get(2);

    var newer = new Now.Domain.Entities.Task(task.Id, task.ExternalId, "Alteradooo", task.Description, task.DoneAt, task.CreatedAt, task.UpdatedAt);

    tasksRepository.Update(newer);
}

void DoneTask()
{
    Now.Domain.Entities.Task task = tasksRepository.Get(2);

    tasksRepository.Done(task);
}

void UndoneTask()
{
    Now.Domain.Entities.Task task = tasksRepository.Get(2);

    tasksRepository.Undone(task);
}

ListTasks();
DoneTask();
ListTasks();
UndoneTask();
ListTasks();