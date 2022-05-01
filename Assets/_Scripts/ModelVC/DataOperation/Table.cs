using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace vts.data_operation
{
    public class Table
    {
        public Action onFileLoaded;
        public Action onFileSaved;
        private List<List<string>> content;

        public IEnumerable<List<string>> iterTable()
        {
            foreach (List<string> row in content)
            {
                yield return row;
            }
        }

        public int getRowNumber()
        {
            return content.Count;
        }

        #region 檔案讀取
        public void load(string path)
        {
            string table = Loader.readFile(path: path);

            // 分析檔案內容
            parseFile(content: table);
        }

        public async Task loadAsync(string path)
        {
            string table = await Loader.readFileAsync(path: path);

            // 分析檔案內容
            parseFile(content: table);

            onFileLoaded?.Invoke();
        }

        public void loadResourceFile(string path)
        {
            // 讀取 csv 二進制文件  
            TextAsset asset = Resources.Load(path, typeof(TextAsset)) as TextAsset;

            // 分析檔案內容
            parseFile(content: asset.text);
        }

        private void parseFile(string content)
        {
            this.content = new List<List<string>>();

            // 讀取每一行的內容  
            string[] lines = content.Split('\r');

            // 暫存每一行拆分後的內容
            string[] line;

            List<string> row;

            // 把 csv 中的數據儲存在二位數組中  
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    line = lines[i].Split(',');
                    row = new List<string>();

                    foreach(string element in line)
                    {
                        row.Add(element.Trim());
                    }

                    this.content.Add(row);
                }
                catch (IndexOutOfRangeException)
                {
                    Utils.error($"#lines[{i}]: {lines[i]}");
                }
            }
        }
        #endregion

        #region 檔案寫出
        public void loadContent(List<List<string>> content)
        {
            this.content = new List<List<string>>(content);
        }

        public void save(string path)
        {
            if (content == null)
            {
                return;
            }

            // 檢查檔案是否存在，不存在則建立
            StreamWriter writer;

            if (!File.Exists(path))
            {
                writer = new FileInfo(path).CreateText();
                Utils.log($"Create file: {path}");
            }
            else
            {
                writer = new FileInfo(path).AppendText();
            }

            string line;

            foreach(List<string> row in content)
            {
                line = string.Join(",", row);
                writer.WriteLine(line);
            }

            // Flush: 強制執行了一次把資料寫出硬碟，此時資料才確實寫入到了檔案中。避免如果程式突然中斷，資料尚未寫到檔案中，造成資料丟失。
            writer.Flush();

            // Close 負責關閉，Dispose 負責銷燬物件。Dispose 會負責 Close 的一切事務，額外還有銷燬物件的工作，即 Dispose 包含 Close
            // 但 StreamWriter 對 Close 覆寫，裡面實際呼叫 Dispose。兩者效果相同，則一呼叫即可。
            //writer.Close();
            writer.Dispose();

            Utils.log($"Save to {path}");
        } 

        public async Task saveAsync(string path)
        {
            if (content == null)
            {
                return;
            }

            StreamWriter writer = new StreamWriter(path: path, append: false, 
                                                   encoding: System.Text.Encoding.UTF8);
            string line;

            foreach (List<string> row in content)
            {
                line = string.Join(",", row);
                await writer.WriteLineAsync(line);
            }

            writer.Flush();
            writer.Dispose();

            Utils.log($"Save to {path}");
            onFileSaved?.Invoke();
        }
        #endregion
    }
}
