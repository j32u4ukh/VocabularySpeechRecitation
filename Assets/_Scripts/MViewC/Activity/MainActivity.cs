using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class MainActivity : MonoBehaviour
    {
        [Header("Fragment")]
        #region Fragment
        public GameObject bookmark_fragment;
        public GameObject speech_fragment;
        public GameObject custom_fragment;
        public GameObject exam_fragment;
        public GameObject setting_fragment;
        #endregion

        [Header("Activity")]
        public GameObject speech_activity;

        private void OnEnable()
        {
            Utils.log();
        }

        // Start is called before the first frame update
        void Start()
        {
            Utils.log();
        }

        private void OnDisable()
        {
            Utils.log();
        }
    }
}
