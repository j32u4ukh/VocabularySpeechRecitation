using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class VocabularyProxy : Proxy
    {
        SystemLanguage language;
        int table_number;

        List<VocabularyNorm> vocabularies;

        // key: 目標語言, value: 描述語言
        Dictionary<string, string> dictionary;

        /// <summary>
        /// 必須寫建構函式，在建構函式中必須呼叫父類的建構函式，Proxy中只提供了一個有參構造
        /// 可以在建構函式中從外部傳入資料data使用，也可以在建構函式中初始化資料
        /// </summary>
        public VocabularyProxy(string proxy_name, SystemLanguage language, int table_number) : base(proxy_name: proxy_name)
        {
            Utils.log($"ProxyName: {ProxyName}");
            this.language = language;
            this.table_number = table_number;
        }

        public override void onRegister()
        {
            string language_code = Utils.getLanguageCode(language: language);
            string path = $"vocabulary/{language_code}/table{table_number}";
            Utils.log($"path: {path}");

            // 讀取 csv 二進制文件  
            TextAsset asset = Resources.Load(path, typeof(TextAsset)) as TextAsset;

            // 讀取每一行的內容  
            string[] contents = asset.text.Split('\r');

            vocabularies = new List<VocabularyNorm>();

            // 初始化詞彙字典
            dictionary = new Dictionary<string, string>();

            // 把 csv 中的數據儲存在二位數組中  
            for (int i = 0; i < contents.Length; i++)
            {
                try
                {
                    string[] content = contents[i].Split(',');

                    vocabularies.Add(new VocabularyNorm(vocabulary: content[0], description: content[1]));
                    dictionary.Add(content[0], content[1]);
                }
                catch (IndexOutOfRangeException)
                {
                    Utils.error($"#contents[{i}]: {contents[i]}");
                }
            }

            Utils.log($"Vocabulary loaded, #vocabulary: {vocabularies.Count}");

            // 通知大家，單字已載入完畢
            AppFacade.getInstance().sendNotification(notification: ENotification.VocabularyLoaded);
        }

        public override void onRemove()
        {

        }

        public SystemLanguage getLanguage()
        {
            return language;
        }

        public VocabularyNorm getVocabulary(int index)
        {
            return vocabularies[index];
        }
    }
}

