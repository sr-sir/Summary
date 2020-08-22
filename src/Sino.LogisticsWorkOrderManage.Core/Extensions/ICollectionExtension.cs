using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public static class ICollectionExtension
    {
        /// <summary>
        /// 枚举器转换成List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerator<T> enumerator)
        {
            List<T> retVal = null;
            if (enumerator != null)
            {
                retVal = new List<T>();
                while (enumerator.MoveNext())
                {
                    retVal.Add(enumerator.Current);
                }
            }
            return retVal;
        }

        /// <summary>
        /// 将集合对象转换成IEnumerable&lt;object&gt;对象
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="collection">源集合对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static IEnumerable<object> ChangeCollectionDataType<T>(this IEnumerable<T> collection, Type targetType)
        {
            return collection.Select(t => t.ChangeDataType(targetType));
        }
    }
}
