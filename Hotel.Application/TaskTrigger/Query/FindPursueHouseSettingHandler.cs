namespace Hotel.Application.TaskTrigger.Query;
internal sealed class FindPursueHouseSettingHandler : IRequestHandler<FindPursueHouseSettingQry, FindPursueHouseSettingDto>
{

    readonly ISqlSugarRepository<PursueHouseSettingEntity> _pursueHouseSettingRepo;

    public FindPursueHouseSettingHandler(ISqlSugarRepository<PursueHouseSettingEntity> pursueHouseSettingRepo)
    {
        _pursueHouseSettingRepo = pursueHouseSettingRepo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FindPursueHouseSettingDto> Handle(FindPursueHouseSettingQry request, CancellationToken cancellationToken)
    {
        var data = await _pursueHouseSettingRepo.GetSingleAsync(x => x.BusinessId == request.BusinessId);
        if (data is not null)
        {
            return new()
            {
                BusinessId = data.BusinessId,
                EndDate = data.EndDate,
                Id = data.Id,
                MsgPushType = data.MsgPushType,
                Premium = data.Premium,
                StartDate = data.StartDate,
                Status = data.Status
            };
        }
        return new();
    }
}
