using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace My_Schedule.Shared.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns the DisplayAttribute <c>Name</c> set for an <c>enum</c> parameter.
        /// </summary>
        /// <param name="value"> Enum parameter. </param>
        /// <returns> The DisplayAttribute <c>Name</c> set for an <c>enum</c> parameter. </returns>
        public static string GetDisplayName(Enum value)
        {
            return value.GetType()
                      .GetMember(value.ToString())
                      .FirstOrDefault()?
                      .GetCustomAttribute<DisplayAttribute>(false)?
                      .Name ?? value.ToString();
        }

        public static bool HasAttribute<T>(Enum enumValue)
            where T : Attribute
        {
            return enumValue.GetType()
                      .GetMember(enumValue.ToString())
                      .FirstOrDefault()?
                      .GetCustomAttribute<T>(false) != null;
        }

        /// <summary>
        /// Get the desciption of the field from [Description()]
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Returns [Description()] as string</returns>
        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
}