using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

namespace vts.data_operation
{
    public class Operator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Table table = new Table();
            List<List<string>> rows = new List<List<string>>();
            List<string> row;

            for (int i = 0; i < 29; i++)
            {
                row = new List<string>() 
                {
                    $"EnTw {i}",
                    "en",
                    "zh_TW",
                    $"table{i}"
                };

                rows.Add(row);
            }

            table.loadContent(rows);
            _ = table.saveAsync(Path.Combine(Application.streamingAssetsPath, "vocabulary", $"EnTw.csv"));
        }
    }
}