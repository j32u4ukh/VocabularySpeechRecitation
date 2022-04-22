public enum ReciteMode
{
    // 目標單字
    Word,

    // 目標單字的拼法
    Spelling,

    // 間隔空白
    Interval,

    // 描述語言的說明
    Description
}

public enum State
{
    Idle,

    // 狀態改變完成
    Status,

    // 開始念誦
    Start,

    // 結束念誦
    Done,

    // 中止念誦(可能原因：被後面的指令中斷而沒有執行)
    Stop
}