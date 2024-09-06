namespace Hotel.Application.Hotel.Query.PageList;

internal sealed class HotelPageListHandler : IRequestHandler<HotelPageParamQry, List<HotelPageListDto>>
{

     readonly ISqlSugarRepository<HotelEntity> _hotelRepo;

    public HotelPageListHandler(ISqlSugarRepository<HotelEntity> hotelRepo)=> _hotelRepo = hotelRepo;
    

    public async Task<List<HotelPageListDto>> Handle(HotelPageParamQry request, CancellationToken cancellationToken)
    {
        RefAsync<int> total = 0;
        var result = await _hotelRepo.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(request.Keyword), x => x.searchkey!.Contains(request.Keyword!))
            .ToPageListAsync(request.PageIndex, request.PageSize, total,cancellationToken);
        List<HotelPageListDto> data = new();
        result.ForEach(item =>
        {
            data.Add(new HotelPageListDto()
            {
                Id=item.Id,
                HotelCode = item.hotelcode,
                HotelName = item.hotelname,
                Address = item.address,
                Telphone = item.telphone,
                Createtime = item.createtime,
                StatusName = item.status switch
                {
                    0 => "无效",
                    1 => "有效",
                    2 => "待审核",
                    3 => "已审核",
                    _ => ""
                },
                Status = item.status ?? 0
            });
        });
        return data;
    }
}
