using Dapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Now.Infrastructure.Repositories.TasksRepository;

namespace Now.Tests.Domain
{
    public class TasksRepositoryTests
    {
        private void CreateTableTasks(string connectionString)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                string queryCreateTableTasks = @"
                    create table tasks (
	                    id integer primary key autoincrement,
	                    external_id text not null,
	                    name text not null,
	                    description text,
	                    done_at datetime,
	                    created_at datetime DEFAULT (datetime('now','localtime')) NOT NULL,
	                    updated_at datetime
                    );";

                connection.Execute(queryCreateTableTasks);
            }
        }

        [Fact]
        public void All_SimulateQueryWithInMemoryDb_ShouldRetrieveExpectedAttributes()
        {
            /**
             * String de conexao para db sqlite em memoria 
             * compartilhavel entre conexoes do tipo IDisposable
             * https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/in-memory-databases
             */
            string connectionString = "Data Source=All_SimulateQueryWithInMemoryDb_ShouldRetrieveExpectedAttributes;Mode=Memory;Cache=Shared";

            /**
             * Conexao persistente necessaria para o 
             * banco de dados em memoria nao ser destruido
             */
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);

                string queryInsertFirstTask = @"
                    insert into tasks
                        (external_id,name,description)
                    values
                        ('21202c2a-35bd-4e9a-898d-0a8e0d5c4fc9', 'asdf', 'First Task xD');";

                connection.Execute(queryInsertFirstTask);

                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                IEnumerable<Now.Domain.Entities.Task> tasks = tasksRepository.All();

                tasks.Should().NotBeNullOrEmpty();
                tasks.Should().ContainSingle();

                Now.Domain.Entities.Task task = tasks.Single();

                task.ExternalId.Should().Be("21202c2a-35bd-4e9a-898d-0a8e0d5c4fc9");
                task.Name.Should().Be("asdf");
                task.Description.Should().Be("First Task xD");
                task.DoneAt.Should().Be(DateTime.MinValue);

                connection.Close();
            }
        }

        [Fact]
        public void Insert_InsertTask_ShouldPersistExpectedData()
        {
            /**
             * O item 'Data Source' deve ser distinto
             * para que testes sendo executados ao mesmo
             * tempo nao influenciem uns nos outros
             */
            string connectionString = "Data Source=Insert_InsertTask_ShouldPersistExpectedData;Mode=Memory;Cache=Shared";

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);
                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                var expectedTask = new Now.Domain.Entities.Task(name: "Other task", description: "With description!");

                tasksRepository.Insert(expectedTask);

                IEnumerable<Now.Domain.Entities.Task> tasks = tasksRepository.All();

                tasks.Should().NotBeNullOrEmpty();
                tasks.Should().ContainSingle();

                Now.Domain.Entities.Task task = tasks.Single();

                task.ExternalId.Should().Be(expectedTask.ExternalId);
                task.Name.Should().Be(expectedTask.Name);
                task.Description.Should().Be(expectedTask.Description);
                task.DoneAt.Should().Be(expectedTask.DoneAt);

                connection.Close();
            }
        }

        [Fact]
        public void Delete_InsertTaskAndRemoveAfter_RepositoryShouldPersistAndRemoveIt()
        {
            string connectionString = "Data Source=Delete_InsertTaskAndRemoveAfter_RepositoryShouldPersistAndRemoveIt;Mode=Memory;Cache=Shared";

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);

                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                var expectedTask = new Now.Domain.Entities.Task(name: "Other task", description: "With description!");

                tasksRepository.Insert(expectedTask);

                IEnumerable<Now.Domain.Entities.Task> tasksBefore = tasksRepository.All();

                tasksBefore.Should().NotBeNullOrEmpty();
                tasksBefore.Should().ContainSingle();

                Now.Domain.Entities.Task task = tasksBefore.Single();

                tasksRepository.Remove(task);

                IEnumerable<Now.Domain.Entities.Task> tasksAfter = tasksRepository.All();

                tasksAfter.Should().BeEmpty();

                connection.Close();
            }
        }

        [Fact]
        public void Update_InsertTaskAndUpdateAfter_ShouldPersistAllChangedInTask()
        {
            string connectionString = "Data Source=Update_InsertTaskAndUpdateAfter_ShouldPersistAllChangedInTask;Mode=Memory;Cache=Shared";

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);

                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                var expectedTask = new Now.Domain.Entities.Task(name: "Other task", description: "With description!");

                tasksRepository.Insert(expectedTask);

                IEnumerable<Now.Domain.Entities.Task> tasksBefore = tasksRepository.All();

                tasksBefore.Should().NotBeNullOrEmpty();
                tasksBefore.Should().ContainSingle();

                string newName = "Changed name";
                string newDescription = "Changed description";

                Now.Domain.Entities.Task task = tasksBefore.Single();

                var copy = new Now.Domain.Entities.Task(
                    id: task.Id,
                    externalId: task.ExternalId,
                    name: newName,
                    description: newDescription,
                    doneAt: task.DoneAt,
                    createdAt: task.CreatedAt,
                    updatedAt: task.CreatedAt);

                tasksRepository.Update(copy);

                Now.Domain.Entities.Task updatedTask = tasksRepository.Get(copy.Id);

                updatedTask.Should().NotBeNull();

                updatedTask.Name.Should().Be(newName);
                updatedTask.Description.Should().Be(newDescription);

                connection.Close();
            }
        }

        [Fact]
        public void Update_DoneAndUndoneTasl_ShouldUpdateTask()
        {
            string connectionString = "Data Source=Update_DoneAndUndoneTasl_ShouldUpdateTask;Mode=Memory;Cache=Shared";

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);

                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                var task = new Now.Domain.Entities.Task(name: "Other task", description: "With description!");

                tasksRepository.Insert(task);

                IEnumerable<Now.Domain.Entities.Task> tasksBefore = tasksRepository.All();

                tasksBefore.Should().NotBeNullOrEmpty();
                tasksBefore.Should().ContainSingle();


                Now.Domain.Entities.Task? taskBeforeDone = tasksRepository.Get(task.ExternalId);
                taskBeforeDone.Should().NotBeNull();
                taskBeforeDone.DoneAt.Should().Be(DateTime.MinValue);

                tasksRepository.Done(taskBeforeDone);

                Now.Domain.Entities.Task? taskAfterDone = tasksRepository.Get(task.ExternalId);
                taskAfterDone.Should().NotBeNull();
                taskAfterDone.DoneAt.Should().NotBe(DateTime.MinValue);

                tasksRepository.Undone(taskAfterDone);

                Now.Domain.Entities.Task? taskAfterUndone = tasksRepository.Get(task.ExternalId);
                taskAfterUndone.Should().NotBeNull();
                taskAfterUndone.DoneAt.Should().Be(DateTime.MinValue);

                connection.Close();
            }
        }

        [Fact]
        public void All_QueryEmptyTable_ShouldNotRetrieveNullList()
        {
            string connectionString = "Data Source=All_QueryEmptyTable_ShouldNotRetrieveNullList;Mode=Memory;Cache=Shared";

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                CreateTableTasks(connectionString);

                var tasksRepository = new Now.Infrastructure.Repositories.TasksRepository(connectionString);

                IEnumerable<Now.Domain.Entities.Task> tasksBefore = tasksRepository.All();

                tasksBefore.Should().NotBeNull();
                tasksBefore.Should().BeEmpty();
            }
        }
    }
}
