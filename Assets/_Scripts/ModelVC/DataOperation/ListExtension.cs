using System.Collections.Generic;
using System.Text;

namespace VTS
{
    public static class ListExtension
    {
        // 返回格式化 List 的字串
        public static string toString<T>(this List<T> list, int digits = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            int i, len = list.Count;
            string format;

            /* Type to string 
             * float: System.Single
             * double: System.Double
             * int: System.Int32
             * string: System.String
             * List<string>: System.Collections.Generic.List`1[System.String]
             * string[]: System.String[]
             */
            switch (typeof(T).ToString())
            {
                case "System.Single":
                case "System.Double":
                    format = $"{{0:F{digits}}}";
                    break;
                default:
                    format = "{0}";
                    break;
            }

            if (len > 0)
            {
                for (i = 0; i < len - 1; i++)
                {
                    sb.Append(string.Format($"{format}, ", list[i].ToString()));
                }

                sb.Append(string.Format($"{format}", list[len - 1].ToString()));
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
