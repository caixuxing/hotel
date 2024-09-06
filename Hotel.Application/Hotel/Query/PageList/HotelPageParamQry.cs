

namespace Hotel.Application.Hotel.Query.PageList;



public record HotelPageParamQry : IRequest<List<HotelPageListDto>>
{

    public string? Keyword { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}
