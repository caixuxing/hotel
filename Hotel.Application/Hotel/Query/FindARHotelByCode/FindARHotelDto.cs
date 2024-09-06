namespace Hotel.Application.Hotel.Query.FindARHotelByCode;

public class FindARHotelDto
{
    /// <summary>
    /// 酒店Code
    /// </summary>
    public string? HotelCode { get; set; }

    /// <summary>
    /// 酒店名称
    /// </summary>
    public string? HotelName { get; set; }
    /// <summary>
    /// 追房平台类型
    /// </summary>
    public string? OtherPlatType { get; set; }

    /// <summary>
    /// 追房平台类型名称
    /// </summary>
    public string? OtherPlatTypeName { get; set; }

    /// <summary>
    /// 追房平台Url
    /// </summary>
    public string? OtherPlatformUrl { get; set; }
}
