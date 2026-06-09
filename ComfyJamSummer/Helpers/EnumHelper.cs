using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Extensions
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }


        public static List<object> GetEnumValues<T>() where T : Enum
        {
            var enumType = typeof(T);
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            List<object> orderedEnumValues = new List<object>();

            foreach (var field in fields)
            {
                orderedEnumValues.Add(field.GetValue(null));
            }

            return orderedEnumValues;
        }

        /// <summary>
        /// without obsolete values
        /// </summary>
        /// <returns></returns>
        //public static ExampleEnum[] GetActiveObjects()
        //{
        //    return Enum.GetValues<ExampleEnum>().Where(i => !IsObsolete(i)).ToArray();
        //}

        public static bool IsObsolete(Enum item)
        {
            var member = typeof(Enum).GetMember(item.ToString())[0];
            return Attribute.IsDefined(member, typeof(ObsoleteAttribute));
        }
    }
}