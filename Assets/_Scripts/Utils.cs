using FantomLib;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace vts
{
    public class Utils
    {
        /// <summary>
        /// 可參考 AndroidLocale.cs 的 ConstantValues，或 http://fantom1x.blog130.fc2.com/blog-entry-295.html
        /// </summary>
        public readonly static Dictionary<SystemLanguage, string> language_codes = new Dictionary<SystemLanguage, string>()
        {
            { SystemLanguage.ChineseTraditional, AndroidLocale.Taiwan },
            { SystemLanguage.Chinese, AndroidLocale.Taiwan },
            { SystemLanguage.English, AndroidLocale.English },
            { SystemLanguage.Japanese, AndroidLocale.Japanese }
        };

        /// <summary>
        /// 嘗試取得語言代碼(ISO 639)，若未在列表中，則返回 null
        /// </summary>
        /// <param name="language">欲查詢的語言</param>
        /// <returns></returns>
        public static string getLanguageCode(SystemLanguage language)
        {
            language_codes.TryGetValue(key: language, out string code);

            return code;
        }

        public static IEnumerable<string> iterEnumArray<T>(params T[] enums)
        {
            foreach(T @enum in enums)
            {
                yield return @enum.ToString();
            }
        }

        #region 無法藉由點擊 Editor log 跳到實際腳本位置
        /// <summary>
        /// CallerLineNumber、CallerMemberName、CallerFilePath 等屬性只能置於參數位置，不能於函式內呼叫
        /// 參考網站: https://stackoverflow.com/questions/12556767/how-do-i-get-the-current-line-number
        /// </summary>
        /// <param name="message"></param>
        /// <param name="line_num"> CallerLineNumber: 實際呼叫的行數位置 </param>
        /// <param name="member"> CallerMemberName: 實際呼叫的函數名稱 </param>
        /// <param name="file_path"> CallerFilePath: 實際呼叫的腳本路徑 </param>
        /// <returns></returns>
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
