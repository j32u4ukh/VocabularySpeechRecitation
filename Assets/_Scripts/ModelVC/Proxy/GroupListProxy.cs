using System.IO;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class GroupListProxy : TableProxy
    {
        public GroupListProxy(string source) : base()
        {
            // 檔案載入完成監聽器
            table.onFileLoaded += () =>
            {
                Facade.getInstance().sendNotification(Notification.GroupListLoaded);
            };

            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{source}.csv");
            _ = table.loadAsync(path: path);
        }
    }
}
