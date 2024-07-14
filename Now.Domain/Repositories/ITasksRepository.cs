using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Domain.Repositories
{
    public interface ITasksRepository
    {
        IEnumerable<Entities.Task> All();
        Entities.Task Get(int id);
        Entities.Task Get(string uuid);
        void Insert(Entities.Task task);
        void Update(Entities.Task task);
        void Done(Entities.Task task);
        void Undone(Entities.Task task);
        void Remove(Entities.Task task);
    }
}
