using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Entity
{

    [SugarTable("ARHotel", "酒店与平台映射表")]
    [SugarIndex("index_{table}_C", nameof(CDate), OrderByType.Asc)]
    public partial class ARHotelEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; init; }

        /// <summary>
        /// 酒店Code
        /// </summary>
        public string HotelCode { get; set; } = null!;

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; } = null!;

        /// <summary>
        /// 追房平台类型
        /// </summary>
        public string OtherPlatType { get; set; } = null!;

        /// <summary>
        /// 追房平台Url
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "Nvarchar(1000)")]
        public string? OtherPlatformUrl { get; set; }

        /// <summary>
        /// 其他平台酒店标识
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? OtherHotelCode { get; private set; } 

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CDate { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperatorId { get; set; } = null!;

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; } = null!;

    }


    public partial class ARHotelEntity
    {

        public ARHotelEntity() { }
        private ARHotelEntity(int id, string hotelCode, string hotelName, string otherPlatType, string otherPlatformUrl,
            string otherHotelCode, DateTime cDate, string operatorId, string operatorName)
        {
            if (string.IsNullOrWhiteSpace(otherHotelCode))
            {
                throw new Exception("平台酒店标识不能为空!");
            }
            Id = id;
            HotelCode = hotelCode;
            HotelName = hotelName;
            OtherPlatType = otherPlatType;
            OtherPlatformUrl = otherPlatformUrl;
            OtherHotelCode = otherHotelCode;
            CDate = cDate;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        public static ARHotelEntity Create(string hotelCode, string hotelName, string otherPlatType, string otherPlatformUrl, string operatorId, string operatorName)
        {
            return new ARHotelEntity(0, hotelCode, hotelName, otherPlatType, otherPlatformUrl,
                otherPlatType.ResolveOtherPlatformUrl(otherPlatformUrl)
                , DateTime.Now, operatorId, operatorName);
        }

        /// <summary>
        /// 追房Url地址变更
        /// </summary>
        /// <param name="otherPlatformUrl"></param>
        public void ChangePlatformUrl(string otherPlatformUrl)
        {
            if (this.OtherPlatformUrl != otherPlatformUrl)
            {
                this.OtherPlatformUrl = otherPlatformUrl;
                if(!string.IsNullOrWhiteSpace(otherPlatformUrl))
                this.OtherHotelCode = this.OtherPlatType.ResolveOtherPlatformUrl(this.OtherPlatformUrl);
                else
                    this.OtherHotelCode = null;
            }
        }

        /// <summary>
        /// 设置追房平台Url
        /// </summary>
        /// <param name="otherPlatformUrl"></param>
        /// <returns></returns>
        public ARHotelEntity SetOtherPlatformUrl(string otherPlatformUrl)
        {
            this.OtherPlatformUrl = otherPlatformUrl;
            return this;
        }

        /// <summary>
        /// 设置其他平台酒店标识
        /// </summary>
        /// <param name="otherHotelCode"></param>
        /// <returns></returns>
        public ARHotelEntity SetOtherHotelCode(string otherHotelCode)
        {
            this.OtherHotelCode = otherHotelCode;
            return this;
        }
    }
}
