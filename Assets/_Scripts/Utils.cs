using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace vts
{
    public class Utils
    {
        public static IEnumerable<string> iterEnumArray<T>(params T[] enums)
        {
            foreach(T @enum in enums)
            {
                yield return @enum.ToString();
            }
        }

        #region �L�k�ǥ��I�� Editor log �����ڸ}����m
        /* �H�U�ݩʥu��m��ѼƦ�m�A�����禡���I�s
         * CallerLineNumber: ��کI�s����Ʀ�m
         * CallerMemberName: ��کI�s����ƦW��
         * CallerFilePath: ��کI�s���}�����|
         * �ѦҺ���: https://stackoverflow.com/questions/12556767/how-do-i-get-the-current-line-number
         */
        static string debugMessage(string message, int line_num, string member, string file_path)
        {
            string[] split_path = file_path.Split('\\');
            string script_name = split_path[split_path.Length - 1].Split('.')[0];

            return string.Format("[{0}] {1} ({2}) | {3}", script_name, member, line_num, message);
        }

        public static void log(string message = "", [CallerLineNumber] int line_num = 0, [CallerMemberName] string member = "", [CallerFilePath] string file_path = "")
        {
            //message = string.Format("[{0}] ({1}) {2}\n{3}", member, line_num, message, file_path);
            Debug.Log(debugMessage(message, line_num, member, file_path));
        }

        public static void warn(string message, [CallerLineNumber] int line_num = 0, [CallerMemberName] string member = "", [CallerFilePath] string file_path = "")
        {
            //message = string.Format("[{0}] ({1}) {2}\n{3}", member, line_num, message, file_path);
            Debug.LogWarning(debugMessage(message, line_num, member, file_path));
        }

        public static void error(string message, [CallerLineNumber] int line_num = 0, [CallerMemberName] string member = "", [CallerFilePath] string file_path = "")
        {
            //message = string.Format("[{0}] ({1}) {2}\n{3}", member, line_num, message, file_path);
            Debug.LogError(debugMessage(message, line_num, member, file_path));
        }
        #endregion
    }
}
