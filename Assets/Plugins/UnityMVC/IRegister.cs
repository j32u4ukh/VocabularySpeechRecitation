using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IRegister
    {
        /// <summary>
        /// 設置實體名稱
        /// </summary>
        /// <param name="name"></param>
        void setName(string name);

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
        /// 移除實體前呼叫
        /// </summary>
        void onRemove();
    }
}