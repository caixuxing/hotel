namespace Hotel.Application.Hotel.Commands.SaveARHotel;

public class SaveARHotelCmd : IRequest<bool>
{
    public List<ARHotelCmd> data { get; set; }
}


public class ARHotelCmd
{
    public string HotelCode { get; set; }
    public string HotelName { get; set; }
    public string OtherPlatType { get; set; }
    public string OtherPlatformUrl { get; set; }
}
