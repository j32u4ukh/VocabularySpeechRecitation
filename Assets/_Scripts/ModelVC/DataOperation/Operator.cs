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
        Action onFileLoaded;

        // Start is called before the first frame update
        void Start()
        {
            Table table = new Table();
            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", "EnTw.csv");
            List<List<string>> content = new List<List<string>>();
            List<string> row;

            for(int i = 0; i < 29; i++)
            {
                row = new List<string>() { $"EnTw{i}", $"table{i}" };
                content.Add(row);
            }

            table.loadContent(content);
            _ = table.saveAsync(path: path);

            table.onFileSaved += () => 
            {
                Utils.log("onFileSaved");
            };
        }
    }
}