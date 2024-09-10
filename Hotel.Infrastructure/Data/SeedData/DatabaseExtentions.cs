using Hotel.Domain.Entity;
using Hotel.Domain.EntityMG;
using Hotel.Domain.Shared.Const;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hotel.Infrastructure.Data.SeedData;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>().CopyNew();


        db.DbMaintenance.CreateDatabase();
        db.CodeFirst.InitTables(
            typeof(HotelEntity),
            typeof(ARHotelEntity),
            typeof(PursueHouseSettingEntity),
            typeof(PursueHouseRecordEntity)
            );
        await SeedAsync(db);



        var mongoDbContext = serviceScope.ServiceProvider.GetRequiredService<MongoDbContext>();
        await SeedTaskExecCursorAsync(mongoDbContext);
    }

    private static async Task SeedAsync(SqlSugarClient sqlSugarClient)
    {
        await SeedHotelAsync(sqlSugarClient);

    }
    private static async Task SeedHotelAsync(SqlSugarClient sqlSugarClient)
    {
        if (!await sqlSugarClient.Queryable<HotelEntity>().AnyAsync())
            await sqlSugarClient.Insertable<HotelEntity>(InitData.hotels).ExecuteCommandAsync();
    }

    private static async Task SeedTaskExecCursorAsync(MongoDbContext _context)
    {
        var _collection = _context.GetCollection<TaskExecCursor>(nameof(TaskExecCursor));
        if (!await _collection.Find(x => true).AnyAsync())
            await _collection.InsertOneAsync(new TaskExecCursor()
            {
                Id = new ObjectId(GlobaConst.TaskExecCursorObjId),
                RunId = 0,
                TakeId = 0
            });
    }

}
