namespace Hotel.Application.Hotel.Query.PageList;

public class HotelPageListDto
{
    public int Id { get; set; }
    /// <summary>
    /// 酒店名称
    /// </summary>
    public  string HotelName { get; set; } = default!;
    /// <summary>
    /// 酒店编码
    /// </summary>
    public string HotelCode { get; set; } = default!;
    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; } = default!;

    /// <summary>
    /// 电话
    /// </summary>
    public string Telphone { get; set; } = default!;

    /// <summary>
    /// 状态值
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// 状态名
    /// </summary>
    public string StatusName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? Createtime { get; set; }
}
