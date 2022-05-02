using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTS
{
    public class Notification
    {
        // 切換書籤
        public const string SwitchBookmark = "SwitchBookmark";

        // 初始化 SpeechActivity
        public const string InitSpeechActivity = "InitSpeechActivity";

        // 單字組清單載入完成
        public const string GroupListLoaded = "GroupListLoaded";

        // 單字載入完成
        public const string VocabularyLoaded = "VocabularyLoaded";

        // 念單字(與它的說明)
        public const string Speak = "Speak";

        // 當前單字念完
        public const string FinishedReading = "FinishedReading";
    }
}
