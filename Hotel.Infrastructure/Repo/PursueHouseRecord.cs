using Hotel.Domain.EntityMG;
using Hotel.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hotel.Infrastructure.Repo
{


    public class PursueHouseRecordRepo : IPursueHouseRecordRepo
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<PursueHouseRecord> _booksCollection;

        public PursueHouseRecordRepo(MongoDbContext context)
        {
            _context = context;
            _booksCollection = _context.GetCollection<PursueHouseRecord>("PursueHouseRecords");
        }

        public async Task<List<PursueHouseRecord>> GetAllBooks()
        {
            return await _booksCollection.Find(_ => true).ToListAsync();
        }

        public async Task<PursueHouseRecord> GetPursueHouseRecordById(ObjectId id)
        {
            return await _booksCollection.Find(book => book.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddPursueHouseRecord(PursueHouseRecord data)
        {
            await _booksCollection.InsertOneAsync(data);
        }

        public async Task AddPursueHouseRecord(IEnumerable<PursueHouseRecord> data)
        {
            await _booksCollection.InsertManyAsync(data);
        }

        public async Task UpdatePursueHouseRecord(PursueHouseRecord model)
        {
            await _booksCollection.ReplaceOneAsync(b => b.Id == model.Id, model);
        }

        public async Task DeletePursueHouseRecord(ObjectId id)
        {
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
        }

        public List<PursueHouseRecord> GetListAsync(Expression<Func<PursueHouseRecord, bool>> whereExpression)
        {
            return  _booksCollection.Find(whereExpression).Limit(50).ToList() ;
        }

     
    }
}
