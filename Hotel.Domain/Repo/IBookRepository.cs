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
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<Book> GetBookById(ObjectId id);
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(ObjectId id);

        Task<List<Book>> GetListAsync(Expression<Func<Book, bool>> whereExpression);
    }
}
