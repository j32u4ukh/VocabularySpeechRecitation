using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace VTS.DataOperation
{
    public class Table : CharacterSeparatedValues
    {

        public Table() {}

        public Table(string content, params char[] separator) : base(content: content, separator: separator) {}

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
            parseFile(content: content, '\r');

            int n_column = this.content[0].Count;

            foreach (List<string> row in this.content)
            {
                if (!n_column.Equals(row.Count))
                {
                    throw new TableInconsistentException(array2d: this.content,
                                                         message: $"Table 的每一列(row)的欄位數(column)應相同，{n_column} -> {row.Count}");
                }
            }
        }
        #endregion

        public void release()
        {
            content.Clear();
        }
    }
}
