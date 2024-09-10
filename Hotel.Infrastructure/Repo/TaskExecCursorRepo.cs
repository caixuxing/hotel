using Hotel.Domain.EntityMG;
using Hotel.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repo
{
    public class TaskExecCursorRepo: ITaskExecCursorRepo
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<TaskExecCursor> _taskExecCursorCollection;

        public TaskExecCursorRepo(MongoDbContext context)
        {
            _context = context;
            _taskExecCursorCollection = _context.GetCollection<TaskExecCursor>("TaskExecCursor");
        }

        public async Task<List<TaskExecCursor>> GetAllTaskExecCursor()
        {
            return await _taskExecCursorCollection.Find(_ => true).ToListAsync();
        }

        public async Task<TaskExecCursor> GetTaskExecCursorById(ObjectId id)
        {
            return await _taskExecCursorCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddTaskExecCursor(TaskExecCursor data)
        {
            await _taskExecCursorCollection.InsertOneAsync(data);
        }

        public async Task UpdateTaskExecCursor(TaskExecCursor data)
        {
            await _taskExecCursorCollection.ReplaceOneAsync(b => b.Id == data.Id, data);
        }

        public async Task DeleteTaskExecCursor(ObjectId id)
        {
            await _taskExecCursorCollection.DeleteOneAsync(book => book.Id == id);
        }

        public async Task<List<TaskExecCursor>> GetListAsync(Expression<Func<TaskExecCursor, bool>> whereExpression)
        {
            return await (await _taskExecCursorCollection.FindAsync(whereExpression)).ToListAsync();
        }
    }
}
