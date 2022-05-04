using System.Collections.Generic;
using UnityMVC;
using VTS.DataOperation;

namespace VTS
{
    public class GroupListProxy : Proxy
    {
        Table table;

        public GroupListProxy(string path) : base()
        {
            table = new Table();

            // 檔案載入完成監聽器
            table.onFileLoaded += () =>
            {
                Facade.getInstance().sendNotification(Notification.GroupListLoaded);
            };

            _ = table.loadAsync(path: path);
        }

        public int getRowNumber()
        {
            return table.getRowNumber();
        }

        public IEnumerable<List<string>> iterTable()
        {
            if(table == null)
            {
                Utils.error("table == null");
                yield break;
            }

            IEnumerable<List<string>> content = table.iterTable();

            foreach (List<string> row in content)
            {
                yield return row;
            }
        }

        public override void release()
        {

        }
    }
}
