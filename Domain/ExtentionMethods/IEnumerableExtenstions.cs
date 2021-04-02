using System.Collections;
using System.Text;

namespace Domain.ExtentionMethods
{
    public static class IEnumerableExtenstions
    {
        public static string BuildString(this IEnumerable enumerable)
        {
            StringBuilder result_string_builder = new StringBuilder();
            foreach (var item in enumerable)
                result_string_builder.Append($"{item}, ");

            return result_string_builder.ToString();
        }
    }
}
