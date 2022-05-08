using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTS
{
    public static class ListExtension
    {
        public static List<List<T>> sort<T>(this List<List<T>> list2d, int order = 0)
        {
            return list2d.OrderBy(x => x[order]).ToList();
        }

        public static string toString<T>(this List<List<T>> list2d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            int i, len = list2d.Count;

            if (len > 0)
            {
                for (i = 0; i < len - 1; i++)
                {
                    sb.Append(string.Format("{0}, ", list2d[i].toString()));
                }

                sb.Append(string.Format("{0}", list2d[len - 1].toString()));
            }

            sb.Append("]");

            return sb.ToString();
        }

        // 返回格式化 List 的字串
        public static string toString<T>(this List<T> list, int digits = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            int i, len = list.Count;
            string format;

            // TODO: 或許可以改用 typeof(T).Name
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

        public static List<T> shuffle<T>(this List<T> list)
        {
            System.Random rand = new System.Random();

            return list.OrderBy(x => rand.Next()).ToList();
        }
    }
}
