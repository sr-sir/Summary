using Newtonsoft.Json.Converters;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// json属性值转为日期
    /// </summary>
    public class DateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// 类构造
        /// </summary>
        public DateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
