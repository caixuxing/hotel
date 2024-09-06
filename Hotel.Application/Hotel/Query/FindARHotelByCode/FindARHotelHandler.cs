namespace Hotel.Application.Hotel.Query.FindARHotelByCode;

internal sealed class FindARHotelHandler : IRequestHandler<FindARHotelParamQry, List<FindARHotelDto>>
{
     readonly ISqlSugarRepository<ARHotelEntity> _arHotelRepo;
     readonly ISqlSugarClient _db;

    public FindARHotelHandler(ISqlSugarRepository<ARHotelEntity> arHotelRepo, ISqlSugarClient db)
    {
        _arHotelRepo = arHotelRepo;
        _db = db;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<FindARHotelDto>> Handle(FindARHotelParamQry request, CancellationToken cancellationToken)
    {
        var data = await _arHotelRepo.GetListAsync(x => x.HotelCode == request.code);
        var hotelName = await _db.Queryable<HotelEntity>()
               .Where(x => x.hotelcode == request.code)
               .Select(x => x.hotelname).SingleAsync();

        List<FindARHotelDto> result = new();
        Enum.GetValues(typeof(PlatTypeEnums)).Cast<int>().ToList()
            .ForEach(item =>
            {
                var model = data.Where(x => x.OtherPlatType == item.ToString()).Select(x => new FindARHotelDto()
                {
                    OtherPlatformUrl = x?.OtherPlatformUrl
                }).FirstOrDefault() ?? new FindARHotelDto() { };
                model.HotelCode = request.code;
                model.HotelName = hotelName;
                model.OtherPlatType = item.ToString();
                model.OtherPlatTypeName = item.ToString() switch
                {
                    "1" => "EXP",
                    "2" => "同程",
                    "3" => "猫头鹰",
                    "5" => "Agoda",
                    "6" => "去哪",
                    _ => ""
                };
                result.Add(model);
            });
        return result;
    }
}