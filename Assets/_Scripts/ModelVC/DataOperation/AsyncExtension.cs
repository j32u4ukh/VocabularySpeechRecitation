using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace vts
{
    public static class AsyncExtension
    {
        // UnityWebRequestAsyncOperation 的方法擴充 for await request.SendWebRequest();
        public static UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOperation)
        {
            return new UnityWebRequestAsyncOperationAwaiter(asyncOperation);
        }

        // ResourceRequest 的方法擴充
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest asyncOperation)
        {
            return new ResourceRequestAwaiter(asyncOperation);
        }
    }

    public class UnityWebRequestAsyncOperationAwaiter : INotifyCompletion
    {
        UnityWebRequestAsyncOperation _asyncOperation;

        public bool IsCompleted
        {
            get
            {
                return _asyncOperation.isDone;
            }
        }

        public UnityWebRequestAsyncOperationAwaiter(UnityWebRequestAsyncOperation async_operation)
        {
            this._asyncOperation = async_operation;
        }

        // NOTE: 結果はUnityWebRequestからアクセスできるので、ここで返す必要性は無い
        public void GetResult()
        {

        }

        // await 後的部分會封裝成一個 Action continuation 在此執行
        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => { continuation(); };
        }
    }

    public class ResourceRequestAwaiter : INotifyCompletion
    {
        ResourceRequest async_operation;
        string result;

        public bool IsCompleted
        {
            get
            {
                return async_operation.isDone;
            }
        }

        public ResourceRequestAwaiter(ResourceRequest async_operation)
        {
            this.async_operation = async_operation;
            result = async_operation.asset.ToString();
        }

        // NOTE: 結果は UnityWebRequest からアクセスできるので、ここで返す必要性は無い
        public string GetResult()
        {
            return result;
        }

        // await 後的部分會封裝成一個 Action continuation 在此執行
        public void OnCompleted(Action continuation)
        {
            async_operation.completed += _ => { continuation(); };
        }
    }
}