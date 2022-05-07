using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VTS.DataOperation
{
    public class CharacterSeparatedValues
    {
        public string this[int row, int column]
        {
            get
            {
                return getData(row: row, column: column);
            }
            set
            {
                setData(row: row, column: column, value: value);
            }
        }

        protected List<List<string>> content;
        public Action onFileLoaded;
        public Action onFileSaved;

        public CharacterSeparatedValues()
        {

        }

        public CharacterSeparatedValues(string content, params char[] separator)
        {
            parseFile(content, separator);
        }

        protected void parseFile(string content, params char[] separator)
        {
            this.content = new List<List<string>>();

            // 讀取每一行的內容  
            string[] lines = content.Split(separator);

            // 暫存每一行拆分後的內容
            string[] columns;

            // 每一行處理過後的內容
            List<string> row;

            // 把 csv 中的數據儲存在二位數組中  
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    columns = lines[i].Split(',');
                    row = new List<string>();

                    foreach (string column in columns)
                    {
                        row.Add(column.Trim());
                    }

                    this.content.Add(row);
                }
                catch (IndexOutOfRangeException)
                {
                    Utils.error($"IndexOutOfRangeException, #lines[{i}]: {lines[i]}");
                }
                catch (Exception e)
                {
                    Utils.error($"Exception: {e}");
                }
            }
        }

        public IEnumerable<List<string>> iterContent()
        {
            foreach (List<string> row in content)
            {
                yield return row;
            }
        }

        #region 檔案寫出
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
            int i, len = content.Count - 1;

            for(i = 0; i < len; i++)
            {
                line = string.Join(",", content[i]);
                writer.WriteLine(line);
            }

            line = string.Join(",", content[len]);
            writer.Write(line);

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
            int i, len = content.Count - 1;

            for (i = 0; i < len; i++)
            {
                line = string.Join(",", content[i]);
                await writer.WriteLineAsync(line);
            }

            line = string.Join(",", content[len]);
            await writer.WriteAsync(line);

            writer.Flush();
            writer.Dispose();

            Utils.log($"Save to {path}");
            onFileSaved?.Invoke();
        }
        #endregion

        public void loadContent(List<List<string>> content)
        {
            this.content = new List<List<string>>(content);
        }

        public List<List<string>> getContent()
        {
            return new List<List<string>>(content);
        }

        public virtual void setData(int row, int column, string value)
        {
            checkRange(row: row, column: column);
            content[row][column] = value;
        }

        public virtual string getData(int row, int column)
        {
            checkRange(row: row, column: column);
            return content[row][column];
        }

        public List<string> getRow(int row_index)
        {
            return content[row_index];
        }

        public int getRowNumber()
        {
            return content.Count;
        }

        public int getColumnNumber(int row)
        {
            checkRowRange(row: row);
            return content[row].Count;
        }

        protected virtual void checkRange(int row, int column)
        {
            checkRowRange(row: row);

            int upper_bound = content[row].Count - 1;

            if (column < 0 || upper_bound < column)
            {
                throw new IndexOutOfRangeException(message: $"Column: {column} out of range(0 ~ {upper_bound})");
            }
        }

        protected virtual void checkRowRange(int row)
        {
            int upper_bound = content.Count - 1;

            if (row < 0 || upper_bound < row)
            {
                throw new IndexOutOfRangeException(message: $"Row: {row} out of range(0 ~ {upper_bound})");
            }
        }
    }
}