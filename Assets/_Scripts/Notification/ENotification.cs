namespace vts.mvc
{
    /// <summary>
    /// 在使用 PureMVC 時，使用字串傳遞通知訊息，為了方便呼叫防止寫錯，可以宣告一個通知名類用於管理所有通知名。
    /// </summary>
    public enum ENotification
    {
        Init,

        // 單字載入完成
        VocabularyLoaded,

        // ======
        None,
        Free
    }
}