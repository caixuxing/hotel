using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Extend
{

    public class JsonDynamicConverter : ISugarDataConverter
    {
        public SugarParameter ParameterConverter<T>(object value, int i)
        {
            //该功能ORM自带的IsJson就能实现这里只是用这个用例来给大家学习
            var name = "@myp" + i;
            var str = "null";
            if (value != null && value.GetType() == typeof(string))
            {
                str = $"{value}";
            }
            else
            {
                //str = new SerializeService().SerializeObject(value);
                str = JsonConvert.SerializeObject(value, NewtonsoftJsonSerializerProviderExt.Newtosoft_Json_JsonSerializerSettings);
            }

            //可以更改DbType=System.Data.DbType.XXX
            //PgSql中可以使用:CustomDbType=NpgsqlDbType.XXX;
            return new SugarParameter(name, str);
        }

        public T? QueryConverter<T>(IDataRecord dr, int i)
        {
            var str = $"{dr.GetValue(i)}".Trim();
            if (str.StartsWith("\""))
            {
                //str = new SerializeService().DeserializeObject<string>(str);
                str = JsonConvert.DeserializeObject<string>(str, NewtonsoftJsonSerializerProviderExt.Newtosoft_Json_JsonSerializerSettings);
            }
            //return new SerializeService().DeserializeObject<T>(str);
            return JsonConvert.DeserializeObject<T>(str, NewtonsoftJsonSerializerProviderExt.Newtosoft_Json_JsonSerializerSettings);
        }
    }

    public class NewtonsoftJsonSerializerProviderExt
    {
        /// <summary>
        /// 
        /// </summary>
        public static JsonSerializerSettings Newtosoft_Json_JsonSerializerSettings { get; }
        static NewtonsoftJsonSerializerProviderExt()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // 首字母小写（驼峰样式）
                DateFormatString = "yyyy-MM-dd HH:mm:ss", // 时间格式化
                                                          // options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                                                          // options.SerializerSettings.DateParseHandling = DateParseHandling.None;
                                                          //StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                StringEscapeHandling = StringEscapeHandling.Default,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 忽略循环引用
                NullValueHandling = NullValueHandling.Ignore, // 忽略空值
                MaxDepth = 20, // 设置序列化的最大层数
#if DEBUG
                Formatting = Formatting.Indented,
#endif
            };

            jsonSerializerSettings.Converters.Add(new StringEnumConverter() { AllowIntegerValues = true });
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss", /*DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal*/ });
            //jsonSerializerSettings.Converters.AddLongTypeConverters(); // long转string（防止js精度溢出） 超过16位开启

            Newtosoft_Json_JsonSerializerSettings = jsonSerializerSettings;
        }
    }
}
