namespace SwaggerEnum.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelString(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (char.IsUpper(str[0]))
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }

            return str;
        }
    }
}
