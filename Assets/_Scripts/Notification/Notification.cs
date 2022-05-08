using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTS
{
    public class Notification
    {
        #region MainActivity (一開始就是開著，會自行初始化，因此沒有 InitMainActivity 事件)
        // 切換到 MainActivity
        public const string OpenMainActivity = "OpenMainActivity";

        // 切換書籤
        public const string SwitchBookmark = "SwitchBookmark";

        // 要求載入單字組清單
        public const string InitGroupList = "InitGroupList";

        // 單字組清單載入完成
        public const string GroupListLoaded = "GroupListLoaded";
        #endregion

        #region InitSpeechActivity
        // 切換到 SpeechActivity
        public const string OpenSpeechActivity = "OpenSpeechActivity";

        // Group 載入完成
        public const string GroupLoaded = "GroupLoaded";

        // 單字載入完成
        public const string VocabularyLoaded = "VocabularyLoaded";

        // 念單字(與它的說明)
        public const string Speak = "Speak";

        // 念單字(與它的說明)
        public const string StopSpeaking = "StopSpeaking";

        // 當前單字念完
        public const string FinishedReading = "FinishedReading"; 

        // 觸發下一個單字的念誦
        public const string NextVocabulary = "NextVocabulary"; 
        #endregion
    }
}
