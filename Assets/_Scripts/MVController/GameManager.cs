using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class GameManager : MonoBehaviour
    {
        public MainActivity main;

        private static GameManager instance = null;
        private List<IEnumerator> enumerators = null;
        [SerializeField] private GameObject reporter;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            reporter.SetActive(true);
#else
            reporter.SetActive(false);
#endif

            AppFacade.getInstance().init(main);

            SpeechFragmentMediator.onCreateGroup += getInstantiate;
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

        public void getInstantiate(GameObject prefab, Transform parent)
        {
            Utils.log();
            GameObject obj = Instantiate(prefab, parent);

        }

        public void destory<T>(T garbage)
        {
            Destroy(garbage as UnityEngine.GameObject);
        }
    }
}
