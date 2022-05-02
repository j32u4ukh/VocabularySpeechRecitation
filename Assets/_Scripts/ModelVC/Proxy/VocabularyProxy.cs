using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class VocabularyProxy : Proxy
    {
        // 目標語言
        SystemLanguage target;

        // 描述語言
        SystemLanguage describe;

        string table_name;

        List<VocabularyNorm> vocabularies;

        // key: 目標語言, value: 描述語言
        Dictionary<string, string> dictionary;

        /// <summary>
        /// 必須寫建構函式，在建構函式中必須呼叫父類的建構函式，Proxy中只提供了一個有參構造
        /// 可以在建構函式中從外部傳入資料data使用，也可以在建構函式中初始化資料
        /// </summary>
        public VocabularyProxy(string proxy_name, SystemLanguage target, SystemLanguage describe, string table_name)
        {
            this.target = target;
            this.describe = describe;
            this.table_name = table_name;
        }

        #region Life cycle
        public override void onRegister()
        {
            string target_code = Utils.getLanguageCode(language: target);
            string describe_code = Utils.getLanguageCode(language: describe);

            string path = $"vocabulary/{target_code}/{describe_code}/{table_name}";
            Utils.log($"path: {path}");

            // 讀取 csv 二進制文件  
            TextAsset asset = Resources.Load(path, typeof(TextAsset)) as TextAsset;

            // 讀取每一行的內容  
            string[] contents = asset.text.Split('\r');

            vocabularies = new List<VocabularyNorm>();

            // 初始化詞彙字典
            dictionary = new Dictionary<string, string>();

            string[] content;

            // 把 csv 中的數據儲存在二位數組中  
            for (int i = 0; i < contents.Length; i++)
            {
                try
                {
                    content = contents[i].Split(',');
                    vocabularies.Add(new VocabularyNorm(vocabulary: content[0].Trim(), description: content[1].Trim()));
                    dictionary.Add(content[0].Trim(), content[1].Trim());
                }
                catch (IndexOutOfRangeException)
                {
                    Utils.error($"#contents[{i}]: {contents[i]}");
                }
            }

            Utils.log($"Vocabulary loaded, #vocabulary: {vocabularies.Count}");

            // 通知大家，單字已載入完畢
            Facade.getInstance().sendNotification(notification_name: Notification.VocabularyLoaded);
        }
        #endregion

        public SystemLanguage getTargetLanguage()
        {
            return target;
        }

        public SystemLanguage getDescribeLanguage()
        {
            return describe;
        }

        public VocabularyNorm getVocabulary(int index)
        {
            return vocabularies[index];
        }
    }
}

