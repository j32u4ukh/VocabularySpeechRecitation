using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace VTS.DataOperation
{
    public class Loader
    {

        // PC 讀檔 & iOS 讀檔
        public static string readFile(string path)
        {
            string result = string.Empty;

            if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                using (StreamReader reader = new StreamReader(path: path, encoding: System.Text.Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                result = readAndroidFile(path: path);
            }
            else
            {
                Utils.warn("當前環境不為 Editor、iOS 或 Android，因此內容為空");
            }

            return result;
        }

        // Android 讀檔
        public static string readAndroidFile(string path)
        {
            UnityWebRequest request = UnityWebRequest.Get(path);
            _ = request.SendWebRequest();

            // isDone: 只代表結束，不代表是否成功
            while (!request.isDone) { }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }

            return null;
        }

        #region Async
        // PC 讀檔 & iOS 讀檔
        public static async Task<string> readFileAsync(string path)
        {
            string result = string.Empty;

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                using (StreamReader reader = new StreamReader(path: path, encoding: System.Text.Encoding.UTF8))
                {
                    result = await reader.ReadToEndAsync();
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                result = await readAndroidFileAsync(path: path);
            }
            else
            {
                Utils.warn("當前環境不為 Editor、iOS 或 Android，因此內容為空");
            }

            return result;
        }

        // Android 讀檔
        public static async Task<string> readAndroidFileAsync(string path)
        {
            UnityWebRequest request = UnityWebRequest.Get(path);
            Utils.log($"androidReadSvgAsync: {path}");

            await request.SendWebRequest();
            Utils.log("request sent.");

            // TODO: request.SendWebRequest 可等待的話，是否就不需要這行來等待了？
            // isDone: 只代表結束，不代表是否成功
            while (!request.isDone) 
            {
                Utils.log("Waiting...");
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }

            return null;
        } 
#endregion
    }
}


