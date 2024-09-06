namespace Hotel.Application.Hotel.Query.FindARHotelByCode;


public record FindARHotelParamQry(string code):IRequest<List<FindARHotelDto>>;
