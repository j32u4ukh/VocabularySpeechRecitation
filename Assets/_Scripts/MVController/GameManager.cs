using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        private List<IEnumerator> enumerators = null;

        private void Awake()
        {
            instance = this;
        }

        public static GameManager getInstance()
        {
            return instance;
        }

        public void addIEnumerator(IEnumerator enumerator)
        {
            if (enumerators == null)
            {
                enumerators = new List<IEnumerator>();
            }

            enumerators.Add(enumerator);

            if(enumerators.Count == 1)
            {
                StartCoroutine(executeIEnumerators());
            }
        }

        IEnumerator executeIEnumerators()
        {
            IEnumerator enumerator;

            while(enumerators.Count > 0)
            {
                enumerator = enumerators[0];
                yield return StartCoroutine(enumerator);
                enumerators.RemoveAt(0);
            }
        }

        /// <summary>
        /// 讓沒有掛載 MonoBehaviour 的腳本也可執行 Coroutine
        /// </summary>
        /// <param name="enumerator"></param>
        public void executeIEnumerator(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
    }
}
