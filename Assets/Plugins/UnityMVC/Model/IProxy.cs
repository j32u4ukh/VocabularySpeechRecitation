namespace UnityMVC
{
    public interface IProxy : IRegister
    {
        /// <summary>
        /// 設置數據
        /// </summary>
        /// <param name="data"></param>
        public void setData(object data);

        /// <summary>
        /// 取得數據
        /// </summary>
        /// <returns></returns>
        public object getData();
    }
}
