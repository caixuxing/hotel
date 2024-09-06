using Hotel.Domain.Enums;
using NetTaste;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hotel.Domain
{
    public static class Utils
    {

        /// <summary>
        ///平台Url解析字典
        /// </summary>
        private static readonly Lazy<Dictionary<PlatTypeEnums, Func<string, string>>> _InitData = new(() => new() {
            {PlatTypeEnums.EXP, (otherPlatformUrl)=>RegexStr(otherPlatformUrl,".h\\d+.")},
            {PlatTypeEnums.SameTrip, (otherPlatformUrl)=>RegexStr(otherPlatformUrl,".?hotelId=\\d+&")},
            {PlatTypeEnums.owl, (otherPlatformUrl)=>RegexStr(otherPlatformUrl,"-d\\d+-")},
            {PlatTypeEnums.Agoda, (otherPlatformUrl)=>new Uri(otherPlatformUrl).LocalPath.Substring(1)}
        });


        /// <summary>
        /// 解析平台Url
        /// </summary>
        /// <param name="otherPlatType"></param>
        /// <param name="otherPlatformUrl"></param>
        /// <returns></returns>
        public static string ResolveOtherPlatformUrl(this string otherPlatType, string otherPlatformUrl)
        {
            var func = _InitData.Value.SingleOrDefault(x => x.Key == (PlatTypeEnums)Enum.Parse(typeof(PlatTypeEnums), otherPlatType)).Value;
            if (func == null)
            {
                throw new("没有找到可解析的方法");
            }
            var result = func(otherPlatformUrl);
            return result ?? string.Empty;
        }

        private static string RegexStr(string url, string regex)
        {
            var match = Regex.Matches(url, $@"{regex}");
            if (match.Count < 1) return string.Empty;
            var val = match[0].Value;
            return Regex.Replace(val, @"[^\d]", "");
        }
    }
}
