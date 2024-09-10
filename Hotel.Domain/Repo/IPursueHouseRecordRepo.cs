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

    public interface IPursueHouseRecordRepo
    {
        Task<List<PursueHouseRecord>> GetAllBooks();
        Task<PursueHouseRecord> GetPursueHouseRecordById(ObjectId id);
        Task AddPursueHouseRecord(PursueHouseRecord data);

        Task AddPursueHouseRecord(IEnumerable<PursueHouseRecord> data);

        Task UpdatePursueHouseRecord(PursueHouseRecord data);
        Task DeletePursueHouseRecord(ObjectId id);

        Task DeletePursueHouseRecord(Expression<Func<PursueHouseRecord, bool>> whereExpression);

        List<PursueHouseRecord> GetListAsync(Expression<Func<PursueHouseRecord, bool>> whereExpression);
    }
}
