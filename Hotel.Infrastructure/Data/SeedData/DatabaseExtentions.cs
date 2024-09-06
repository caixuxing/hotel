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
    }

    private static async Task SeedAsync(SqlSugarClient sqlSugarClient)
    {
        await SeedSysUsersAsync(sqlSugarClient);
    }
    private static async Task SeedSysUsersAsync(SqlSugarClient sqlSugarClient)
    {
        if (!await sqlSugarClient.Queryable<HotelEntity>().AnyAsync())
            await sqlSugarClient.Insertable<HotelEntity>(InitData.hotels).ExecuteCommandAsync();
    }

}
