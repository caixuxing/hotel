using Hotel.Domain.Enums;
using SqlSugar;

namespace Hotel.Domain.Entity
{
    [SugarTable("b_hotel", "酒店表")]
    [SugarIndex("index_{table}_S", nameof(searchkey), OrderByType.Asc)]
    public partial class HotelEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }


        /// <summary>
        /// 酒店编号
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? hotelcode { get; set; }


        /// <summary>
        /// 酒店名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? hotelname { get; set; }


        /// <summary>
        /// 酒店名称(英文)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? hotelnameen { get; set; }


        /// <summary>
        /// 国家id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? countryid { get; set; }


        /// <summary>
        /// 区域id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? areaid { get; set; }


        /// <summary>
        /// 星级
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? starlevel { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varchar(500)")]
        public string? address { get; set; }



        /// <summary>
        /// 地址(英文)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varchar(500)")]
        public string? addressen { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? telphone { get; set; }



        /// <summary>
        /// email
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? email { get; set; }


        /// <summary>
        /// 传真
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? fax { get; set; }


        /// <summary>
        /// 网址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varchar(500)")]
        public string? website { get; set; }



        /// <summary>
        /// gps
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? gpstype { get; set; }



        /// <summary>
        /// 纬度
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? latitude { get; set; }


        /// <summary>
        /// 经度
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? longitude { get; set; }


        /// <summary>
        /// 描叙
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "text")]
        public string? content { get; set; }



        /// <summary>
        /// 特色
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varchar(8000)")]
        public string? sellingpoint { get; set; }


        /// <summary>
        /// 状态(0无效 1有效 2待审核 3已审核)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? status { get; set; }


        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? createuser { get; set; }



        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? createtime { get; set; }


        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? modifyuser { get; set; }



        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? modifytime { get; set; }



        /// <summary>
        /// 上线时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? uptime { get; set; }


        /// <summary>
        /// 评论数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? commentnum { get; set; }



        /// <summary>
        /// 开业时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? opentime { get; set; }



        /// <summary>
        /// 检索关键词
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varchar(800)")]
        public string? searchkey { get; set; }


        /// <summary>
        /// 携程Code
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? ctripcode { get; set; }


        /// <summary>
        /// 携程名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? ctripcityname { get; set; }

        /// <summary>
        /// 携程状态
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? ctripstatus { get; set; }
    }
}
