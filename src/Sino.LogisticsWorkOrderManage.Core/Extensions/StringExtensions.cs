using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static int GetCode(this string Str)
        {
            if (!string.IsNullOrWhiteSpace(Str))
            {
                return int.Parse(Str.Substring(1));
            }
            return 10000;
        }


        /// <summary>
        /// 值转换成目标类型值
        /// </summary>
        /// <param name="obj">需转换的值</param>
        /// <param name="type">转换结果类型</param>
        /// <returns></returns>
        public static object ChangeDataType(this object obj, Type type)
        {
            if (obj.GetType() == type)
            {
                return obj;
            }

            if (type == typeof(string))
            {
                return obj.ToString();
            }
            else if (type == typeof(DateTime?) && obj != null)
            {
                DateTime? temp = null;
                DateTime dateTemp;
                if (DateTime.TryParse(obj.ToString(), out dateTemp))
                {
                    temp = dateTemp;
                }
                return temp;
            }
            else if (type == typeof(DateTime))
            {
                DateTime temp;
                if (DateTime.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(int?) || type == typeof(int))
            {
                int temp;
                if (int.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(int?) || type == typeof(int))
            {
                int temp;
                if (int.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(float?) || type == typeof(float))
            {
                float temp;
                if (float.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(double?) || type == typeof(double))
            {
                double temp;
                if (double.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(decimal?) || type == typeof(decimal))
            {
                decimal temp;
                if (decimal.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(bool?) || type == typeof(bool))
            {
                bool temp;
                if (bool.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            return null;
        }

      
    }
}
