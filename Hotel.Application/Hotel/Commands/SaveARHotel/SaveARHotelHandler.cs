namespace Hotel.Application.Hotel.Commands.SaveARHotel;

internal sealed class SaveARHotelHandler : IRequestHandler<SaveARHotelCmd, bool>
{
    readonly ISqlSugarRepository<ARHotelEntity> _arHotelRepo;

    public SaveARHotelHandler(ISqlSugarRepository<ARHotelEntity> arHotelRepo)=>  _arHotelRepo = arHotelRepo;

    /// <summary>
    /// 保存酒店平台映射配置
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SaveARHotelCmd request, CancellationToken cancellationToken)
    {
        var hotelCodes = request.data.Select(x => x.HotelCode).ToList();
        var originalData = await _arHotelRepo.GetListAsync(x => hotelCodes.Contains(x.HotelCode ?? string.Empty));
        List<dynamic> ids = new();
        foreach (var i in request.data)
        {
            var model = originalData.SingleOrDefault(x => x.OtherPlatType == i.OtherPlatType);
            if (model is not null && string.IsNullOrWhiteSpace(i.OtherPlatformUrl))
            {
                ids.Add(model.Id);
                originalData.Remove(model);
                continue;
            }
            if (model is null && !string.IsNullOrWhiteSpace(i.OtherPlatformUrl))
            {
                originalData.Add(ARHotelEntity.Create(
                    i.HotelCode,
                    i.HotelName,
                    i.OtherPlatType,
                    i.OtherPlatformUrl,
                    "11360847",
                    "admin"));
                continue;
            }
            model?.ChangePlatformUrl(i.OtherPlatformUrl);
        }
        await _arHotelRepo.DeleteByIdsAsync(ids.ToArray());
        return await _arHotelRepo.InsertOrUpdateAsync(originalData);
    }
}
