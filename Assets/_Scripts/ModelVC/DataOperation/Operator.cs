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

            string source = "vocabulary";
            string file_path = Path.Combine(Application.streamingAssetsPath,
                                            "vocabulary",
                                            Utils.getLanguageCode(SystemLanguage.English),
                                            Utils.getLanguageCode(SystemLanguage.Chinese),
                                            $"{source}.csv");
            Utils.log($"file_path: {file_path}");

            string content = Loader.readFile(path: file_path);
            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            csv.parseFile(content: content, ',');
            int n_row = csv.getRowNumber();
            Utils.log($"#row: {n_row}");

            List<List<string>> table = csv.getContent();
            table = table.shuffle();

            List<List<string>> new_content;
            int i, j, idx, n_file = 29, file_size = 100;

            for(i = 0; i < n_file; i++)
            {
                new_content = new List<List<string>>();

                for (j = 0; j < file_size; j++)
                {
                    idx = 100 * i + j;

                    if(idx < n_row)
                    {
                        new_content.Add(table[idx]);
                    }
                }

                new_content = new_content.sort();

                Table new_table = new Table();
                new_table.loadContent(new_content);
                new_table.save(Path.Combine(Application.streamingAssetsPath,
                                            "vocabulary",
                                            Utils.getLanguageCode(SystemLanguage.English),
                                            Utils.getLanguageCode(SystemLanguage.Chinese),
                                            $"table{i}.csv"));
            }
        }
    }
}