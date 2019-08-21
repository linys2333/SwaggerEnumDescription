using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SwaggerEnum.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var typeInfo = value.GetType().GetTypeInfo();
            var field = typeInfo.GetDeclaredField(value.ToString());
            if (field == null)
            {
                throw new ArgumentException("传入的值不属于正确的枚举定义值", nameof(value));
            }
            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}