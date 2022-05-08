using System.IO;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class GroupListProxy : TableProxy
    {
        string source;

        public GroupListProxy(string source) : base()
        {
            load(source: source);
        }

        public void load(string source)
        {
            this.source = source;

            // 檔案載入完成監聽器
            table.onFileLoaded += () =>
            {
                Facade.getInstance().sendNotification(Notification.GroupListLoaded);
            };

            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{source}.csv");
            _ = table.loadAsync(path: path);
        }

        public string getSource()
        {
            return source;
        }
    }
}
