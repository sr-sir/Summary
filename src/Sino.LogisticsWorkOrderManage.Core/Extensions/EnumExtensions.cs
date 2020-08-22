using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sino.LogisticsWorkOrderManage.Core
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举文本内容
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(this object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            Type enumType = typeof(TEnum);
            var name = Enum.GetName(enumType, Convert.ToInt32(value));
            if (name == null)
                return string.Empty;
            var descAttr = enumType.GetField(name).GetCustomAttribute<DescriptionAttribute>();
            return descAttr != null ? descAttr.Description : string.Empty;        
        }

        public static string GetEnumNameByValue<TEnum>(this int value) where TEnum : struct
        {
            return Enum.Parse(typeof(TEnum), value.ToString()).ToString();
        }

        public static TEnum? GetEnumByDesc<TEnum>(this string description) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }
            Type enumType = typeof(TEnum);
            //获取所有成员
            var fields = enumType.GetFields();
            foreach (var f in fields)
            {
                var attrs = f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    var temp = attrs[0] as DescriptionAttribute;
                    if (description == temp.Description)
                    {
                        return (TEnum)Enum.Parse(enumType, f.Name);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据枚举类型参数获取其值与描述
        /// </summary>
        /// <typeparam name="TEnum">枚举类型参数</typeparam>
        /// <param name="exceptMembers">排除对象</param>
        /// <returns></returns>
        public static IList<KeyValuePair<int, string>> GetEnumValueAndDesc<TEnum>(params int[] exceptMembers) where TEnum : struct
        {
            IList<KeyValuePair<int, string>> keyValues = new List<KeyValuePair<int, string>>();
            var enumType = typeof(TEnum);
            var fields = enumType.GetFields();
            DescriptionAttribute descAttr = null;
            foreach (var field in fields)
            {
                if (enumType == field.FieldType)
                {
                    descAttr = field.GetCustomAttribute<DescriptionAttribute>();
                    var value = (int)Enum.Parse(enumType, field.Name);
                    if (descAttr != null && !exceptMembers.Contains(value))
                    {
                        keyValues.Add(new KeyValuePair<int, string>(value, descAttr.Description));
                    }
                }
            }
            return keyValues;
        }
    }
}
