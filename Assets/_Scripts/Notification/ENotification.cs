namespace vts.mvc
{
    /// <summary>
    /// 在使用 PureMVC 時，使用字串傳遞通知訊息，為了方便呼叫防止寫錯，可以宣告一個通知名類用於管理所有通知名。
    /// 相當於事件(event)
    /// </summary>
    public enum ENotification
    {
        Init,

        SwitchBookmark,

        // 單字組清單載入完成
        GroupListLoaded,

        // 初始化 SpeechActivity
        InitSpeech,

        // 單字載入完成
        VocabularyLoaded,

        // 念單字(與它的說明)
        Speak,

        // 當前單字念完
        FinishedReading,

        // 前往下一個單字
        NextWord,

        // ======
        None,
        Free
    }
}