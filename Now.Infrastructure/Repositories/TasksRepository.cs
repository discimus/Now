using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Infrastructure.Repositories
{
    public class TasksRepository : Domain.Repositories.ITasksRepository
    {
        private readonly string _connectionString;

        public TasksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public class Row
        {
            public int id { get; set; }
            public string external_id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public DateTime done_at { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }

            public Domain.Entities.Task ToTask()
            {
                return new Domain.Entities.Task(
                    id: id,
                    externalId: external_id,
                    name: name,
                    description: description ?? "",
                    doneAt: done_at,
                    createdAt: created_at,
                    updatedAt: updated_at);
            }

            public static Row FromTask(Domain.Entities.Task task)
            {
                return new Row()
                {
                    id = task.Id,
                    external_id = task.ExternalId,
                    name = task.Name,
                    description = task.Description,
                    done_at = task.DoneAt,
                    created_at = task.CreatedAt,
                    updated_at = task.UpdatedAt,
                };
            }
        }

        public IEnumerable<Domain.Entities.Task> All()
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        id,
                        external_id,
                        name,
                        description,
                        done_at,
                        created_at,
                        updated_at
                    from tasks;";

                IEnumerable<Row> rows = connection.Query<Row>(query);

                return rows.Select(t => t.ToTask());
            }
        }

        public void Remove(Domain.Entities.Task task)
        {
            if (task == null)
            {
                return;
            }

            Domain.Entities.Task? item = this.Get(task.Id);

            if (item != null)
            {
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
                {
                    string query = @"
                    delete
                    from tasks
                    where id = @id;";

                    connection.Execute(sql: query, param: new { id = task.Id });
                }
            }
        }

        public Domain.Entities.Task? Get(int id)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        id,
                        external_id,
                        name,
                        description,
                        done_at,
                        created_at,
                        updated_at
                    from tasks
                    where id = @id;";

                Row? row = connection.QueryFirstOrDefault<Row>(sql: query, param: new { id });

                return row?.ToTask();
            }
        }

        public Domain.Entities.Task? Get(string uuid)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        id,
                        external_id,
                        name,
                        description,
                        done_at,
                        created_at,
                        updated_at
                    from tasks
                    where external_id = @uuid;";

                Row? row = connection.QueryFirstOrDefault<Row>(sql: query, param: new { uuid });

                return row?.ToTask();
            }
        }

        public void Insert(Domain.Entities.Task task)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    insert into tasks (
                        external_id,
                        name,
                        description)
                    values (
                        @external_id, 
                        @name, 
                        @description);";

                connection.QueryFirstOrDefault<Row>(sql: query, param: Row.FromTask(task));
            }
        }

        public void Update(Domain.Entities.Task task)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    UPDATE tasks
                    SET 
                        name = @name, 
                        description = @description 
                    where id = @id;";

                connection.Execute(sql: query, param: Row.FromTask(task));
            }
        }

        public void Done(Domain.Entities.Task task)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    UPDATE tasks
                    SET 
                        done_at = (datetime('now','localtime'))
                    where id = @id;";

                connection.Execute(sql: query, param: Row.FromTask(task));
            }
        }

        public void Undone(Domain.Entities.Task task)
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
            {
                string query = @"
                    UPDATE tasks
                    SET 
                        done_at = null
                    where id = @id;";

                connection.Execute(sql: query, param: Row.FromTask(task));
            }
        }
    }
}
