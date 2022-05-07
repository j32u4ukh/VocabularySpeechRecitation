using System.IO;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class GroupProxy : TableProxy
    {
        string source;

        public GroupProxy(string source) : base()
        {
            this.source = source;

            // 檔案載入完成監聽器
            table.onFileLoaded += () =>
            {
                Facade.getInstance().sendNotification(Notification.GroupLoaded);
            };

            string target = Utils.getLanguageCode(language: Config.target);
            string describe = Utils.getLanguageCode(language: Config.describe);
            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", target, describe, $"{source}.csv");
            _ = table.loadAsync(path: path);
        }

        public string getSource()
        {
            return source;
        }
    }
}
