namespace Hotel.Application.Hotel.Commands.CreateHotel;

internal sealed class CreateHotelHandler : IRequestHandler<HotelInputCmd, bool>
{
    private readonly ISqlSugarRepository<HotelEntity> _hotelRepo;

    public CreateHotelHandler(ISqlSugarRepository<HotelEntity> hotelRepo)=> _hotelRepo = hotelRepo;
  

    public async Task<bool> Handle(HotelInputCmd request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(true);
    }
}
