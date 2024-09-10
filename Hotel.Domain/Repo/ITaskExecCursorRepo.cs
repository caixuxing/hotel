using Hotel.Domain.Entity;
using Hotel.Domain.EntityMG;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Repo
{

    public interface ITaskExecCursorRepo
    {
        Task<List<TaskExecCursor>> GetAllTaskExecCursor();
        Task<TaskExecCursor> GetTaskExecCursorById(ObjectId id);
        Task AddTaskExecCursor(TaskExecCursor data);
        Task UpdateTaskExecCursor(TaskExecCursor data);
        Task DeleteTaskExecCursor(ObjectId id);

        Task<List<TaskExecCursor>> GetListAsync(Expression<Func<TaskExecCursor, bool>> whereExpression);
    }
}
