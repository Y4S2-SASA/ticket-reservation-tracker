using System.ComponentModel;
using System.Reflection;

namespace TRT.Application.Common.Helpers
{
    public class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attribute = fi.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
