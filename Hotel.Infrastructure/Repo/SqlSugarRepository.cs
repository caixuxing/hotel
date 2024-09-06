namespace Hotel.Infrastructure.Repo;

 public class SqlSugarRepository<T> : SimpleClient<T>, ISqlSugarRepository<T> where T : class, new()
{
    public ITenant? itenant = null;//多租户事务
    public SqlSugarRepository(ISqlSugarClient? context = null) : base(context)
    {
        var configId = typeof(T).GetCustomAttribute<TenantAttribute>()?.configId;
        if (configId != null)
        {
            Context = DbScoped.SugarScope.GetConnectionScope(configId);
        }
        else
        {
            Context = context ?? DbScoped.SugarScope.GetConnectionScope(0);
        }
        itenant = DbScoped.SugarScope;
    }
    public async Task<bool> SplitTableInsertAsync(T input)
    {
        return await base.AsInsertable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableInsertAsync(List<T> input)
    {
        return await base.AsInsertable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableUpdateAsync(T input)
    {
        return await base.AsUpdateable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableUpdateAsync(List<T> input)
    {
        return await base.AsUpdateable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableDeleteableAsync(T input)
    {
        return await base.Context.Deleteable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableDeleteableAsync(List<T> input)
    {
        return await base.Context.Deleteable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public Task<T> SplitTableGetFirstAsync(Expression<Func<T, bool>> whereExpression)
    {
        return base.AsQueryable().SplitTable().FirstAsync(whereExpression);
    }

    public Task<bool> SplitTableIsAnyAsync(Expression<Func<T, bool>> whereExpression)
    {
        return base.Context.Queryable<T>().Where(whereExpression).SplitTable().AnyAsync();
    }

    public Task<List<T>> SplitTableGetListAsync()
    {
        return Context.Queryable<T>().SplitTable().ToListAsync();
    }

    public Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression)
    {
        return Context.Queryable<T>().Where(whereExpression).SplitTable().ToListAsync();
    }

    public Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression, string[] tableNames)
    {
        return Context.Queryable<T>().Where(whereExpression).SplitTable(t => t.InTableNames(tableNames)).ToListAsync();
    }
}

