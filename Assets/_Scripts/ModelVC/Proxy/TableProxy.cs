using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;
using VTS.DataOperation;

namespace VTS
{
    public class TableProxy : Proxy
    {
        protected Table table;

        public TableProxy() : base()
        {
            table = new Table();
        }

        public int getRowNumber()
        {
            return table.getRowNumber();
        }

        public IEnumerable<List<string>> iterTable()
        {
            if (table == null)
            {
                Utils.error("table == null");
                yield break;
            }

            IEnumerable<List<string>> content = table.iterContent();

            foreach (List<string> row in content)
            {
                yield return row;
            }
        }

        public (string vocabulary, string description) getRow(int row_index)
        {
            if (table == null)
            {
                Utils.error("table == null");
                return (null, null);
            }

            List<string> row = table.getRow(row_index);

            return (row[0], row[1]);
        }

        public override void release()
        {
            table.release();
            Facade.getInstance().expulsionProxy(proxy_name: proxy_name);
        }
    }
}
