namespace UnityMVC
{
    public interface IRegister
    {
        /// <summary>
        /// 設定實體名稱並進行註冊
        /// </summary>
        /// <returns></returns>
        public void register(string name);

        /// <summary>
        /// 取得實體名稱
        /// </summary>
        /// <returns></returns>
        public string getName();

        /// <summary>
        /// 註冊完成後呼叫
        /// </summary>
        void onRegister();

        /// <summary>
        /// 移除實體後呼叫
        /// </summary>
        void onRelease();
    }
}