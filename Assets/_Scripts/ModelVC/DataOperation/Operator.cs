using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

namespace VTS.DataOperation
{
    public class Operator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Table list = new Table();
            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", "EnTw.csv");
            list.load(path);

            int s, length = list.getRowNumber();

            for(s = 2; s < length; s++)
            {
                string source = list[s, 1];
                string file_path = Path.Combine(Application.streamingAssetsPath,
                                                "vocabulary",
                                                Utils.getLanguageCode(SystemLanguage.English),
                                                Utils.getLanguageCode(SystemLanguage.Chinese),
                                                $"{source}.csv");
                Utils.log($"file_path: {file_path}");

                Table table = new Table();

                try
                {
                    table.load(file_path);
                    Utils.log($"#row: {table.getRowNumber()}");
                }
                catch (TableInconsistentException tie)
                {
                    Utils.log($"TableInconsistentException: {s}");
                    List<List<string>> content = tie.getContent();
                    content.RemoveAt(index: content.Count - 1);

                    Table new_table = new Table();
                    new_table.loadContent(content);
                    new_table.save(path: file_path);
                }
            }
        }
    }
}