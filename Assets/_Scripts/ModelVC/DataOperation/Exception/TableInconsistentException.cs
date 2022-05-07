using System;
using System.Collections.Generic;

namespace VTS.DataOperation
{
    /// <summary>
    /// Table 的每一列(row)的欄位數(column)不相同時的例外
    /// </summary>
    public class TableInconsistentException : Exception
    {
        List<List<string>> array2d;

        public TableInconsistentException(List<List<string>> array2d) : base()
        {
            this.array2d = array2d;
        }

        public TableInconsistentException(List<List<string>> array2d, string message) : base(message: message)
        {
            this.array2d = array2d;
        }

        public List<List<string>> getContent()
        {
            return new List<List<string>>(array2d);
        }
    }
}
