namespace Hotel.Application.Hotel.Commands.CreateHotel;

public class HotelInputCmd : IRequest<bool>
{

    public string hotelCode { get; set; }

    public string hotelName { get; set; }

    public DateTime checkIn { get; set; }

    public DateTime? checkOut { get; set; }

    public decimal minPrice { get; set; }
    public decimal maxPrice { get; set; }
    public HotelStateType stateType { get; set; }

}
