using Hotel.Domain.EntityMG;
using Hotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Hotel.Infrastructure.Repo
{
    public class BookRepository : IBookRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Book> _booksCollection;

        public BookRepository(MongoDbContext context)
        {
            _context = context;
            _booksCollection = _context.GetCollection<Book>("books");
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _booksCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Book> GetBookById(ObjectId id)
        {
            return await _booksCollection.Find(book => book.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddBook(Book book)
        {
            await _booksCollection.InsertOneAsync(book);
        }

        public async Task UpdateBook(Book book)
        {
            await _booksCollection.ReplaceOneAsync(b => b.Id == book.Id, book);
        }

        public async Task DeleteBook(ObjectId id)
        {
            await _booksCollection.DeleteOneAsync(book => book.Id == id);
        }

        public  async Task<List<Book>> GetListAsync(Expression<Func<Book, bool>> whereExpression)
        {
    /*        var filter = Builders<Book>.Filter.And( Builders<Book>.Filter.Eq("No", 1832689690612994048));
            var orders = _booksCollection.Find(filter).ToList();*/
            return await(await _booksCollection.FindAsync(whereExpression)).ToListAsync();


        }
    }
}
